using System;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerController), typeof(SnakeTail))]
    public class Player : MonoBehaviour
    {
        private CamerasSwitcher _cameras;
        
        public PlayerController Controller { get; private set; }
        public SnakeTail SnakeTail { get; private set; }
        public bool IsOwner { get; private set; } = false;
        public bool IsDead { get; private set; } = false;
        public event Action<Player> Died;

        private void Awake()
        {
            Initialize(false);
        }
        
        public void Initialize(bool isOwner)
        {
            _cameras = FindFirstObjectByType<CamerasSwitcher>();
            _cameras.SetActiveCamera(GetComponentInChildren<Camera>());
            
            IsOwner = isOwner;
            
            Controller = GetComponent<PlayerController>();
            Controller.Initialize(isOwner);
            
            SnakeTail = GetComponent<SnakeTail>();
            SnakeTail.Initialize(this, isOwner);
        }

        public void Die()
        {
            if (IsDead) return;

            IsDead = true;
            SnakeTail.DieAll();
            Controller.Stop();
            _cameras.SwitchCamera(this);
            transform.position += Vector3.down * 5f;

            Died?.Invoke(this);
        }
    }
}