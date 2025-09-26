using System;
using InputSystems;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class Movement
    {
        [field: SerializeField] public float Speed;
        
        public Vector3 Forward { get; private set; }
        public bool AffectByIce;
        
        private CharacterController _controller;
        private Vector3 _moveVector;
        private Player _player;

        public Transform ControllerTransform => _controller.transform;
        
        public void Initialize(Player player)
        {
            _controller = player.GetComponentInChildren<CharacterController>();
            _player = player;
            
            if(_controller == null)
                throw new NullReferenceException("Movement cannot get CharacterController");
            
            player.FixedUpdated += FixedUpdate;
            player.Input.StartMoving += OnMoveStarted;
            player.Input.EndMoving += OnMoveEnded;
        }

        private void OnMoveStarted(PlayerInput input, Vector2 vector)
        {
            _moveVector = new Vector3(vector.x, 0, vector.y);
            Forward = _moveVector;
            _player.Updated += CheckControls;
        }

        private void OnMoveEnded(PlayerInput input)
        {
            _player.Updated -= CheckControls;
            _moveVector = Vector3.zero;
        }

        private void CheckControls()
        {
            var oldMoveVector = _moveVector;
            var moveAxis = _player.Input.MoveAxis;
            _moveVector = new Vector3(moveAxis.x, 0, moveAxis.y);
            
            if (AffectByIce)
            {
                _moveVector = Vector3.Lerp(oldMoveVector, _moveVector, Time.deltaTime);
            }
            
            Forward = _moveVector;
        }

        private void FixedUpdate()
        {
            _controller.Move(_moveVector * (Speed * Time.fixedDeltaTime));
        }
    }
}