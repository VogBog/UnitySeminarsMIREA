namespace Player.Abilities
{
    public interface IPlayerAbility
    {
        IPlayerAbilityData Data { get; }
        
        void QuickAbility();
        void HeavyAbility();
    }
}