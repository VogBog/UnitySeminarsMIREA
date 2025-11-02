using Game.EventBus;
using UnityEngine;

namespace Network.General
{
    [RequireComponent(typeof(LocalMultiplayerEventBus))]
    public class EventBusNetwork : AbstractNetwork
    {
        private LocalMultiplayerEventBus _localMultiplayer;

        protected override void BeforeAwake()
        {
            _localMultiplayer = GetComponent<LocalMultiplayerEventBus>();
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