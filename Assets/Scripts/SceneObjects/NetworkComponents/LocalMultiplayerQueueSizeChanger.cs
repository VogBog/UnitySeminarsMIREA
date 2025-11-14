using System.Collections;
using Data;
using MainMenu;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace SceneObjects.NetworkComponents
{
    public class LocalMultiplayerQueueSizeChanger : MonoBehaviour
    {
        [SerializeField] private int _changeMaxPacketSizeTo = 128;
        
        private void Awake()
        {
            if(StaticParameters.NetworkType is NetworkTypes.LocalMultiplayer)
                StartCoroutine(AwakeRoutine());
        }

        private IEnumerator AwakeRoutine()
        {
            var config = NetworkManager.Singleton.NetworkConfig.NetworkTransport as UnityTransport;

            yield return new WaitForSeconds(5f);

            config.MaxPacketQueueSize = _changeMaxPacketSizeTo;
        }
    }
}