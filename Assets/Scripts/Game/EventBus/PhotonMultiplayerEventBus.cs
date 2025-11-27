using System;
using Photon.Pun;
using UnityEngine;

namespace Game.EventBus
{
    public class PhotonMultiplayerEventBus : MonoBehaviour, IEventBusTransport
    {
        private PhotonView _photonView;
        
        #region Utils
        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        public static int GetActorNumber(Player player)
        {
            if (!player.TryGetComponent(out PhotonView photonView))
                throw new NullReferenceException("Cannot get actor number from player, cannot find PhotonView");
            return photonView.Owner.ActorNumber;
        }

        public static (int, int) GetViewIDAndActorNumber(Player player)
        {
            if(!player.TryGetComponent(out PhotonView photonView))
                throw new NullReferenceException(
                    "Cannot get actor number and viewID from player, cannot find PhotonView");
            
            return (photonView.ViewID, photonView.Owner.ActorNumber);
        }

        public static int GetViewId(object obj)
        {
            PhotonView photonView = null;
            if (obj is PhotonView pView)
                photonView = pView;
            if (obj is MonoBehaviour monoBehaviour)
                photonView = monoBehaviour.GetComponent<PhotonView>();
            else if (obj is GameObject gameObject)
                photonView = gameObject.GetComponent<PhotonView>();
            if (photonView == null)
                throw new ArgumentException("Cannot get PhotonView from object", nameof(obj));

            return photonView.ViewID;
        }

        public static T GetObjectByViewId<T>(int viewId) where T : class
        {
            var photonView = PhotonView.Find(viewId);
            if (photonView == null)
                throw new NullReferenceException($"Cannot find PhotonView with ViewID {viewId}");

            if (typeof(T) == typeof(PhotonView))
                return photonView as T;

            if(!photonView.TryGetComponent(out T result))
                throw new NullReferenceException(
                    $"Cannot get component {typeof(T).Name} from PhotonView with ViewID {viewId}");

            return result;
        }

        public static Player GetPlayer(int viewId)
        {
            var photonView = PhotonView.Find(viewId);
            if (photonView == null)
                throw new NullReferenceException($"Cannot find PhotonView with ViewID {viewId}");

            if(!photonView.TryGetComponent(out Player player))
                throw new NullReferenceException($"Cannot get player from PhotonView with ViewID {viewId}");

            return player;
        }
#endregion

        #region Interact
        public void Interact(Player player, IInteractable interactable)
        {
            var (playerId, actorNumber) = GetViewIDAndActorNumber(player);
            int interactableId = GetViewId(interactable);
            
            if (PhotonNetwork.IsMasterClient)
            {
                interactable.Interact(player);
                EventBus.RawInstance.OpenInteracted?.Invoke(player, interactable);
                InteractRpc(actorNumber, playerId, interactableId);
            }
            else
            {
                InteractServerRpc(actorNumber, playerId, interactableId);
            }
        }

        [PunRPC]
        private void InteractServerRpc(int actorNumber, int viewId, int interactableId)
        {
            var player = GetPlayer(viewId);
            var interactable = GetObjectByViewId<IInteractable>(interactableId);
            interactable.Interact(player);
            EventBus.RawInstance.OpenInteracted?.Invoke(player, interactable);
            
            _photonView.RPC(nameof(InteractRpc), RpcTarget.All, actorNumber, viewId, interactableId);
        }

        [PunRPC]
        private void InteractRpc(int actorNumber, int viewId, int interactableId)
        {
            if(PhotonNetwork.IsMasterClient)
                return;
            
            var player = GetPlayer(viewId);
            var interactable = GetObjectByViewId<IInteractable>(interactableId);
            
            EventBus.RawInstance.OpenInteracted?.Invoke(player, interactable);
        }
        #endregion

        #region PlayerDamaged
        
        public void PlayerDamaged(Player player, int totalHealth)
        {
            int viewId = GetViewId(player);
            _photonView.RPC(nameof(PlayerDamagedRpc), RpcTarget.All, viewId, totalHealth);
        }

        [PunRPC]
        private void PlayerDamagedRpc(int viewId, int totalHealth)
        {
            var player = GetPlayer(viewId);
            EventBus.RawInstance.OpenPlayerDamaged?.Invoke(player, totalHealth);
        }
        
        #endregion

        #region PlayerDied

        public void PlayerDied(Player player)
        {
            int viewId = GetViewId(player);
            _photonView.RPC(nameof(PlayerDiedRpc), RpcTarget.All, viewId);
        }

        [PunRPC]
        private void PlayerDiedRpc(int viewId)
        {
            var player = GetPlayer(viewId);
            EventBus.RawInstance.OpenPlayerDied?.Invoke(player);
        }

        #endregion
    }
}