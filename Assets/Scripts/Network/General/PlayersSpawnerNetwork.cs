using System;
using System.Collections.Generic;
using Network.LocalMultiplayer;
using Network.PhotonMultiplayer;

namespace Network.General
{
    public class PlayersSpawnerNetwork : AbstractNetwork
    {
        protected override void AddRequireComponents(List<Type> components)
        {
            components.Add(typeof(LocalMultiplayerPlayersSpawner));
            components.Add(typeof(PhotonMultiplayerPlayersSpawner));
        }

        protected override void IsNotLocalMultiplayer()
        {
            var playersSpawner = GetComponent<LocalMultiplayerPlayersSpawner>();
            Destroy(playersSpawner);
        }

        protected override void IsNotPhotonMultiplayer()
        {
            var playersSpawner = GetComponent<PhotonMultiplayerPlayersSpawner>();
            Destroy(playersSpawner);
        }
    }
}