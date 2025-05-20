using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    private InputAction _moveAction;

    private Chat _chat;
    private bool _canMove = true;

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

    private void Start()
    {
        _chat = FindFirstObjectByType<Chat>();
        _chat?.SetOwnerName($"Guest{Random.Range(1000, 10000)}");
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _chat?.SetUIActive(false);
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _canMove = !_chat.ChangeUIActive();
        }

        if (_canMove)
        {
            Move();   
        }
    }

    private void Move()
    {
        Vector2 inputVector = _moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        transform.position += _moveSpeed * Time.deltaTime * moveDirection;
    }
}
