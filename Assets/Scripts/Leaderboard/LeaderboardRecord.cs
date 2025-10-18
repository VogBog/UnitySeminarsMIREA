using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Leaderboard
{
    public class LeaderboardRecord : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private Image _placeGlow;
        [SerializeField] private TMP_Text _placeText;
        [SerializeField] private TMP_Text _score;

        [SerializeField] private Color _firstBgColor;
        [SerializeField] private Color _secondBgColor;

        public void SetData(string playerName, int place, string score)
        {
            _playerName.text = playerName;
            _placeText.text = place.ToString();
            _score.text = score;

            _placeGlow.color = place switch
            {
                1 => Color.yellow,
                2 => Color.white,
                3 => new Color(0.72f, 0.45f, 0.2f),
                _ => new Color(0, 0, 0, 0)
            };

            _placeText.color = place switch
            {
                1 => new Color(0, 0, 0.55f),
                2 => Color.black,
                3 => Color.white,
                _ => Color.white
            };
            
            _background.color = place % 2 == 0 ?
                _firstBgColor : _secondBgColor;
        }

        public void SetPlaceText(string place)
        {
            _placeText.text = place;
        }

        public void SetEmpty()
        {
            _background.color = new Color(0, 0, 0, 0);
            _playerName.text = "";
            _placeText.text = "";
            _placeGlow.color = new Color(0, 0, 0, 0);
            _score.text = "";
        }

        public void SetDots()
        {
            _playerName.text = "...";
            _placeText.text = ".";
            _placeGlow.color = new Color(0, 0, 0, 0);
            _background.color = new Color(0, 0, 0, 0);
            _score.text = "..";
        }
    }
}