using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(PlayerController), typeof(Rigidbody))]
    public class CarMovement : MonoBehaviour
    {
        [SerializeField] private float _moveForce;
        [SerializeField] private float _turnSpeed;
        [SerializeField] private float _moveAxisXChangingSpeed;
        [SerializeField] private float _maxForwardSpeed;
        
        private Rigidbody _rb;
        private float _oldMoveAxisX;
        
        public float Velocity { get; private set; }
        
        private void Start()
        {
            var controller = GetComponent<PlayerController>();
            _rb = GetComponent<Rigidbody>();
            
            controller.Move += Move;
        }

        private void Move(Vector2 moveAxis)
        {
            moveAxis.x = Mathf.Lerp(_oldMoveAxisX, moveAxis.x, Time.fixedDeltaTime * _moveAxisXChangingSpeed);
            _oldMoveAxisX = moveAxis.x;
            float forward = Vector3.Dot(transform.forward, _rb.linearVelocity);
            
            Velocity = _rb.linearVelocity.magnitude;
            
            _rb.transform.Rotate(transform.up, _turnSpeed * Time.fixedDeltaTime * forward * moveAxis.x);
            
            if(forward < _maxForwardSpeed)
                _rb.AddForce(_moveForce * moveAxis.y * Time.fixedDeltaTime * _rb.mass * transform.forward, ForceMode.Force);
        }
    }
}