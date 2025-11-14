using System;
using System.Collections;
using UnityEngine;

namespace Game.GameStarter
{
    public interface IGameStarter
    {
        event Action<int> TimerChanged;
        
        bool IsServer();
        IEnumerator InstantiatePlayer(
            Player.Player prefab, Vector3 position, Quaternion rotation, Action<Player.Player> onSpawn);

        void InvokeTimerChanged(int value);
    }
}