using System;
using System.Collections.Generic;
using Network.LocalMultiplayer;

namespace Network.General
{
    public class KillPointNetwork : AbstractNetwork
    {
        protected override void AddRequireComponents(List<Type> components)
        {
            components.Add(typeof(LocalMultiplayerKillPointSync));
        }

        protected override void IsNotLocalMultiplayer()
        {
            var sync = GetComponent<LocalMultiplayerKillPointSync>();
            Destroy(sync);
        }
    }
}