using System;
using System.Collections.Generic;
using Network.LocalMultiplayer;

namespace Network.General
{
    public class PlayersSpawnerNetwork : AbstractNetwork
    {
        protected override void AddRequireComponents(List<Type> components)
        {
            components.Add(typeof(LocalMultiplayerPlayersSpawner));
        }

        protected override void IsNotLocalMultiplayer()
        {
            var playersSpawner = GetComponent<LocalMultiplayerPlayersSpawner>();
            Destroy(playersSpawner);
        }
    }
}