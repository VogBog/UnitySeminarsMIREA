using Global;
using Unity.Netcode;
using UnityEngine;

namespace Game.LocalMultiplayer
{
    public class LocalMultiplayerStartTimer : NetworkBehaviour
    {
        private GameUI _gameUI;
        
        private void Awake()
        {
            if (StaticParameters.NetworkType is not StaticParameters.NetworkTypes.LocalMultiplayer)
            {
                Destroy(this);
                return;
            }
            
            var players = FindObjectsByType<CarMovement>(FindObjectsSortMode.None);
            foreach(var player in players)
            {
                if(!player.TryGetComponent(out NetworkObject networkObject) ||
                   !networkObject.IsOwner)
                    continue;

                var gameUI = player.GetComponentInChildren<GameUI>(true);
                InitializeGameUI(gameUI);
                break;
            }
        }

        private void InitializeGameUI(GameUI gameUI)
        {
            _gameUI = gameUI;

            if (IsServer)
                gameUI.TimerChanged += OnTimerChanged;
        }

        private void OnTimerChanged(int timer)
        {
            TimerChangedRpc(timer);
        }

        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
        private void TimerChangedRpc(int timer)
        {
            if (IsServer) return;
            _gameUI.StopTimerAndUpdate(timer);
        }
    }
}