using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class Player : MonoBehaviour
    {
        public PlayerController Controller { get; private set; }
        public bool IsOwner { get; private set; } = false;
        
        public void Initialize(bool isOwner)
        {
            IsOwner = isOwner;
            
            Controller = GetComponent<PlayerController>();
            Controller.Initialize();
        }
    }
}