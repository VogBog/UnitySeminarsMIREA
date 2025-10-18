using System.Collections;
using System.Linq;
using Data;
using Leaderboard;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AfterGame
{
    public class AfterGame : MonoBehaviour
    {
        [SerializeField] private VerticalLayoutGroup _scoreParent;
        [SerializeField] private ScoreRecord _recordPrefab;
        [SerializeField] private LeaderboardTable _leaderboardTable;
        [SerializeField] private TMP_Text _totalScore;

        private IEnumerator Start()
        {
            var scoreData = StaticParameters.PlayerScore;
            StaticParameters.PlayerScore = null;
            bool isWhite = true;
            
            foreach (var score in scoreData)
            {
                var instance = Instantiate(_recordPrefab, _scoreParent.transform);
                instance.SetData(score.Item1, score.Item2, isWhite);
                
                isWhite = !isWhite;

                yield return new WaitForSeconds(0.5f);
            }
            
            _scoreParent.enabled = false;

            float totalScore = scoreData.Sum(x => x.Item2);
            _totalScore.text = $"Total: {totalScore:00.0}";
            
            yield return new WaitForSeconds(0.5f);

            _leaderboardTable.enabled = true;
        }

        public void Quit()
        {
            SceneManager.LoadScene(1);
        }
    }
}