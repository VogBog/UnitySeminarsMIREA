using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using MainGame;
using Unity.Netcode;
using UnityEngine;

namespace Pool
{
    public class LocalMultiplayerObjectPoolNetworkObject : NetworkBehaviour, IObjectPool
    {
        private IObjectPool _pool;
        private PlayersRepo _playersRepo;
        private readonly Dictionary<Type, Dictionary<Component, int>> _livingComponents = new();

        public void SetPool(IObjectPool pool)
        {
            _pool = pool;
            _playersRepo = this.FindFirstObjectByTypeOrException<PlayersRepo>();
        }
        
        public void RegisterPrefab(Type type, PooledPrefab prefab)
        {
            _pool.RegisterPrefab(type, prefab);
        }

        public void CreateInstances(Type type, int count)
        {
            _pool.CreateInstances(type, count);
        }

        public void RegisterAndInstantiatePrefab(Type type, PooledPrefab prefab, int count)
        {
            _pool.RegisterAndInstantiatePrefab(type, prefab, count);
        }

        public T Spawn<T>(Vector3 position, Quaternion rotation) where T : Component
        {
            var result = _pool.Spawn<T>(position, rotation);
            var type = typeof(T).FullName;
            int index = _playersRepo.PlayerIndex;
            
            SpawnServerRpc(type, position, rotation, index);

            return result;
        }

        public Component Spawn(Vector3 position, Quaternion rotation, Type type)
        {
            var result = _pool.Spawn(position, rotation, type);
            var typeName = type.FullName;
            int index = _playersRepo.PlayerIndex;
            
            SpawnServerRpc(typeName, position, rotation, index);

            return result;
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void SpawnServerRpc(string typeName, Vector3 position, Quaternion rotation, int ownerIndex)
        {
            int componentId = 0;
            var type = Type.GetType(typeName);
            if (type == null)
                return;

            if (_livingComponents.TryGetValue(type, out var dict))
            {
                var keys = dict.Values.OrderBy(x => x);
                foreach (var id in keys)
                {
                    if (componentId != id)
                        break;
                    componentId++;
                }
            }
            else
            {
                componentId = 0;
            }
            
            SpawnClientRpc(typeName, position, rotation, ownerIndex, componentId);
        }

        [ClientRpc]
        private void SpawnClientRpc(string typeName, Vector3 position, Quaternion rotation,
            int ownerIndex, int componentId)
        {
            if (_playersRepo.PlayerIndex == ownerIndex)
                return;
            
            var type = Type.GetType(typeName);
            if (type == null)
                return;

            var comp = _pool.Spawn(position, rotation, type);

            if (!_livingComponents.TryGetValue(type, out var dict))
            {
                dict = new Dictionary<Component, int>();
                _livingComponents.Add(type, dict);
            }
                
            dict.Add(comp, componentId);
        }

        public void Despawn<T>(T component) where T : Component
        {
            Despawn(component, typeof(T));
        }

        public void Despawn(Component component, Type type)
        {
            _pool.Despawn(component, type);
            var typeName = type.FullName;
            if (!_livingComponents.TryGetValue(type, out var dict))
                return;

            int id = dict[component];
            int playerIndex = _playersRepo.PlayerIndex;
            
            DespawnServerRpc(typeName, id, playerIndex);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DespawnServerRpc(string typeName, int componentId, int ownerIndex)
        {
            DespawnClientRpc(typeName, ownerIndex, componentId);
        }

        [ClientRpc]
        private void DespawnClientRpc(string typeName, int componentId, int ownerIndex)
        {
            var type = Type.GetType(typeName);
            if (type == null || !_livingComponents.TryGetValue(type, out var dict))
                return;
            
            var comp = dict.FirstOrDefault(x => x.Value == componentId);
            if (comp.Key == null)
                return;
            
            dict.Remove(comp.Key);

            if (_playersRepo.PlayerIndex == ownerIndex)
                return;
            
            _pool.Despawn(comp.Key, type);
        }
    }
}