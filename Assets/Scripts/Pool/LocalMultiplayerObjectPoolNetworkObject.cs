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
        private readonly Dictionary<Type, Dictionary<Component, (int, int)>> _livingComponents = new();

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

        private void AddToLivingComponents(Type type, Component component, int playerId, int componentId)
        {
            if (!_livingComponents.TryGetValue(type, out var dict))
            {
                dict = new Dictionary<Component, (int, int)>();
                _livingComponents.Add(type, dict);
            }
            
            dict.Add(component, (playerId, componentId));
        }

        private int GetFreeId(Type type, int playerId)
        {
            int componentId = 0;
            
            if (_livingComponents.TryGetValue(type, out var dict))
            {
                var keys = dict.Values.OrderBy(x => x);
                foreach (var (_, id) in keys)
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

            return componentId;
        }

        public void Spawn<T>(Vector3 position, Quaternion rotation, Action<T> onSpawn) where T : Component
        {
            int index = _playersRepo.PlayerIndex;
            int componentId = GetFreeId(typeof(T), index);
            _pool.Spawn<T>(position, rotation, result =>
            {
                AddToLivingComponents(typeof(T), result, index, componentId);
                onSpawn?.Invoke(result);
            });
            
            var type = typeof(T).FullName;
            SpawnServerRpc(type, position, rotation, index, componentId);
        }

        public void Spawn(Vector3 position, Quaternion rotation, Type type, Action<Component> onSpawn)
        {
            int index = _playersRepo.PlayerIndex;
            int componentId = GetFreeId(type, index);
            
            _pool.Spawn(position, rotation, type, result =>
            {
                AddToLivingComponents(type, result, index, componentId);
                onSpawn?.Invoke(result);
            });
            
            var typeName = type.FullName;
            SpawnServerRpc(typeName, position, rotation, index, componentId);
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void SpawnServerRpc(string typeName, Vector3 position, Quaternion rotation, int ownerIndex, int componentId)
        {
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

            _pool.Spawn(position, rotation, type, comp =>
            {
                AddToLivingComponents(type, comp, ownerIndex, componentId);
            });
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

            var (playerId, id) = dict[component];
            int playerIndex = _playersRepo.PlayerIndex;
            
            DespawnServerRpc(typeName, playerId, id, playerIndex);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DespawnServerRpc(string typeName, int playerId, int componentId, int ownerIndex)
        {
            DespawnClientRpc(typeName, playerId, componentId, ownerIndex);
        }

        [ClientRpc]
        private void DespawnClientRpc(string typeName, int playerId, int componentId, int ownerIndex)
        {
            var type = Type.GetType(typeName);
            if (type == null || !_livingComponents.TryGetValue(type, out var dict))
                return;
            
            var comp = dict.FirstOrDefault(x =>
                x.Value == (playerId, componentId));
            if (comp.Key == null)
                return;
            
            dict.Remove(comp.Key);

            if (_playersRepo.PlayerIndex == ownerIndex)
                return;
            
            _pool.Despawn(comp.Key, type);
        }
    }
}