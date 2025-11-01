using UnityEngine;

namespace Game
{
    public interface IPlayerSpawnerPolitics
    {
        Player Instantiate(Player prefab, Vector3 pos, Quaternion rot, Transform parent);
    }
}