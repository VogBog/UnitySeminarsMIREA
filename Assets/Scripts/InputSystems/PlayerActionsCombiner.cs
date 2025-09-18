using System;
using UnityEngine.InputSystem;

namespace InputSystems
{
    public class PlayerActionsCombiner : IPlayerActions
    {
        private static InputSystem _inputSystem;
        private readonly InputActionMap _actionMap;
        
        public PlayerActionsCombiner(InputAction move, InputAction interact, InputActionMap get)
        {
            Move = move;
            Interact = interact;
            _actionMap = get;
        }

        public static InputSystem InputSystem => _inputSystem ??= new InputSystem();
        
        public InputAction Move { get; private set; }
        public InputAction Interact { get; private set; }

        public InputActionMap Get() => _actionMap;

        public void Enable() => _actionMap.Enable();

        public void Disable() => _actionMap.Disable();

        public bool enabled => _actionMap.enabled;

        public static IPlayerActions CreatePlayer(InputSystem.Player1Actions actions)
        => new PlayerActionsCombiner(
            actions.Move, actions.Interact, actions.Get());
        
        public static IPlayerActions CreatePlayer(InputSystem.Player2Actions actions)
            => new PlayerActionsCombiner(
                actions.Move, actions.Interact, actions.Get());
        
        public static IPlayerActions CreatePlayer(InputSystem.Player3Actions actions)
            => new PlayerActionsCombiner(
                actions.Move, actions.Interact, actions.Get());
        
        public static IPlayerActions CreatePlayer(InputSystem.Player4Actions actions)
            => new PlayerActionsCombiner(
                actions.Move, actions.Interact, actions.Get());

        public static IPlayerActions CreatePlayer(int playerIndexFrom1)
        {
            return playerIndexFrom1 switch
            {
                1 => CreatePlayer(new InputSystem.Player1Actions(InputSystem)),
                2 => CreatePlayer(new InputSystem.Player2Actions(InputSystem)),
                3 => CreatePlayer(new InputSystem.Player3Actions(InputSystem)),
                4 => CreatePlayer(new InputSystem.Player4Actions(InputSystem)),
                _ => throw new ArgumentException("playerIndex must be in range from 1 to 4")
            };
        }
    }
}