using System;
using System.Collections;
using UnityEngine;

namespace MainGame.GameStarters
{
    public interface IGameStarter
    {
        IEnumerator CreatePlayerRoutine(
            GameStarter gameStarter, Vector3 spawnPoint, Action<Player.Player> playerCreated);
    }
}