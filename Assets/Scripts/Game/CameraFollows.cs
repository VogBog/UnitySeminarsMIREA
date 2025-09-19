using UnityEngine;

namespace Game
{
    public class CameraFollows : MonoBehaviour
    {
        private Transform _target;

        [SerializeField] private Vector3 _delta;
        [SerializeField] private float _addYForEverySpeedUnit;
        
        public Vector3 Delta { get; private set; }

        public void SetTarget(Player target)
        {
            _target = target.transform;
            target.Movement.SpeedChanged += OnPlayerSpeedChanged;
            Delta = _delta;
            OnPlayerSpeedChanged(target.Movement.Speed);
        }

        private void LateUpdate()
        {
            if(_target != null)
                transform.position = _target.position + Delta;
        }

        private void OnPlayerSpeedChanged(float speed)
        {
            Delta = _delta + Vector3.up * (speed * _addYForEverySpeedUnit);
        }
    }
}