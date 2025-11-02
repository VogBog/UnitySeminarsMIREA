using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _initSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _sphereRadius;

        [SerializeField] private GameObject _marker;

        private Player _player;

        private float _speed;
        private bool _stopped = true;

        public event Action<float> SpeedChanged;
        public event Action<bool> StoppedChanged; 

        public float Speed => _speed;
        public bool IsStopped => _stopped;
        public PlayerController Controller { get; private set; }
        public GameObject Marker => _marker;

        public void Initialize(Player player)
        {
            _player = player;
            
            Controller = GetComponent<PlayerController>();
            Controller.Pressing += OnPressingBtn;
            Controller.UnPressed += OnUnPressedBtn;
            
            _speed = _initSpeed;
        }

        public void AddSpeed(float add)
        {
            _speed = Mathf.Clamp(_speed + add, _initSpeed, 100f);
            SpeedChanged?.Invoke(_speed);
        }

        private void OnPressingBtn()
        {
            _marker.SetActive(true);
            _stopped = true;
            transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
            StoppedChanged?.Invoke(_stopped);
        }

        private void OnUnPressedBtn()
        {
            _marker.SetActive(false);
            _stopped = false;
            StoppedChanged?.Invoke(_stopped);
        }

        private void FixedUpdate()
        {
            if (_stopped)
                return;
            
            var ray = new Ray(transform.position, transform.forward);
            if (!Physics.Raycast(ray, out var hit, _speed * Time.fixedDeltaTime + _sphereRadius))
            {
                transform.position += _speed * Time.fixedDeltaTime * transform.forward;
            }
            else
            {
                var newDirection = Vector3.Reflect(transform.forward, hit.normal);
                transform.position = hit.point - transform.forward * _sphereRadius;
                transform.LookAt(transform.position + newDirection);

                if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    EventBus.EventBus.Interact(_player, interactable);
                }
            }
        }
    }
}