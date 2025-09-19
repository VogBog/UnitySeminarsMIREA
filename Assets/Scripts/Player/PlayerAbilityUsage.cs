using System;
using Data;
using InputSystems;
using Player.Abilities;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerAbilityUsage
    {
        [SerializeField] private Transform _projectileOrigin;
        
        private Player _player;
        private float _usingTime;
        private float _heavyCooldown;
        private float _quickCooldown;
        private IPlayerAbility _ability;

        public Transform ProjectileOrigin => _projectileOrigin;
        
        public void Initialize(Player player, ElementalData data)
        {
            _ability = data.AbilityData.CreateAbility(player);
            _player = player;
            player.Input.Interacted += StartAbility;
            player.Input.EndInteraction += EndAbility;
            player.Updated += CountCooldown;
        }

        private void StartAbility(PlayerInput input)
        {
            _usingTime = 0f;
            _player.Updated += CountTime;
        }

        private void EndAbility(PlayerInput input)
        {
            _player.Updated -= CountTime;

            if (_usingTime >= _ability.Data.HeavyAbilityTime && _heavyCooldown <= 0f)
            {
                _ability.HeavyAbility();
                _heavyCooldown = _ability.Data.HeavyAbilityCooldown;
            }
            else if(_quickCooldown <= 0f)
            {
                _ability.QuickAbility();
                _quickCooldown = _ability.Data.QuickAbilityCooldown;
            }
        }

        private void CountTime()
        {
            _usingTime = Mathf.Clamp(_usingTime + Time.deltaTime, 0f, _ability.Data.HeavyAbilityTime);
        }

        private void CountCooldown()
        {
            _heavyCooldown = Mathf.Clamp(_heavyCooldown - Time.deltaTime, 0f, 1000f);
            _quickCooldown = Mathf.Clamp(_quickCooldown - Time.deltaTime, 0f, 1000f);
        }
    }
}