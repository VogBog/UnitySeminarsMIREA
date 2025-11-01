using System;
using Global;
using MainMenu;
using Network.LocalMultiplayer;
using UnityEngine;

namespace Game
{
    public class PlayersSpawnerPolitics : IPlayerSpawnerPolitics
    {
        public IPlayerSpawnerPolitics GetPolitics(PlayersSpawner spawner)
        {
            return StaticParameters.GameType switch
            {
                GameTypes.Single => this,
                GameTypes.SplitScreen => this,
                GameTypes.LocalMultiplayer => GetLocalMultiplayerPolitics(spawner),
                _ => throw new NotImplementedException()
            };
        }
        
        public Player Instantiate(Player prefab, Vector3 pos, Quaternion rot, Transform parent)
        {
            return UnityEngine.Object.Instantiate(prefab, pos, rot, parent);
        }

        private IPlayerSpawnerPolitics GetLocalMultiplayerPolitics(PlayersSpawner spawner)
        {
            var playersSpawner = spawner.GetComponentInChildren<LocalMultiplayerPlayersSpawner>();
            if(playersSpawner == null)
                throw new NullReferenceException("PlayersSpawner must have LocalMultiplayerPlayersSpawner");

            return playersSpawner;
        }
    }
}