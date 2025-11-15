using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerController), typeof(SnakeTail))]
    public class Player : MonoBehaviour
    {
        public PlayerController Controller { get; private set; }
        public SnakeTail SnakeTail { get; private set; }
        public bool IsOwner { get; private set; } = false;
        
        public void Initialize(bool isOwner)
        {
            IsOwner = isOwner;
            
            Controller = GetComponent<PlayerController>();
            Controller.Initialize();
            
            SnakeTail = GetComponent<SnakeTail>();
            SnakeTail.Initialize(this);
        }
    }
}