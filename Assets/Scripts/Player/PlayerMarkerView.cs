using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    [Serializable]
    public class PlayerMarkerView
    {
        private Player _player;

        private Vector3 _maxScale;
        private Vector3 _maxHeavyScale;
        
        [field: SerializeField] public Transform Marker { get; private set; }
        [field: SerializeField] public Transform HeavyMarker { get; private set; }

        public void Initialize(Player player)
        {
            _player = player;
            _maxScale = Marker.localScale;
            _maxHeavyScale = HeavyMarker.localScale;
            
            player.Updated += Update;
        }

        private void Update()
        {
            Marker.localPosition = _player.Movement.Forward;
            float scale = 1f - _player.AbilityUsage.QuickCooldownMp;
            Marker.localScale = _maxScale * scale;

            scale = 1f - _player.AbilityUsage.HeavyCooldownMp;
            float usingTime = _player.AbilityUsage.UsingTimeMp;
            if(usingTime > 0f)
                scale *= usingTime;
            if (usingTime >= 1f)
            {
                scale += Random.Range(0.05f, 0.15f);
            }
            
            HeavyMarker.localScale = new Vector3(_maxHeavyScale.x * scale, _maxHeavyScale.y, _maxHeavyScale.z * scale);
        }
    }
}