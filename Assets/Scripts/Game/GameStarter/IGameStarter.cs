using System;
using System.Collections;
using MultiNetwork;
using UnityEngine;

namespace Game.GameStarter
{
    public interface IGameStarter : INetworkTypeRequirer
    {
        event Action<int> TimerChanged;
        event Action<Player.Player> Initialized; 
        
        bool IsServer();
        IEnumerator InstantiatePlayer(
            Player.Player prefab, Vector3 position, Quaternion rotation);

        bool IsAllPlayersConnected();
        void InvokePlayerConnected();

        void InvokeTimerChanged(int value);
    }
}