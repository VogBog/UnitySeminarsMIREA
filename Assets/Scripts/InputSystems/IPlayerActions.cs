using UnityEngine.InputSystem;

namespace InputSystems
{
    public interface IPlayerActions
    {
        public InputAction Move { get; }
        public InputAction Interact { get; }
        public InputActionMap Get();
        public void Enable();
        public void Disable();
        public bool enabled { get; }
    }
}