namespace Game.EventBus
{
    public class DefaultEventBus : IEventBusTransport
    {
        public void Interact(Player player, IInteractable interactable)
        {
            interactable.Interact(player);
            EventBus.RawInstance.OpenInteracted?.Invoke(player, interactable);
        }
    }
}