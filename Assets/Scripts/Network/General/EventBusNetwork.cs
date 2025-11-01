using Game.EventBus;
using UnityEngine;

namespace Network.General
{
    [RequireComponent(typeof(LocalMultiplayerEventBus))]
    public class EventBusNetwork : AbstractNetwork
    {
        protected override void IsNotLocalMultiplayer()
        {
            var eventBus = GetComponent<LocalMultiplayerEventBus>();
            Destroy(eventBus);
        }
    }
}