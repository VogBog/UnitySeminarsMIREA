using Global;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Leaderboard
{
    public class LeaderboardProfile : MonoBehaviour
    {
        [SerializeField] private TMP_Text _username;
        [SerializeField] private TMP_Text _score;
        [SerializeField] private Button _quitBtn;

        private void Awake()
        {
            _quitBtn.onClick.AddListener(OnQuitBtnClicked);

            var acc = StaticParameters.Account;
            _username.text = acc.Name;
            _score.text = acc.MaxScore.ToString("F2");
        }

        private void OnQuitBtnClicked()
        {
            SceneManager.LoadScene(0);
        }
    }
}