using System;
using UnityEngine;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        private bool _newDirection = false;
        private Vector3 _oldDirectionPure = Vector3.zero;
        
        public bool IsOwner { get; private set; } = false;
        public Vector3 MoveAxis { get; private set; }

        public event Action<Vector3> NewPoint; 
        
        public void Initialize()
        {
            IsOwner = true;
        }

        private void Update()
        {
            if (!IsOwner)
                return;
            
            var moveAxis = Vector3.zero;
            
            if(Input.GetKeyDown(KeyCode.W))
                moveAxis.z += 1;
            if(Input.GetKeyDown(KeyCode.S))
                moveAxis.z -= 1;
            if(Input.GetKeyDown(KeyCode.A))
                moveAxis.x -= 1;
            if(Input.GetKeyDown(KeyCode.D))
                moveAxis.x += 1;

            if (_oldDirectionPure != moveAxis)
                _newDirection = true;
            _oldDirectionPure = moveAxis;
            
            if(moveAxis.x != 0 || moveAxis.z != 0)
                moveAxis.Normalize();
            
            MoveAxis = moveAxis;
        }

        private void FixedUpdate()
        {
            if(!IsOwner)
                return;
            
            if(_newDirection)
                NewPoint?.Invoke(transform.position);
            _newDirection = false;
            
            transform.position += _speed * Time.fixedDeltaTime * MoveAxis;
        }
    }
}