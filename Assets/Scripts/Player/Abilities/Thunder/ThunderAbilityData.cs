using UnityEngine;

namespace Player.Abilities.Thunder
{
    [CreateAssetMenu(menuName = "Player/Abilities/Thunder")]
    public class ThunderAbilityData : PlayerAbilityScriptableData
    {
        public override IPlayerAbility CreateAbility(Player player)
            => new ThunderAbility(player, this);

        [field: SerializeField] public override float HeavyAbilityTime { get; protected set; }
        [field: SerializeField] public override float HeavyAbilityCooldown { get; protected set; }
        [field: SerializeField] public override float QuickAbilityCooldown { get; protected set; }
        
        [field: SerializeField] public float QuickFirstDistance { get; private set; }
        [field: SerializeField] public float QuickSecondDistance { get; private set; }
        [field: SerializeField] public int QuickDamage { get; private set; }
        
        [field: Space] [field: SerializeField] public float HeavyDistance { get; private set; }
        [field: SerializeField] public float HeavyWidth { get; private set; }
        [field: SerializeField] public int HeavyDamage { get; private set; }
        
        [field: Space] [field: SerializeField] public ThunderProjectile Projectile { get; private set; }
    }
}