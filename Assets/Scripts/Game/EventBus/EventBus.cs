using System;
using UnityEngine;

namespace Game.EventBus
{
    public class EventBus : MonoBehaviour
    {
        private static EventBus _instance;
        private IEventBusTransport _transport;

        public Action<Player, IInteractable> OpenInteracted;

        public static EventBus RawInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<EventBus>();
                    _instance?.Initialize();
                }
                    
                return _instance;
            }
        }

        private void Initialize()
        {
            _transport = GetComponentInChildren<IEventBusTransport>() ?? new DefaultEventBus();
        }

        public static IEventBusTransport Transport => RawInstance._transport;
        
        #region Subscribing
        public static void SubscribeOnInteracted(Action<Player, IInteractable> onInteracted)
        => RawInstance.OpenInteracted += onInteracted;
        #endregion
        
        #region Unsubscribing
        public static void UnsubscribeOnInteracted(Action<Player, IInteractable> onInteracted)
        => RawInstance.OpenInteracted -= onInteracted;
        #endregion

        #region Transport
        public static void Interact(Player player, IInteractable interactable)
            => RawInstance._transport.Interact(player, interactable);
        #endregion
    }
}