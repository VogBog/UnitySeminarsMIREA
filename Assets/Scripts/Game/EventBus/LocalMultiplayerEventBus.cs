using Unity.Netcode;
using UnityEngine;

namespace Game.EventBus
{
    public class LocalMultiplayerEventBus : NetworkBehaviour, IEventBusTransport
    {
        private bool TryGetNetworkId(object obj, string objName, out ulong id)
        {
            id = 0;
            var network = obj as NetworkObject;
            if (network == null)
                network = (obj as MonoBehaviour)?.GetComponent<NetworkObject>();
            if (network == null)
                network = (obj as NetworkBehaviour)?.NetworkObject;

            if (network != null)
            {
                id = network.NetworkObjectId;
            }
            else
            {
                Debug.LogError($"{objName} must have NetworkObject");
            }
            
            return network != null;
        }

        private bool TryGetNetworkId(Player player, out ulong id)
        {
            var playerNetwork = player.LocalMultiplayerSync;
            id = 0;
            
            if (playerNetwork == null)
            {
                Debug.LogError("Player must have NetworkObject");
                return false;
            }
            
            id = playerNetwork.NetworkObjectId;
            return true;
        }

        private T GetFromId<T>(ulong id) where T : class
        {
            if (!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(id, out var networkObject))
            {
                Debug.LogError($"Cannot find object with ID {id} in NetworkManager.SpawnManager.SpawnedObjects");
                return null;
            }

            if (!networkObject.TryGetComponent(out T component))
            {
                Debug.LogError($"Cannot find {typeof(T).Name} component in NetworkManager.SpawnManager.SpawnedObjects");
                return null;
            }
            
            return component;
        }
        
        #region Interact
        public void Interact(Player player, IInteractable interactable)
        {
            if (!TryGetNetworkId(player, out var playerId))
                return;
            if (!TryGetNetworkId(interactable, nameof(IInteractable), out var interactableId))
                return;
            
            if (IsServer)
            {
                interactable.Interact(player);
                InteractClientRpc(playerId, interactableId);
            }
            else
            {
                InteractServerRpc(playerId, interactableId);
            }
        }

        [ServerRpc(InvokePermission = RpcInvokePermission.Everyone)]
        private void InteractServerRpc(ulong playerId, ulong interactableId)
        {
            var player = GetFromId<Player>(playerId);
            var interactable = GetFromId<IInteractable>(interactableId);
            interactable.Interact(player);
            
            InteractClientRpc(playerId, interactableId);
        }

        [ClientRpc]
        private void InteractClientRpc(ulong clientId, ulong interactableId)
        {
            var player = GetFromId<Player>(clientId);
            var interactable = GetFromId<IInteractable>(interactableId);
            EventBus.RawInstance.OpenInteracted?.Invoke(player, interactable);
        }
        #endregion
    }
}