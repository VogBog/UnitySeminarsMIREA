using System.Collections.Generic;
using Account;
using Global;
using UnityEngine;
using UnityEngine.Networking;

namespace Leaderboard
{
    public class LeaderboardTable : MonoBehaviour
    {
        [SerializeField] private Transform _recordsParent;
        
        private LeaderboardRecord[] _records;

        private void Start()
        {
            _records = _recordsParent.GetComponentsInChildren<LeaderboardRecord>();

            if (StaticParameters.PlayerScore > 0)
            {
                SetScore(StaticParameters.PlayerScore);
                StaticParameters.PlayerScore = 0f;
            }
            else
            {
                SetData();
            }
        }

        private void SetScore(float score)
        {
            var acc = StaticParameters.Account;
            if (string.IsNullOrEmpty(acc.Id))
            {
                SetData();
                return;
            }

            acc.MaxScore = score;
            StaticParameters.Account = acc;
            
            AccountRepository.SetAccount(acc, res =>
            {
                if (res != UnityWebRequest.Result.ConnectionError)
                {
                    SetData();
                    return;
                }
                AccountRepository.SetAccount(acc, res2 =>
                {
                    if (res2 != UnityWebRequest.Result.ConnectionError)
                    {
                        SetData();
                        return;
                    }
                    AccountRepository.SetAccount(acc, res3 =>
                    {
                        if (res3 != UnityWebRequest.Result.ConnectionError)
                        {
                            SetData();
                            return;
                        }
                        AccountRepository.SetAccount(acc, _ =>
                        {
                            SetData();
                        });
                    });
                });
            });
        }

        private void SetData()
        {
            LeaderboardRepository.GetLeaderboardSeveralTries(SetLeaderboardTop);
            LeaderboardRepository.GetNeighboursSeveralTries(StaticParameters.Account, SetLeaderboardBottom);
        }

        private void SetLeaderboardTop(List<PlayerAccount> accounts)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                var record = _records[i];
                var acc = accounts[i];
                
                record.SetPlayer(i + 1, acc);
            }

            for (int i = accounts.Count; i < LeaderboardRepository.LeaderboardRecordsCount + 1; i++)
            {
                _records[i].SetInvisible();
            }
        }

        private void SetLeaderboardBottom(List<PlayerAccount> accounts)
        {
            int startFrom = LeaderboardRepository.LeaderboardRecordsCount + 1;
            for (int i = 0; i < accounts.Count; i++)
            {
                var record = _records[startFrom + i];
                var acc = accounts[i];
                record.SetPlayerWithoutPlace(i, acc);
            }
            
            startFrom += accounts.Count;
            for (int i = startFrom; i < _records.Length; i++)
            {
                _records[i].SetInvisible();
            }
        }
    }
}