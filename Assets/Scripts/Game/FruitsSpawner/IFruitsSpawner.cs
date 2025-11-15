using UnityEngine;

namespace Game.FruitsSpawner
{
    public interface IFruitsSpawner
    {
        bool IsServer();
        void SetPrefab(Fruit fruitPrefab);
        void Instantiate(Vector3 position);
        void Despawn(Fruit fruit);
    }
}