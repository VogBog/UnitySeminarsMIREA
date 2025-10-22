using Account;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Leaderboard
{
    public class LeaderboardRecord : MonoBehaviour
    {
        [SerializeField] private Image _bg;
        [SerializeField] private TMP_Text _place;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _score;

        public void SetPlayer(int place, PlayerAccount account)
        {
            var clr = _bg.color;
            clr.a = place % 2 == 0 ? 1f : 0.6f;
            _bg.color = clr;
            
            _place.text = place.ToString();
            _name.text = account.Name;
            _score.text = account.MaxScore.ToString("F2");
        }

        public void SetInvisible()
        {
            var clr = _bg.color;
            clr.a = 0f;
            _bg.color = clr;

            _place.text = "";
            _name.text = "";
            _score.text = "";
        }

        public void SetPlayerWithoutPlace(int order, PlayerAccount account)
        {
            SetPlayer(order, account);
            _place.text = "..";
        }
    }
}