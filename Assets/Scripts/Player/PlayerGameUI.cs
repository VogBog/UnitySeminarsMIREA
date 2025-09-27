using DG.Tweening;
using GridMap;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerGameUI : MonoBehaviour
    {
        [SerializeField] private Transform _layoutGroup;
        [SerializeField] private Image _smokeImage;

        private Player _player;
        private Image[] _images;

        public void Initialize(Player player)
        {
            _images = _layoutGroup.GetComponentsInChildren<Image>();
            _player = player;
            
            _player.Health.Changed += OnPlayerHealthChanged;
            _player.MoveByTiles.MoveByTiles.WentToNewCellType += OnWentToNewCellType;
            
            var clr = _smokeImage.color;
            clr.a = 0f;
            _smokeImage.color = clr;
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

        private void OnWentToNewCellType(byte type, Vector2Int position)
        {
            if(type is GridMapValues.Steam)
                GoIntoSmoke();
            else
                GoFromSmoke();
        }

        public void GoIntoSmoke()
        {
            var clr = _smokeImage.color;
            clr.a = 1f;
            _smokeImage.DOColor(clr, 1f);
        }

        public void GoFromSmoke()
        {
            var clr = _smokeImage.color;
            clr.a = 0f;
            _smokeImage.DOColor(clr, 1f);
        }
    }
}