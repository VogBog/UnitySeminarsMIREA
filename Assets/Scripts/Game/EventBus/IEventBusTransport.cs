namespace Game.EventBus
{
    public interface IEventBusTransport
    {
        void Interact(Player player, IInteractable interactable);
    }
}