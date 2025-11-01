using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace MainGame.PlayersRepos
{
    public class LocalMultiplayerPlayersRepo : NetworkBehaviour, IPlayersRepo
    {
        private IPlayersRepo _repo;
        private int _lastIndex = 0;
        
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
                int code = UnityEngine.Random.Range(1_000, 99_000);
                code += Count * 100_000;
                PlayerIndex = code;
                GetPlayerIndexServerRpc(code);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void GetPlayerIndexServerRpc(int code)
        {
            GetPlayerIndexClientRpc(code, _lastIndex++);
        }

        [ClientRpc]
        private void GetPlayerIndexClientRpc(int code, int index)
        {
            if (PlayerIndex != code)
                return;
            PlayerIndex = index;
        }

        public List<Player.Player> GetPlayersCopy() => _repo.GetPlayersCopy();

        public Player.Player Get(int index) => _repo.Get(index);

        public int Get(Player.Player player) => _repo.Get(player);
    }
}