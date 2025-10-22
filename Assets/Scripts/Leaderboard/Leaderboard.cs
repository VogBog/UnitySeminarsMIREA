using System;
using System.Collections;
using System.Linq;
using Global;
using Leaderboard.FirebaseDesktopHelper;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Leaderboard
{
    public class Leaderboard : MonoBehaviour
    {
        [SerializeField] private Config _config;
        [SerializeField] private PlayerNameInput _playerNameInput;
        [SerializeField] private Transform _dataParent;
        [SerializeField] private LeaderboardItem _itemPrefab;

        private bool _ended = false;

        private void Awake()
        {
            FirebaseRestRequests.SetData(_config.FirebaseUrl, _config.FirebaseApiKey);
            _playerNameInput.Initialize();
        }

        public void LoadNextScene()
        {
            SceneManager.LoadScene(0);
        }

        private void Start()
        {
            StartCoroutine(StartRoutine());
        }

        private IEnumerator StartRoutine()
        {
            var allPlayersNameInput = new AllPlayersNameInput();
            yield return allPlayersNameInput.WaitForAllInputRoutine(_playerNameInput, StaticParameters.FinishData);
            StaticParameters.FinishData = null;

            StartCoroutine(RetryRoutine(allPlayersNameInput));
        }

        private IEnumerator RetryRoutine(AllPlayersNameInput allPlayersNameInput)
        {
            StartCoroutine(MainRoutine(allPlayersNameInput));

            yield return new WaitForSeconds(10f);
            
            if(!_ended)
                StartCoroutine(RetryRoutine(allPlayersNameInput));
        }

        private IEnumerator MainRoutine(AllPlayersNameInput allPlayersNameInput)
        {
            yield return SavePlayersDataRoutine(allPlayersNameInput);

            var playerNames = allPlayersNameInput.Data ?? Array.Empty<AllPlayersNameInput.PlayerData>();

            yield return ShowPlayersDataRoutine(playerNames);
            
            _ended = true;
        }

        private IEnumerator ShowPlayersDataRoutine(AllPlayersNameInput.PlayerData[] playerNames)
        {
            var leaderboardRef = new LeaderboardService.ListRef();
            yield return LeaderboardService.GetTotalLeaderboard(
                playerNames.Select(x => x.Name),
                list => leaderboardRef.List = list);
            var leaderboard = leaderboardRef.List;

            if (leaderboard == null || leaderboard.Count == 0)
                yield break;

            int index = 1;
            foreach (var playerData in leaderboard)
            {
                var instance = Instantiate(_itemPrefab, _dataParent);
                if (playerData.Time == 0f)
                    index = -1;

                if (index == -1)
                {
                    instance.SetData(playerData.Name, playerData.Time);
                }
                else
                {
                    instance.SetData(index, playerData.Name, playerData.Time);
                    index++;
                }

                yield return null;
            }
        }

        private IEnumerator SavePlayersDataRoutine(AllPlayersNameInput allPlayersNameInput)
        {
            var data = allPlayersNameInput.Data;

            if (data == null)
                yield break;
            
            foreach (var i in data)
            {
                yield return LeaderboardService.SaveRoutine(i.Name, i.Time);
            }
        }
    }
}