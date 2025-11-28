using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public interface ICarsSpawner
    {
        event Action<CarMovement> MustInitialize;

        bool IsAllPlayersReady();
        void SendReadyMessageToServer();
        bool CanSpawnCars();
        bool AddCameraAndMovement();
        IEnumerator Instantiate(CarMovement prefab, Vector3 position, Quaternion rotation, Action<CarMovement> onSpawn);
    }
}