using System.Collections.Generic;
using Account;
using Global;
using UnityEngine;

namespace Leaderboard
{
    public class LeaderboardTable : MonoBehaviour
    {
        [SerializeField] private Transform _recordsParent;
        
        private LeaderboardRecord[] _records;

        private void Start()
        {
            _records = _recordsParent.GetComponentsInChildren<LeaderboardRecord>();
            
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