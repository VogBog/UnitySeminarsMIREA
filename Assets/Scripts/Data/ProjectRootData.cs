using System;
using System.Collections.Generic;
using Account;

namespace Data
{
    [Serializable]
    public struct ProjectRootData
    {
        public Dictionary<string, PlayerAccount> PlayerAccounts;
    }
}