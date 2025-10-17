using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Account;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Leaderboard
{
    public class LeaderboardPage : MonoBehaviour
    {
        [SerializeField] private VerticalLayoutGroup _group;
        [SerializeField] private LeaderboardRecord _recordPrefab;
        [SerializeField] private TMP_Text _errorText;

        private LeaderboardRecord[] _records;
        private IEnumerator _setDataCoroutine;

        private void Awake()
        {
            for (int i = 0; i < Leaderboard.MaxPlayers; i++)
            {
                var instance = Instantiate(_recordPrefab, _group.transform);
                instance.gameObject.SetActive(false);
            }

            _records = new LeaderboardRecord[Leaderboard.MaxPlayers];
            for (int i = 0; i < Leaderboard.MaxPlayers; i++)
            {
                _records[i] = _group.transform.GetChild(i).GetComponent<LeaderboardRecord>();
            }
        }

        private void OnEnable()
        {
            if (_setDataCoroutine != null)
            {
                _setDataCoroutine.Reset();
                StartCoroutine(_setDataCoroutine);
            }
        }

        public void LoadData(
            Func<List<PlayerAccount>> getAccounts,
            Task<UnityWebRequest.Result> loadAccounts,
            Func<PlayerAccount, float> getScore)
        {
            Task.Run(async () => await LoadDataAsync(getAccounts, loadAccounts, getScore)).ConfigureAwait(true);
        }

        public async Task LoadDataAsync(
            Func<List<PlayerAccount>> getAccounts,
            Task<UnityWebRequest.Result> loadAccounts,
            Func<PlayerAccount, float> getScore)
        {
            _errorText.text = "Loading data...";

            var status = await loadAccounts;
            if (status != UnityWebRequest.Result.Success)
            {
                _errorText.text = status switch
                {
                    UnityWebRequest.Result.ConnectionError => "Connection Error",
                    UnityWebRequest.Result.ProtocolError => "Protocol Error",
                    UnityWebRequest.Result.DataProcessingError => "Data Processing Error",
                    _ => "Error"
                };

                return;
            }

            _errorText.text = "";
            var accounts = getAccounts.Invoke();

            _setDataCoroutine = SetData(accounts, getScore);
            if (gameObject.activeSelf)
            {
                StartCoroutine(_setDataCoroutine);
            }
        }

        public IEnumerator SetData(IEnumerable<PlayerAccount> accounts, Func<PlayerAccount, float> getScore)
        {
            int count = 0;
            _group.enabled = true;
            foreach (var account in accounts)
            {
                SetData(count, account, getScore.Invoke(account).ToString("00.00"));
                count++;

                if (count >= Leaderboard.MaxPlayers)
                    break;

                yield return null;
            }

            yield return null;
            _group.enabled = false;
            _setDataCoroutine = null;
        }

        public void SetData(int index, PlayerAccount account, string score)
        {
            if (index < 0 || index >= Leaderboard.MaxPlayers)
                return;
            
            var record = _records[index];
            record.SetData(account.NickName, index + 1, score);
            record.gameObject.SetActive(true);
        }
    }
}