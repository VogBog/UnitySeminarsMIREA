using System;
using System.Collections;
using Lobby;
using UnityEngine;

namespace MainGame.GameStarters
{
    public class SplitScreenGameStarter : IGameStarter
    {
        public IEnumerator CreatePlayerRoutine(
            GameStarter gameStarter, Vector3 spawnPoint, Action<Player.Player> playerCreated)
        {
            var prefab = gameStarter.PlayerPrefab;
            var instance = UnityEngine.Object.Instantiate(prefab, gameStarter.transform);
            instance.transform.position = spawnPoint;

            yield return null;
            
            playerCreated?.Invoke(instance);
        }

        public void InitializePlayer(Player.Player player, PlayerData data)
        {
            player.Initialize(data);
        }
    }
}