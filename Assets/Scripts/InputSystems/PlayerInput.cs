using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystems
{
    public class PlayerInput : IDisposable
    {
        private readonly IPlayerActions _actions;
        
        public Vector2 MoveAxis => _actions.Move.ReadValue<Vector2>();
        public InputDevice Device;

        public event Action<PlayerInput, Vector2> StartMoving;
        public event Action<PlayerInput> EndMoving, Interacted, EndInteraction;

        public event Action<PlayerInput, InputDevice> SomethingPressed;

        public PlayerInput(int playerIndex)
        {
            _actions = Initialize(PlayerActionsCombiner.CreatePlayer(playerIndex));
        }

        public PlayerInput(IPlayerActions playerActions)
        {
            _actions = Initialize(playerActions);
        }

        private IPlayerActions Initialize(IPlayerActions playerActions)
        {
            playerActions.Move.started += OnMoveStarting;
            playerActions.Move.canceled += OnMoveEnded;
            playerActions.Interact.started += OnInteractionPerformed;
            playerActions.Interact.canceled += OnInteractionEnd;
            playerActions.Enable();

            return playerActions;
        }

        private void OnMoveStarting(InputAction.CallbackContext ctx)
        {
            if (Device != null && Device != ctx.control.device || Time.timeScale == 0f)
                return;
            
            SomethingPressed?.Invoke(this, ctx.control.device);
            StartMoving?.Invoke(this, ctx.ReadValue<Vector2>());
        }

        private void OnMoveEnded(InputAction.CallbackContext ctx)
        {
            if (Device != null && Device != ctx.control.device || Time.timeScale == 0f)
                return;
            
            EndMoving?.Invoke(this);
        }

        private void OnInteractionPerformed(InputAction.CallbackContext ctx)
        {
            if (Device != null && Device != ctx.control.device || Time.timeScale == 0f)
                return;
            
            SomethingPressed?.Invoke(this, ctx.control.device);
            Interacted?.Invoke(this);
        }

        private void OnInteractionEnd(InputAction.CallbackContext ctx)
        {
            if (Device != null && Device != ctx.control.device || Time.timeScale == 0f)
                return;
            
            EndInteraction?.Invoke(this);
        }

        public void Dispose()
        {
            _actions.Move.started -= OnMoveStarting;
            _actions.Move.canceled -= OnMoveEnded;
            _actions.Interact.performed -= OnInteractionPerformed;
            
            StartMoving = null;
            EndMoving = null;
            Interacted = null;
            SomethingPressed = null;
            
            _actions.Disable();
        }
    }
}