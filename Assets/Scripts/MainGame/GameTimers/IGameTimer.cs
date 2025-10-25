using UnityEngine;

namespace MainGame.GameTimers
{
    public interface IGameTimer
    {
        int Seconds { get; }

        void StartTimer(MonoBehaviour monoBeh);
        void StopTimer(MonoBehaviour monoBeh);
    }
}