using System;
using Extensions;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace MainGame.GameFinishers
{
    public class LocalMultiplayerGameFinisher : NetworkBehaviour, IGameFinisher
    {
        private PlayersRepo _repo;
        
        public event Action<Player.Player> PlayerWinned;
        public event Action Finished;

        private void Awake()
        {
            _repo = this.FindFirstObjectByTypeOrException<PlayersRepo>();
        }

        public bool CanFinishGame() => NetworkManager.IsHost;

        public void InvokePlayerWinned(Player.Player player)
        {
            if (!NetworkManager.IsHost)
                return;

            int index = _repo.Get(player);
            InvokePlayerWinnedClientRpc(index);
        }

        [ClientRpc]
        private void InvokePlayerWinnedClientRpc(int id)
        {
            var player = _repo.Get(id);
            PlayerWinned?.Invoke(player);
        }

        public void InvokeFinished()
        {
            if (!NetworkManager.IsHost)
                return;

            InvokeFinishedClientRpc();
        }

        [ClientRpc]
        private void InvokeFinishedClientRpc()
        {
            Finished?.Invoke();
        }

        public void LoadScene(int sceneIndex)
        {
            if (!NetworkManager.IsHost)
                return;

            var sceneName = SceneManager.GetSceneByBuildIndex(sceneIndex).name;
            NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}