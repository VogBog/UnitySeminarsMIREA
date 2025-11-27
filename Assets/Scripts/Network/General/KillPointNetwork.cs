using System;
using System.Collections.Generic;
using Network.LocalMultiplayer;
using Network.PhotonMultiplayer;

namespace Network.General
{
    public class KillPointNetwork : AbstractNetwork
    {
        protected override void AddRequireComponents(List<Type> components)
        {
            components.Add(typeof(LocalMultiplayerKillPointSync));
            components.Add(typeof(PhotonMultiplayerKillPointSync));
        }

        protected override void IsNotLocalMultiplayer()
        {
            var sync = GetComponent<LocalMultiplayerKillPointSync>();
            Destroy(sync);
        }

        protected override void IsNotPhotonMultiplayer()
        {
            var sync = GetComponent<PhotonMultiplayerKillPointSync>();
            Destroy(sync);
        }
    }
}