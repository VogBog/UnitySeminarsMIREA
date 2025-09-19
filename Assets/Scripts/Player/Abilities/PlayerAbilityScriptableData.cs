using UnityEngine;

namespace Player.Abilities
{
    public abstract class PlayerAbilityScriptableData : ScriptableObject, IPlayerAbilityData
    {
        public abstract IPlayerAbility CreateAbility(Player player);

        public abstract float HeavyAbilityTime { get; protected set; }
        public abstract float HeavyAbilityCooldown { get; protected set; }
        public abstract float QuickAbilityCooldown { get; protected set; }
    }
}