using System;
using Damage;
using Player;
using UnityEngine;

namespace MainGame
{
    public class EventBus : MonoBehaviour
    {
        public delegate void GetDamageDelegate(Player.Player player, ref GetDamageData data);

        public event GetDamageDelegate PlayerTakeDamage;
        public event Action<PlayerHealth, Player.Player> PlayerKilledByAnotherPlayer; 

        public void InvokePlayerTakeDamage(Player.Player damageReceiver, ref GetDamageData data)
        {
            PlayerTakeDamage?.Invoke(damageReceiver, ref data);
        }

        public void InvokePlayerKilledByAnotherPlayer(PlayerHealth health, Player.Player killer)
        {
            PlayerKilledByAnotherPlayer?.Invoke(health, killer);
        }
    }
}