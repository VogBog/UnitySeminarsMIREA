using Game.LocalMultiplayer;
using Game.PhotonMultiplayer;
using Global;

namespace Game
{
    public static class GameServices
    {
        public static ICarsSpawner CreateCarsSpawner()
        {
            var lm = UnityEngine.Object.FindFirstObjectByType<LocalMultiplayerCarsSpawner>();
            var photon = UnityEngine.Object.FindFirstObjectByType<PhotonCarsSpawner>();

            if (StaticParameters.NetworkType is StaticParameters.NetworkTypes.LocalMultiplayer)
            {
                UnityEngine.Object.Destroy(photon);
                return lm;
            }

            if (StaticParameters.NetworkType is StaticParameters.NetworkTypes.Photon)
            {
                UnityEngine.Object.Destroy(lm);
                return photon;
            }
            
            UnityEngine.Object.Destroy(lm);
            UnityEngine.Object.Destroy(photon);
            return new SimpleCarsSpawner();
        }

        public static IGameFinisher CreateGameFinisher()
        {
            if(StaticParameters.NetworkType is StaticParameters.NetworkTypes.LocalMultiplayer)
                return new LocalMultiplayerGameFinisher();
            
            if(StaticParameters.NetworkType is StaticParameters.NetworkTypes.Photon)
                return new PhotonGameFinisher();

            return new SimpleGameFinisher();
        }
    }
}