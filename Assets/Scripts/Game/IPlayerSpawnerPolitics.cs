using System;
using UnityEngine;

namespace Game
{
    public interface IPlayerSpawnerPolitics
    {
        void SetPrefab(Player prefab);
        void SetSpawnPoints(Vector3[] positions);
        void Instantiate(Action<Player> onSpawn);
        void DestroyPlayer(Player player);
        void LoadScene(int sceneIndex);
    }
}