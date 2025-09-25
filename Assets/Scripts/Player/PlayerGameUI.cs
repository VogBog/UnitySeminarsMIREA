using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerGameUI : MonoBehaviour
    {
        [SerializeField] private Transform _layoutGroup;

        private Player _player;
        private Image[] _images;

        public void Initialize(Player player)
        {
            _images = _layoutGroup.GetComponentsInChildren<Image>();
            _player = player;
            
            _player.Health.Changed += OnPlayerHealthChanged;
        }

        private void OnPlayerHealthChanged(PlayerHealth health, int value)
        {
            SetHealth(value);
        }

        public void SetHealth(int value)
        {
            value = Mathf.Clamp(value, 0, _images.Length);
            
            for (int i = 0; i < value; i++)
            {
                _images[i].gameObject.SetActive(true);
            }

            for (int i = value; i < _images.Length; i++)
            {
                _images[i].gameObject.SetActive(false);
            }
        }
    }
}