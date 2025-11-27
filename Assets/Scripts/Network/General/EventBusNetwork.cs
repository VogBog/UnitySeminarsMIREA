using System;
using System.Collections.Generic;
using Game.EventBus;

namespace Network.General
{
    public class EventBusNetwork : AbstractNetwork
    {
        private LocalMultiplayerEventBus _localMultiplayer;
        private PhotonMultiplayerEventBus _photonMultiplayer;
        
        protected override void AddRequireComponents(List<Type> components)
        {
            components.Add(typeof(LocalMultiplayerEventBus));
            components.Add(typeof(PhotonMultiplayerEventBus));
        }

        protected override void BeforeAwake()
        {
            _localMultiplayer = GetComponent<LocalMultiplayerEventBus>();
            _photonMultiplayer = GetComponent<PhotonMultiplayerEventBus>();
        }

        protected override void IsNotLocalMultiplayer()
        {
            Destroy(_localMultiplayer);
            _localMultiplayer = null;
        }

        protected override void IsNotPhotonMultiplayer()
        {
            Destroy(_photonMultiplayer);
            _photonMultiplayer = null;
        }

        public IEventBusTransport GetTransport()
        {
            if(_localMultiplayer != null)
                return _localMultiplayer;
            
            if(_photonMultiplayer != null)
                return _photonMultiplayer;

            return null;
        }
    }
}