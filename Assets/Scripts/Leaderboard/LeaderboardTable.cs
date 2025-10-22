using System.Collections.Generic;
using Account;
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
    }
}