using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Cube
{
    [RequireComponent(typeof(Rigidbody))]
    public class CubeMovement : MonoBehaviour
    {
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _speed;
        
        private bool _isFalling;
        private Rigidbody _rb;
        private float _jumpStrength;

        private void OnValidate()
        {
            _rb = GetComponent<Rigidbody>();
            _jumpStrength = _rb.mass * Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
        }

        private void Update()
        {
            if (!_isFalling && Input.GetKeyDown(KeyCode.Space))
            {
                _isFalling = true;
                _rb.AddForce(_jumpStrength * Vector3.up, ForceMode.Impulse);
            }
            
            var x = Input.GetAxis("Horizontal");
            var moveVec = x * _speed * Time.deltaTime * Vector3.right;
            _rb.linearVelocity += moveVec;
        }

        private void FixedUpdate()
        {
            _isFalling = !IsOnGround();
        }

        private bool IsOnGround()
        {
            var ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out var hit, .55f, LayerMask.GetMask("Ground")))
            {
                return true;
            }

            return false;
        }
    }
}
