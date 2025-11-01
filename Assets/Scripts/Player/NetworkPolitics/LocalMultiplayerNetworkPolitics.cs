using Damage;
using Data;
using Unity.Netcode;

namespace Player.NetworkPolitics
{
    public class LocalMultiplayerNetworkPolitics : NetworkBehaviour, INetworkPolitics
    {
        private Player _player;

        public void SetPlayer(Player player)
        {
            _player = player;
        }
        
        public void TakeDamage(ref GetDamageData data)
        {
            if (IsOwner)
            {
                _player.TakeDamage(ref data);
            }
            else
            {
                var networkObject = data.Attacker.GetComponent<NetworkObject>();
                ulong id = networkObject?.NetworkObjectId ?? 0;
                bool hasObject = networkObject != null;
                
                TakeDamageServerRpc(data.Damage, id, data.Elemental.Name, hasObject);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void TakeDamageServerRpc(int damage, ulong networkId, string elementalName, bool hasAttacker)
        {
            if (IsOwner)
            {
                ConvertAndTakeDamage(damage, networkId, elementalName, hasAttacker);
            }
            else
            {
                TakeDamageClientRpc(damage, networkId, elementalName, hasAttacker);
            }
        }

        [ClientRpc]
        private void TakeDamageClientRpc(int damage, ulong networkId, string elementalName, bool hasAttacker)
        {
            if (!IsOwner)
                return;
            
            ConvertAndTakeDamage(damage, networkId, elementalName, hasAttacker);
        }

        private void ConvertAndTakeDamage(int damage, ulong networkId, string elementalName, bool hasAttacker)
        {
            NetworkObject attacker = null;
            if (hasAttacker)
            {
                NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(networkId, out attacker);
            }

            var element = Elementals.GetByName(elementalName);
            var ev = new GetDamageData(damage, attacker?.gameObject, element);
            
            _player.TakeDamage(ref ev);
        }
    }
}