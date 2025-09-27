using DG.Tweening;
using GridMap;
using MainGame;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerGameUI : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _layoutGroup;
        [SerializeField] private Image _smokeImage;
        [SerializeField] private GameObject _gameOver;
        [SerializeField] private GameObject _win;

        private Player _player;
        private Image[] _images;

        public void Initialize(Player player)
        {
            _images = _layoutGroup.GetComponentsInChildren<Image>();
            _player = player;
            
            _player.Health.Changed += OnPlayerHealthChanged;
            _player.MoveByTiles.MoveByTiles.WentToNewCellType += OnWentToNewCellType;
            _player.Health.Died += _ => ShowGameOver();
            
            var clr = _smokeImage.color;
            clr.a = 0f;
            _smokeImage.color = clr;
            
            _win.SetActive(false);
            _gameOver.SetActive(false);

            FindFirstObjectByType<GameFinisher>().PlayerWinned += OnPlayerWinned;
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

        private void OnPlayerWinned(Player player)
        {
            if(_player == player)
                ShowWin();
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

        public void ShowGameOver()
        {
            _canvas.transform.SetParent(null);
            _gameOver.SetActive(true);
        }

        public void ShowWin()
        {
            _win.SetActive(true);
        }
    }
}