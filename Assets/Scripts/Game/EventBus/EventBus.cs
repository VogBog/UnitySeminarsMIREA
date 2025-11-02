using System;
using UnityEngine;

namespace Game.EventBus
{
    public class EventBus : MonoBehaviour
    {
        private static EventBus _instance;
        private IEventBusTransport _transport;

        public Action<Player, IInteractable> OpenInteracted;
        public Action<Player, int> OpenPlayerDamaged;
        public Action<Player> OpenPlayerDied;

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
        
        public static void SubscribeOnPlayerDamaged(Action<Player, int> onPlayerDamaged)
        => RawInstance.OpenPlayerDamaged += onPlayerDamaged;

        public static void SubscribeOnPlayerDied(Action<Player> onPlayerDied)
            => RawInstance.OpenPlayerDied += onPlayerDied;
        #endregion
        
        #region Unsubscribing
        public static void UnsubscribeOnInteracted(Action<Player, IInteractable> onInteracted)
        => RawInstance.OpenInteracted -= onInteracted;
        
        public static void UnsubscribeOnPlayerDamaged(Action<Player, int> onPlayerDamaged)
        => RawInstance.OpenPlayerDamaged -= onPlayerDamaged;

        public static void UnsubscribeOnPlayerDied(Action<Player> onPlayerDied)
            => RawInstance.OpenPlayerDied -= onPlayerDied;
        #endregion

        #region Transport
        public static void Interact(Player player, IInteractable interactable)
            => RawInstance._transport.Interact(player, interactable);

        public static void PlayerDamaged(Player player, int totalHealth)
            => RawInstance._transport.PlayerDamaged(player, totalHealth);
        
        public static void PlayerDied(Player player)
            => RawInstance._transport.PlayerDied(player);

        #endregion
    }
}