using System;
using Extensions;
using Player;
using Unity.Netcode;

namespace MainGame.NetworkMessaging
{
    public class LocalMultiplayerMessages : NetworkBehaviour
    {
        public event Action<PlayerHealth, Player.Player> PlayerKilledByAnotherPlayer;
        public event Action<Player.Player, PlayerHealth, int> PlayerHealthChanged;
        public event Action<Player.Player> PlayerDied;

        private PlayersRepo _repo;

        private void Awake()
        {
            _repo = this.FindFirstObjectByTypeOrException<PlayersRepo>();
        }

        #region PlayerKilledByAnotherPlayer
        public void InvokePlayerKilledByAnotherPlayer(PlayerHealth playerHealth, Player.Player player)
        {
            int killed = _repo.Get(playerHealth.Owner);
            int killer = _repo.Get(player);
            InvokePlayerKilledByAnotherPlayerServerRpc(killed, killer);
        }

        [ServerRpc]
        private void InvokePlayerKilledByAnotherPlayerServerRpc(int killed, int killer)
        {
            InvokePlayerKilledByAnotherPlayerClientRpc(killed, killer);
        }

        [ClientRpc]
        private void InvokePlayerKilledByAnotherPlayerClientRpc(int killed, int killer)
        {
            var killedHealth = _repo.Get(killed)?.Health;
            var killerPlayer = _repo.Get(killer);
            PlayerKilledByAnotherPlayer?.Invoke(killedHealth, killerPlayer);
        }
        #endregion

        #region PlayerHealthChanged
        public void InvokePlayerHealthChanged(Player.Player player, PlayerHealth playerHealth, int value)
        {
            int playerIndex = _repo.Get(player);
            InvokePlayerHealthChangedServerRpc(playerIndex, value);
        }

        [ServerRpc]
        private void InvokePlayerHealthChangedServerRpc(int playerIndex, int value)
        {
            InvokePlayerHealthChangedClientRpc(playerIndex, value);
        }

        [ClientRpc]
        private void InvokePlayerHealthChangedClientRpc(int playerIndex, int value)
        {
            var player = _repo.Get(playerIndex);
            PlayerHealthChanged?.Invoke(player, player.Health, value);
        }
        #endregion

        #region PlayerDied
        public void InvokePlayerDied(Player.Player player)
        {
            int playerIndex = _repo.Get(player);
            InvokePlayerDiedServerRpc(playerIndex);
        }

        [ServerRpc]
        private void InvokePlayerDiedServerRpc(int playerIndex)
        {
            InvokePlayerDiedClientRpc(playerIndex);
        }

        [ClientRpc]
        private void InvokePlayerDiedClientRpc(int playerIndex)
        {
            var player = _repo.Get(playerIndex);
            PlayerDied?.Invoke(player);
        }
        #endregion
    }
}