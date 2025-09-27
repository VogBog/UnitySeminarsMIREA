using Pool;
using UnityEngine;

namespace Player.Abilities.Fire
{
    [CreateAssetMenu(menuName = "Player/Abilities/Fire")]
    public class FireAbilityData : PlayerAbilityScriptableData
    {
        public override IPlayerAbility CreateAbility(Player player) => new FireAbility(player, this);
        [field: SerializeField] public override float HeavyAbilityTime { get; protected set; }
        [field: SerializeField] public override float HeavyAbilityCooldown { get; protected set; }
        [field: SerializeField] public override float QuickAbilityCooldown { get; protected set; }
        
        [field: SerializeField] public int QuickDamage { get; protected set; }
        [field: SerializeField] public float QuickDistance { get; protected set; }
        [field: SerializeField] public float QuickSpeed { get; protected set; }
        [field: SerializeField] public float QuickFireRadius { get; protected set; }
        
        [field: SerializeField] public int HeavyDamage { get; protected set; }
        [field: SerializeField] public float HeavyDistance { get; protected set; }
        [field: SerializeField] public float HeavySpeed { get; protected set; }
        [field: SerializeField] public float HeavyRadius { get; protected set; }
        
        [field: Space] [field: SerializeField] public FireProjectile ProjectilePrefab { get; protected set; }
        [field: SerializeField] public FireHeavyProjectile HeavyProjectilePrefab { get; protected set; }
        [field: SerializeField] public FireProjectileParticles ProjectileParticles { get; protected set; }
        [field: SerializeField] public FireHeavyProjectileParticles HeavyProjectileParticles { get; protected set; }
        [field: SerializeField] public ExplosionEffect ExplosionEffect { get; private set; }
    }
}