namespace Damage
{
    public interface IDamageable
    {
        void TakeDamage(ref GetDamageData data);
    }
}