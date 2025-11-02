using System;
using UnityEngine;

namespace Game
{
    public interface IPlayerSpawnerPolitics
    {
        void SetPrefab(Player prefab);
        void Instantiate(Vector3 pos, Quaternion rot, Transform parent, Action<Player> onSpawn);
        void DestroyPlayer(Player player);
        void LoadScene(int sceneIndex);
    }
}