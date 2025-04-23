using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Cube
{
    [RequireComponent(typeof(Rigidbody), typeof(Animator))]
    public class CubeMovement : MonoBehaviour
    {
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _speed;
        
        private bool _isFalling;
        private Rigidbody _rb;
        private Animator _animator;
        private float _jumpStrength;

        private void OnValidate()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            
            _jumpStrength = _rb.mass * Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
        }

        private void Update()
        {
            if (!_isFalling && Input.GetKeyDown(KeyCode.Space))
            {
                _isFalling = true;
                _animator.SetTrigger("Jump");
                _rb.AddForce(_jumpStrength * Vector3.up, ForceMode.Impulse);
            }
        }

        private void FixedUpdate()
        {
            var oldFalling = _isFalling;
            _isFalling = !IsOnGround();
            if (oldFalling && !_isFalling)
            {
                _animator.SetTrigger("Ground");
            }
            
            var x = Input.GetAxis("Horizontal");
            var moveVec = x * _speed * Time.deltaTime * Vector3.right;
            _rb.MovePosition(_rb.position + moveVec);
            _animator.SetBool("Walking", x != 0);
        }

        private bool IsOnGround()
        {
            var ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out var hit, .6f, LayerMask.GetMask("Ground")))
            {
                return true;
            }

            return false;
        }
    }
}
