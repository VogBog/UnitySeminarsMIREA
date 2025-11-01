using Network.LocalMultiplayer;
using UnityEngine;

namespace Network.General
{
    [RequireComponent(typeof(LocalMultiplayerPlayersSpawner))]
    public class PlayersSpawnerNetwork : AbstractNetwork
    {
        protected override void IsNotLocalMultiplayer()
        {
            var playersSpawner = GetComponent<LocalMultiplayerPlayersSpawner>();
            Destroy(playersSpawner);
        }
    }
}