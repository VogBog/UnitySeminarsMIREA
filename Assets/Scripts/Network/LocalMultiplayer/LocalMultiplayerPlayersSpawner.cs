using System;
using Game;
using Unity.Netcode;
using UnityEngine;

namespace Network.LocalMultiplayer
{
    public class LocalMultiplayerPlayersSpawner : NetworkBehaviour, IPlayerSpawnerPolitics
    {
        public Player Instantiate(Player prefab, Vector3 pos, Quaternion rot, Transform parent)
        {
            var networkObject = prefab.GetComponent<NetworkObject>();
            if(networkObject == null)
                throw new NullReferenceException("Player must have NetworkObject component");

            ulong ownerId = NetworkManager.LocalClientId;

            var instance = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
                networkObject,
                ownerClientId: ownerId,
                destroyWithScene: true,
                isPlayerObject: true,
                position: pos,
                rotation: rot);

            if(!instance.TryGetComponent<Player>(out var player))
                throw new NullReferenceException("WTF");

            return player;
        }
    }
}