using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    private InputAction _moveAction;

    private void OnEnable()
    {
        _moveAction = new InputAction("Move");
        _moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        _moveAction.Enable();
    }
    
    private void OnDisable()
    {
        _moveAction.Disable();
    }

    private void Update()
    {
        if (!IsOwner) return;
        Move();
    }

    private void Move()
    {
        Vector2 inputVector = _moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        transform.position += _moveSpeed * Time.deltaTime * moveDirection;
    }
}
