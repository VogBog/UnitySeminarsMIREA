using Unity.Netcode;
using UnityEngine;

namespace MainGame.GameTimers
{
    public class LocalMultiplayerGameTimer : NetworkBehaviour, IGameTimer
    {
        private DefaultGameTimer _timer;

        public int Seconds { get; private set; }
        
        public void StartTimer(MonoBehaviour monoBeh)
        {
            if (!NetworkManager.IsHost)
                return;
            
            _timer = new DefaultGameTimer();
            _timer.SecondsChanged += OnSecondsChanged;
            _timer.StartTimer(monoBeh);
        }

        public void StopTimer(MonoBehaviour monoBeh)
        {
            if (!NetworkManager.IsHost)
                return;
            
            _timer.StopTimer(monoBeh);
            _timer.SecondsChanged -= OnSecondsChanged;
        }

        private void OnSecondsChanged(int seconds)
        {
            Seconds = seconds;
            SetSecondsClientRpc(seconds);
        }

        [ClientRpc]
        private void SetSecondsClientRpc(int seconds)
        {
            if (NetworkManager.IsHost)
                return;
            
            Seconds = seconds;
        }
    }
}