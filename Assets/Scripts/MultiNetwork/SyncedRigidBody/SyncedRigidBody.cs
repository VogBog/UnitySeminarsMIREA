using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Global;
using Photon.Pun;
using Unity.Netcode.Components;
using UnityEngine;

namespace MultiNetwork.SyncedRigidBody
{
    public class SyncedRigidBody : MultiNetworkComponent.MultiNetworkComponent
    {
        [SerializeField] private bool _setIsKinematicOnStart;
        [SerializeField] private bool _isKinematicValue;
        
        protected override void RequireComponents(List<Type> components)
        {
            components.Add(typeof(Rigidbody));
            components.Add(typeof(NetworkRigidbody));
            components.Add(typeof(PhotonRigidbodyView));
        }

        protected override void RemoveRedundantComponents(StaticParameters.NetworkTypes type, Dictionary<Type, Component> components)
        {
            if (type is not StaticParameters.NetworkTypes.LocalMultiplayer &&
                components.TryGetValue(typeof(NetworkRigidbody), out var network))
            {
                Destroy(network);
            }

            if (type is not StaticParameters.NetworkTypes.Photon &&
                components.TryGetValue(typeof(PhotonRigidbodyView), out var photonView))
            {
                Destroy(photonView);
            }

            if (type is not StaticParameters.NetworkTypes.LocalMultiplayer &&
                _setIsKinematicOnStart &&
                components.TryGetValue(typeof(Rigidbody), out var component) &&
                component is Rigidbody rb)
            {
                SetKinematic(rb, _isKinematicValue, 200);
            }
        }

        public static async void SetKinematic(Rigidbody rb, bool value, int delayMilliseconds)
        {
            try
            {
                await Task.Delay(delayMilliseconds);
                if (rb != null)
                    rb.isKinematic = value;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}