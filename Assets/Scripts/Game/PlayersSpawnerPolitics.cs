using System;
using Global;
using MainMenu;
using Network.LocalMultiplayer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class PlayersSpawnerPolitics : IPlayerSpawnerPolitics
    {
        private Player _prefab;
        
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

        public void SetPrefab(Player prefab)
        {
            _prefab = prefab;
        }
        
        public void Instantiate(Vector3 pos, Quaternion rot, Transform parent, Action<Player> onSpawn)
        {
            var instance = UnityEngine.Object.Instantiate(_prefab, pos, rot, parent);
            onSpawn?.Invoke(instance);
        }

        public void DestroyPlayer(Player player)
        {
            UnityEngine.Object.Destroy(player.gameObject);
        }

        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
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