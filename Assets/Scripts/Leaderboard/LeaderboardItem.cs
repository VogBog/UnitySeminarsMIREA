using TMPro;
using UnityEngine;

namespace Leaderboard
{
    public class LeaderboardItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _index;
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private TMP_Text _playerTime;

        public void SetData(string playerName, float time)
        {
            _index.text = "..";
            _playerName.text = playerName;
            _playerTime.text = time == 0f ? "" : time.ToString("0.00") + "s";
        }

        public void SetData(int index, string playerName, float time)
        {
            _index.text = index.ToString();
            _playerName.text = playerName;
            _playerTime.text = time == 0f ? "" : time.ToString("0.00") + "s";
        }
    }
}