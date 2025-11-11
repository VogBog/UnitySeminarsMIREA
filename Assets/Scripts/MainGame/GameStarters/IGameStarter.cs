using System;
using System.Collections;
using Lobby;
using UnityEngine;

namespace MainGame.GameStarters
{
    public interface IGameStarter
    {
        IEnumerator CreatePlayerRoutine(
            GameStarter gameStarter,
            Vector3 spawnPoint,
            Action<Player.Player> playerCreated);

        void InitializePlayer(Player.Player player, PlayerData data);
    }
}