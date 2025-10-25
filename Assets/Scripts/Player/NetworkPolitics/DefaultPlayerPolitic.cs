using Damage;

namespace Player.NetworkPolitics
{
    public class DefaultPlayerPolitic : INetworkPolitics
    {
        private readonly PlayerHealth _playerHealth;

        public DefaultPlayerPolitic(PlayerHealth health)
        {
            _playerHealth = health;
        }
        
        public void TakeDamage(ref GetDamageData data) => _playerHealth.TakeDamage(ref data);
    }
}