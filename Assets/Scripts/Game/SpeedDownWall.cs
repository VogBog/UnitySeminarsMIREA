using UnityEngine;

namespace Game
{
    public class SpeedDownWall : MonoBehaviour, IInteractable
    {
        public const float SlowDown = 1f;
        
        public void Interact(Player player)
        {
            player.Movement.AddSpeed(-SlowDown);
        }
    }
}