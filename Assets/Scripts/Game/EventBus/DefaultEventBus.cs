namespace Game.EventBus
{
    public class DefaultEventBus : IEventBusTransport
    {
        public void Interact(Player player, IInteractable interactable)
        {
            interactable.Interact(player);
            EventBus.RawInstance.OpenInteracted?.Invoke(player, interactable);
        }

        public void PlayerDamaged(Player player, int totalHealth)
        {
            EventBus.RawInstance.OpenPlayerDamaged?.Invoke(player, totalHealth);
        }

        public void PlayerDied(Player player)
        {
            EventBus.RawInstance.OpenPlayerDied?.Invoke(player);
        }
    }
}