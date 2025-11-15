using System.Collections;
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
            StartCoroutine(UpdateDirectionRoutine());
        }

        public void SetNextPoint(Transform point)
        {
            _nextPoint = point;
            
            transform.LookAt(point.position);
        }

        private IEnumerator UpdateDirectionRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.25f);
                
                if(Vector3.Angle(transform.forward, GetNextPointDirection()) < 10)
                    transform.LookAt(_nextPoint.position);
            }
        }

        private Vector3 GetNextPointDirection()
        {
            if (_controller.transform == _nextPoint)
                return _controller.MoveAxis;
            return _nextPoint.forward;
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