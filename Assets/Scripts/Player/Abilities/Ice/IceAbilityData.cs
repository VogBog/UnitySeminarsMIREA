using UnityEngine;

namespace Player.Abilities.Ice
{
    [CreateAssetMenu(menuName = "Player/Abilities/Ice")]
    public class IceAbilityData : PlayerAbilityScriptableData
    {
        public override IPlayerAbility CreateAbility(Player player)
        {
            throw new System.NotImplementedException();
        }

        [field: SerializeField] public override float HeavyAbilityTime { get; protected set; }
        [field: SerializeField] public override float HeavyAbilityCooldown { get; protected set; }
        [field: SerializeField] public override float QuickAbilityCooldown { get; protected set; }
    }
}