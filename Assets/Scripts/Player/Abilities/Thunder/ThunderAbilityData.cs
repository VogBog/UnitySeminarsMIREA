using UnityEngine;

namespace Player.Abilities.Thunder
{
    [CreateAssetMenu(menuName = "Player/Abilities/Thunder")]
    public class ThunderAbilityData : PlayerAbilityScriptableData
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