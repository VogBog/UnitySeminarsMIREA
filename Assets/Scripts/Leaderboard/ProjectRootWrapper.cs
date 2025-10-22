using System;
using System.Collections.Generic;
using Account;

namespace Leaderboard
{
    [Serializable]
    public struct ProjectRootWrapper
    {
        public Dictionary<string, PlayerAccount> Accounts;
    }
}