using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AfterGame
{
    public class ScoreRecord : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Image _background;
        [SerializeField] private float _animationTime;

        public void SetData(string name, float score, bool isWhite)
        {
            var backgroundColor = isWhite ?
                new Color(1f, 1f, 1f, 0.02f) :
                new Color(0f, 0f, 0f, 0f);
            var textColor = _nameText.color;
            
            _nameText.text = name;
            _scoreText.text = score.ToString("00.0");

            _nameText.color = new Color(0f, 0f, 0f, 0f);
            _scoreText.color = new Color(0f, 0f, 0f, 0f);
            _background.color = new Color(0f, 0f, 0f, 0f);
            
            _nameText.DOColor(textColor, _animationTime);
            _scoreText.DOColor(textColor, _animationTime);
            _background.DOColor(backgroundColor, _animationTime);
        }
    }
}