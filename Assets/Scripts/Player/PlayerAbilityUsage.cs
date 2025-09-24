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
        private IPlayerAbility _ability;
        
        private float _usingTime;
        private float _heavyCooldown;
        private float _quickCooldown;
        private float _maxUsingTime;
        private bool _activatingAbility = false;

        public Transform ProjectileOrigin => _projectileOrigin;

        public float QuickCooldownMp => _quickCooldown / _ability.Data.QuickAbilityCooldown;
        public float HeavyCooldownMp => _heavyCooldown / _ability.Data.HeavyAbilityCooldown;
        public float UsingTimeMp => Mathf.Clamp01(_usingTime / _ability.Data.HeavyAbilityTime);
        
        public void Initialize(Player player, ElementalData data)
        {
            _ability = data.AbilityData.CreateAbility(player);
            _player = player;
            _maxUsingTime = _ability.Data.HeavyAbilityTime * 3f;
            
            player.Input.Interacted += StartAbility;
            player.Input.EndInteraction += EndAbility;
            player.Updated += CountCooldown;
        }

        public void ActivateAbility()
        {
            if (!_activatingAbility)
                return;
            _activatingAbility = false;
            
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

            _usingTime = 0f;
        }

        private void StartAbility(PlayerInput input)
        {
            _usingTime = 0f;
            _player.Updated += CountTime;
            _activatingAbility = true;
        }

        private void EndAbility(PlayerInput input)
        {
            _player.Updated -= CountTime;

            ActivateAbility();
        }

        private void CountTime()
        {
            _usingTime = Mathf.Clamp(_usingTime + Time.deltaTime, 0f, _maxUsingTime);
            if (_usingTime >= _maxUsingTime)
            {
                ActivateAbility();
            }
        }

        private void CountCooldown()
        {
            _heavyCooldown = Mathf.Clamp(_heavyCooldown - Time.deltaTime, 0f, 1000f);
            _quickCooldown = Mathf.Clamp(_quickCooldown - Time.deltaTime, 0f, 1000f);
        }
    }
}