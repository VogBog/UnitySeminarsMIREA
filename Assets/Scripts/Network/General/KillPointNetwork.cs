using Network.LocalMultiplayer;
using UnityEngine;

namespace Network.General
{
    [RequireComponent(typeof(LocalMultiplayerKillPointSync))]
    public class KillPointNetwork : AbstractNetwork
    {
        protected override void IsNotLocalMultiplayer()
        {
            var sync = GetComponent<LocalMultiplayerKillPointSync>();
            Destroy(sync);
        }
    }
}