using System;
using System.Collections.Generic;
using Game.EventBus;

namespace Network.General
{
    public class EventBusNetwork : AbstractNetwork
    {
        private LocalMultiplayerEventBus _localMultiplayer;

        protected override void BeforeAwake()
        {
            _localMultiplayer = GetComponent<LocalMultiplayerEventBus>();
        }

        protected override void AddRequireComponents(List<Type> components)
        {
            components.Add(typeof(LocalMultiplayerEventBus));
        }

        protected override void IsNotLocalMultiplayer()
        {
            _localMultiplayer = null;
            Destroy(_localMultiplayer);
        }

        public IEventBusTransport GetTransport()
        {
            if(_localMultiplayer != null)
                return _localMultiplayer;

            return null;
        }
    }
}