using UnityEngine;

namespace Player.Abilities.Ice
{
    [CreateAssetMenu(menuName = "Player/Abilities/Ice")]
    public class IceAbilityData : PlayerAbilityScriptableData
    {
        public override IPlayerAbility CreateAbility(Player player)
            => new IceAbility(player, this);

        [field: SerializeField] public override float HeavyAbilityTime { get; protected set; }
        [field: SerializeField] public override float HeavyAbilityCooldown { get; protected set; }
        [field: SerializeField] public override float QuickAbilityCooldown { get; protected set; }

        [field: Space]
        [field: SerializeField] public int QuickDamage { get; private set; }
        [field: SerializeField] public float QuickSpeed { get; private set; }
        [field: SerializeField] public float QuickDistance { get; private set; }
        [field: SerializeField] public float QuickExplosionRadius { get; private set; }
        
        [field: Space]
        [field: SerializeField] public int WallHealth { get; private set; }
        [field: SerializeField] public float WallLifetime { get; private set; }
        
        [field: Space]
        [field: SerializeField] public IceProjectile ProjectilePrefab { get; private set; }
        [field: SerializeField] public IceWall WallPrefab { get; private set; }
    }
}