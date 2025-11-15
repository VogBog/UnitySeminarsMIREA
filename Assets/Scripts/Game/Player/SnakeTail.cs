using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;

namespace Game.Player
{
    public class SnakeTail : MonoBehaviour
    {
        [SerializeField] private SnakePoint _pointPrefab;

        private SnakePoint _lastSnakePoint;
        private Player _player;

        private ISnakeTail _tail;
        private readonly List<SnakePoint> _snakePoints = new();

        private void Awake()
        {
            _tail = GameServices.CreateSnakeTail(this);
            _tail.SetPrefab(_pointPrefab);
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(6f);
            AddLength();
            yield return new WaitForSeconds(1f);
            AddLength();
            yield return new WaitForSeconds(1f);
            AddLength();
        }

        public void Initialize(Player player)
        {
            _player = player;
            _tail.AddNewPoint += OnNewPointAdded;
        }

        public void AddLength()
        {
            var lastPoint = transform.position - _player.Controller.MoveAxis;
            if (_lastSnakePoint != null)
            {
                lastPoint = _lastSnakePoint.transform.position - _lastSnakePoint.transform.forward;
            }
            
            _tail.Instantiate(lastPoint, Quaternion.identity);
        }

        private void OnNewPointAdded(SnakePoint point)
        {
            point.Initialize(_player);
            point.SetNextPoint(_lastSnakePoint?.transform ?? transform);
            _lastSnakePoint = point;
            _snakePoints.Add(point);
        }
    }
}