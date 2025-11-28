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
        private bool _die = false;

        private ISnakeTail _tail;
        private readonly List<SnakePoint> _snakePoints = new();

        public const float DangerRadius = 0.7f;

        public List<SnakePoint> SnakePointsCopy => new(_snakePoints);

        private void Awake()
        {
            _tail = GameServices.CreateSnakeTail(this);
            _tail.SetPrefab(_pointPrefab);
            _tail.SetSnake(this);
        }

        public void Initialize(Player player, bool isOwner)
        {
            _player = player;

            if (isOwner)
            {
                _tail.AddNewPoint += OnNewPointAdded;
            }
        }

        public void AddLength()
        {
            if (_die)
                return;
            
            _tail.CalculateDataForAddLength(data =>
            {
                _tail.Instantiate(data.LastPoint, Quaternion.identity, point =>
                {
                    if(!_snakePoints.Contains(point))
                        _snakePoints.Add(point);
                });
            });
        }

        public SnakeTailCalculationData CalculateDataForAddLength()
        {
            var lastPoint = transform.position - _player.Controller.MoveAxis;
            if (_lastSnakePoint != null)
            {
                lastPoint = _lastSnakePoint.transform.position - _lastSnakePoint.transform.forward;
            }

            return new(lastPoint);
        }

        public bool IsInDanger(Vector3 head, bool calculateHead)
        {
            if (_snakePoints.Count == 0)
                return Vector3.Distance(head, transform.position) <= DangerRadius && calculateHead;

            if (calculateHead && Vector3.Distance(head, transform.position) <= DangerRadius)
                return true;

            bool firstPoint = true;
            int index = -1;
            foreach (var point in _snakePoints)
            {
                index++;
                if (!calculateHead && firstPoint)
                {
                    firstPoint = false;
                    continue;
                }

                if (Vector3.Distance(head, point.transform.position) <= DangerRadius)
                {
                    return true;
                }
                    
            }

            return false;
        }

        public void DieAll()
        {
            _die = true;
            _tail.DespawnAllPoints();
        }

        private void OnNewPointAdded(SnakePoint point)
        {
            var data = CalculateDataForAddLength();
            point.transform.position = data.LastPoint;
            
            point.Initialize(_player);
            point.SetNextPoint(_lastSnakePoint?.transform ?? transform);
            
            _lastSnakePoint = point;
            
            if(!_snakePoints.Contains(point))
                _snakePoints.Add(point);
        }
    }
}