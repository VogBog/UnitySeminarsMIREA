using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace MainGame.PlayersRepos
{
    public class LocalMultiplayerPlayersRepo : NetworkBehaviour, IPlayersRepo
    {
        private IPlayersRepo _repo;
        private int _lastIndex = 0;
        private ulong _code;
        
        public event Action<int> PlayersCountChanged;
        public int Count => _repo.Count;
        public int PlayerIndex { get; private set; }
        public void Initialize(EventBus eventBus)
        {
            _repo = new DefaultPlayersRepo();
            _repo.Initialize(eventBus);
        }

        public void RegisterPlayer(Player.Player player, bool isMy)
        {
            _repo.RegisterPlayer(player, isMy);
            if (isMy)
            {
                _code = NetworkManager.LocalClientId;

                var obj = player.GetComponent<NetworkObject>();
                RegisterPlayerServerRpc(_code);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void RegisterPlayerServerRpc(ulong code)
        {
            RegisterPlayerClientRpc(code, _lastIndex++);
        }

        [ClientRpc]
        private void RegisterPlayerClientRpc(ulong code, int index)
        {
            if (_code == code)
            {
                PlayerIndex = index;
            }

            StartCoroutine(RegisterPlayerRoutine(code, index));
        }

        private IEnumerator RegisterPlayerRoutine(ulong playerId, int index)
        {
            for (int i = 0; i < 120; i++)
            {
                var obj = 
                    NetworkManager.SpawnManager.PlayerObjects
                        .FirstOrDefault(x => x.OwnerClientId == playerId);
                
                if (obj == null)
                {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }

                if (_repo.Count != index)
                {
                    yield return new WaitForSeconds(0.2f);
                    continue;
                }

                var player = obj.GetComponent<Player.Player>();
                bool isMy = obj.IsOwner;
                _repo.RegisterPlayer(player, isMy);

                yield break;
            }
            
            Debug.LogWarning("LocalMultiplayerPlayersRepo::RegisterPlayerRoutine - something went wrong");
        }

        public List<Player.Player> GetPlayersCopy() => _repo.GetPlayersCopy();

        public Player.Player Get(int index) => _repo.Get(index);

        public int Get(Player.Player player) => _repo.Get(player);
    }
}