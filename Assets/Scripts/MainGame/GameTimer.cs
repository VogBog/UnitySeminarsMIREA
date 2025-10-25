using MainGame.GameTimers;
using UnityEngine;

namespace MainGame
{
    public class GameTimer : MonoBehaviour
    {
        private IGameTimer _timer;

        public int Seconds => _timer.Seconds;

        public GameTimer Initialize(IGameTimer timer)
        {
            _timer = timer;
            return this;
        }

        public void StartTimer() => _timer.StartTimer(this);

        public void StopTimer() => _timer.StopTimer(this);
    }
}