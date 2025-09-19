using System;
using UnityEngine;

namespace Game
{
    public class CheckPoint : MonoBehaviour
    {
        public event Action<CarMapRunner, CheckPoint> Collided;

        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.TryGetComponent<CarMapRunner>(out var car))
                Collided?.Invoke(car, this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.TryGetComponent<CarMapRunner>(out var car))
                Collided?.Invoke(car, this);
        }
    }
}