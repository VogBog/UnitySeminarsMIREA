using System;
using Global;
using MainMenu;
using Network.LocalMultiplayer;
using Network.PhotonMultiplayer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class PlayersSpawnerPolitics : IPlayerSpawnerPolitics
    {
        private Player _prefab;
        private Vector3[] _spawnPoints;
        private int _spawnPointIndex;
        
        public IPlayerSpawnerPolitics GetPolitics(PlayersSpawner spawner)
        {
            return StaticParameters.GameType switch
            {
                GameTypes.Single => this,
                GameTypes.SplitScreen => this,
                GameTypes.LocalMultiplayer => GetLocalMultiplayerPolitics(spawner),
                GameTypes.Photon => GetPhotonMultiplayerPolitics(spawner),
                _ => throw new NotImplementedException()
            };
        }

        public void SetPrefab(Player prefab)
        {
            _prefab = prefab;
        }

        public void SetSpawnPoints(Vector3[] positions)
        {
            _spawnPoints = positions;
        }
        
        public void Instantiate(Action<Player> onSpawn)
        {
            var pos = _spawnPoints[_spawnPointIndex];
            var rot = Quaternion.identity;
            _spawnPointIndex = (_spawnPointIndex + 1) % _spawnPoints.Length;
            
            var instance = UnityEngine.Object.Instantiate(_prefab, pos, rot);
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

        private IPlayerSpawnerPolitics GetPhotonMultiplayerPolitics(PlayersSpawner spawner)
        {
            var playersSpawner = spawner.GetComponentInChildren<PhotonMultiplayerPlayersSpawner>();
            if(playersSpawner == null)
                throw new NullReferenceException("PlayersSpawner must have PhotonMultiplayerPlayersSpawner");
            
            return playersSpawner;
        }
    }
}