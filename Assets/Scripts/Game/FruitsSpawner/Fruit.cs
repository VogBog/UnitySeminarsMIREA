using System;
using UnityEngine;

namespace Game.FruitsSpawner
{
    public class Fruit : MonoBehaviour
    {
        private FruitsSpawner _spawner;
        
        private void Awake()
        {
            _spawner = FindFirstObjectByType<FruitsSpawner>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                _spawner.EatenBy(this, player);
            }
        }
    }
}