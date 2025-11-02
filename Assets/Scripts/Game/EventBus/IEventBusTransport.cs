namespace Game.EventBus
{
    public interface IEventBusTransport
    {
        void Interact(Player player, IInteractable interactable);
        void PlayerDamaged(Player player, int totalHealth);
        void PlayerDied(Player player);
    }
}