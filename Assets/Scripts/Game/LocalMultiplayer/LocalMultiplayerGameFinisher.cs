using Unity.Netcode;
using UnityEngine;

namespace Game.LocalMultiplayer
{
    public class LocalMultiplayerGameFinisher : IGameFinisher
    {
        public CarMapRunner[] GetRunnersForRecord()
        {
            var players = Object.FindObjectsByType<CarMapRunner>(FindObjectsSortMode.None);
            foreach (var player in players)
            {
                if (player.TryGetComponent(out NetworkObject networkObject) &&
                    networkObject.OwnerClientId == NetworkManager.Singleton.LocalClientId)
                {
                    return new [] { player };
                }
            }

            return null;
        }

        public bool CanFinish() => NetworkManager.Singleton.IsServer;
        
        public void OnBeforeLoadingScene()
        {
            NetworkManager.Singleton.Shutdown();
            Object.Destroy(NetworkManager.Singleton.gameObject);
        }
    }
}