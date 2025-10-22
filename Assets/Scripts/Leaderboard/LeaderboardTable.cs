using System;
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
        }
    }
}