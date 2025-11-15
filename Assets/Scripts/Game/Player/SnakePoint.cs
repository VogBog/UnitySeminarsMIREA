using UnityEngine;

namespace Game.Player
{
    public class SnakePoint : MonoBehaviour
    {
        private PlayerController _controller;
        private Transform _nextPoint;

        public void Initialize(Player player)
        {
            _controller = player.Controller;
        }

        public void SetNextPoint(Transform point)
        {
            _nextPoint = point;
            
            transform.LookAt(point.position);
        }

        private void FixedUpdate()
        {
            if (_nextPoint == null)
                return;

            if (Vector3.Distance(transform.position, _nextPoint.position) > 1f)
            {
                transform.LookAt(_nextPoint.position);
            }
            
            transform.position += _controller.Speed * Time.fixedDeltaTime * transform.forward;
        }
    }
}