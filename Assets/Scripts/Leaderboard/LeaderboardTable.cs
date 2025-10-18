using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Leaderboard
{
    public class LeaderboardTable : MonoBehaviour
    {
        [SerializeField] private Transform _pagesParent;
        [SerializeField] private Button[] _buttons;
        [SerializeField] private LeaderboardPage _pagePrefab;

        private LeaderboardPage[] _pages;
        private readonly Leaderboard _leaderboard = new();
        private GameObject _leaderboardLoadingGameObject;

        private void Start()
        {
            _leaderboardLoadingGameObject = new GameObject("Leaderboard loading");
            _leaderboardLoadingGameObject.transform.SetParent(null);
            _leaderboardLoadingGameObject.gameObject.SetActive(true);
            var comp = _leaderboardLoadingGameObject.AddComponent<LeaderboardLoadingHelper>();
            comp.StartCoroutine(StartRoutine());
        }

        private IEnumerator StartRoutine()
        {
            _pages = new LeaderboardPage[3];
            for (int i = 0; i < _pages.Length; i++)
            {
                _pages[i] = Instantiate(_pagePrefab, _pagesParent);
                yield return null;
            }

            yield return null;
            yield return null;
            
            OpenPage(0);

            yield return null;
            
            _pages[0].LoadData(
                () => _leaderboard.Leaderboard4,
                _leaderboard.Load4(),
                acc => acc.Max4Score);
            
            _pages[1].LoadData(
                () => _leaderboard.Leaderboard2Vs2,
                _leaderboard.Load2Vs2(),
                acc => acc.Max2V2Score);
            
            _pages[2].LoadData(
                () => _leaderboard.Leaderboard1Vs1,
                _leaderboard.Load1Vs1(),
                acc => acc.Max1V1Score);

            for (int i = 0; i < _buttons.Length; i++)
            {
                int index = i;
                _buttons[index].onClick.AddListener(() => OpenPage(index));
            }

            yield return null;
            
            Destroy(_leaderboardLoadingGameObject);
            _leaderboardLoadingGameObject = null;
        }

        public void OpenPage(int index)
        {
            foreach (var page in _pages)
            {
                page.gameObject.SetActive(false);
            }
            
            _pages[index].gameObject.SetActive(true);
        }
    }
}