namespace Player.Abilities
{
    public interface IPlayerAbilityData
    {
        IPlayerAbility CreateAbility(Player player);
        float HeavyAbilityTime { get; }
        float HeavyAbilityCooldown { get; }
        float QuickAbilityCooldown { get; }
    }
}