using System;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerController), typeof(SnakeTail))]
    public class Player : MonoBehaviour
    {
        public PlayerController Controller { get; private set; }
        public SnakeTail SnakeTail { get; private set; }
        public bool IsOwner { get; private set; } = false;
        public bool IsDead { get; private set; } = false;
        public event Action<Player> Died; 
        
        public void Initialize(bool isOwner)
        {
            IsOwner = isOwner;
            
            Controller = GetComponent<PlayerController>();
            Controller.Initialize();
            
            SnakeTail = GetComponent<SnakeTail>();
            SnakeTail.Initialize(this);
        }

        public void Die()
        {
            if (IsDead) return;

            IsDead = true;
            SnakeTail.DieAll();

            transform.position += Vector3.down * 2f;
            Died?.Invoke(this);
        }
    }
}