using System;
using Damage;
using Data;
using GridMap;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(MoveByTiles))]
    public class PlayerMoveByTiles : MonoBehaviour
    {
        private MoveByTiles _moveByTiles;
        private Player _player;
        private float _tickTime;

        public const float OneTickInSeconds = 1f;

        private Action _tick;
        
        public MoveByTiles MoveByTiles => _moveByTiles;

        public void Initialize(Player player)
        {
            _player = player;
            _moveByTiles = GetComponent<MoveByTiles>();
            _moveByTiles.WentToNewCellType += OnWentNewCellType;
        }

        private void OnWentNewCellType(byte type, Vector2Int coords)
        {
            _tick = null;
            _player.Movement.AffectByIce = false;
            
            if (type is >= GridMapValues.Fire5Seconds and <= GridMapValues.Fire20Seconds)
            {
                _tick += FireTick;
                return;
            }

            if (type is GridMapValues.IceFloor)
            {
                _player.Movement.AffectByIce = true;
                return;
            }

            if (type is >= GridMapValues.ThunderWater5Seconds and <= GridMapValues.ThunderWater30Seconds)
            {
                _tick += ThunderTick;
                return;
            }
        }

        private void FixedUpdate()
        {
            _tickTime += Time.fixedDeltaTime;
            if (_tickTime >= OneTickInSeconds)
            {
                _tickTime = 0f;
                _tick?.Invoke();
            }
        }

        private void FireTick()
        {
            var damageEvent = new GetDamageData(1, _player.gameObject, Elementals.Fire);
            _player.TakeDamage(damageEvent);
        }

        private void ThunderTick()
        {
            _player.Movement.CannotMoveTime += 0.4f;
        }
    }
}