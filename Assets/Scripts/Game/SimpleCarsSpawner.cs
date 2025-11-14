using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class SimpleCarsSpawner : ICarsSpawner
    {
        public event Action<CarMovement> MustInitialize;
        
        public bool CanSpawnCars() => true;

        public bool AddCameraAndMovement() => true;

        public IEnumerator Instantiate(CarMovement prefab, Vector3 position, Quaternion rotation, Action<CarMovement> onSpawn)
        {
            var instance = UnityEngine.Object.Instantiate(prefab, position, rotation);
            onSpawn?.Invoke(instance);
            MustInitialize?.Invoke(instance);

            yield return null;
        }
    }
}