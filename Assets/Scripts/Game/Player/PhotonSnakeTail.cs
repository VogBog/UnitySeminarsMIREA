using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using Photon.Pun;
using UnityEngine;

namespace Game.Player
{
    public class PhotonSnakeTail : MonoBehaviour, ISnakeTail
    {
        private PhotonView _photonView;
        private SnakeTail _tail;
        private SnakePoint _pointPrefab;
        private readonly List<(int, Action<SnakePoint>)> _waitingList = new();
        private readonly List<(int, Action<SnakeTailCalculationData>)> _calculationDataWaitingList = new();
        
        public StaticParameters.NetworkTypes RequiredNetworkType => StaticParameters.NetworkTypes.Photon;
        
        public event Action<SnakePoint> AddNewPoint;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }
        
        public void SetSnake(SnakeTail tail)
        {
            _tail = tail;
        }

        public void SetPrefab(SnakePoint prefab)
        {
            _pointPrefab = prefab;
        }

        public void Instantiate(Vector3 position, Quaternion rotation, Action<SnakePoint> onSpawn)
        {
            int id = 0;
            foreach (var (prevId, _) in _waitingList)
            {
                if (prevId != id)
                    break;
                id++;
            }
            
            _waitingList.Add((id, onSpawn));
            _photonView.RPC(nameof(InstantiateRpc), RpcTarget.All, id, position, rotation);
        }

        [PunRPC]
        private void InstantiateRpc(int id, Vector3 position, Quaternion rotation)
        {
            if (!_photonView.IsMine)
                return;
            
            var go = PhotonNetwork.Instantiate(
                _pointPrefab.name + " Variant",
                position,
                rotation);
            
            var point = go.GetComponent<SnakePoint>();
            var view = go.GetComponent<PhotonView>();
            
            AddNewPoint?.Invoke(point);
            
            int viewId = view.ViewID;
            _photonView.RPC(nameof(InstantiatedCallbackToServer), RpcTarget.MasterClient, id, viewId);
        }

        [PunRPC]
        private void InstantiatedCallbackToServer(int id, int viewId)
        {
            StartCoroutine(InstantiatedCallbackRoutine(id, viewId));
        }

        private IEnumerator InstantiatedCallbackRoutine(int id, int viewId)
        {
            for (int attempt = 0; attempt < 10_000; attempt++)
            {
                var view = PhotonView.Find(viewId);
                if (view == null)
                {
                    yield return null;
                    continue;
                }
                
                var point = view.GetComponent<SnakePoint>();
                if (point == null)
                {
                    yield return null;
                    continue;
                }
                
                for (int i = 0; i < _waitingList.Count; i++)
                {
                    if (_waitingList[i].Item1 == id)
                    {
                        var action = _waitingList[i].Item2;
                        _waitingList.RemoveAt(i);
                        action?.Invoke(point);
                        yield break;
                    }
                }

                yield return null;
            }
            
            Debug.LogError($"Something went wrong. Cannot instantiate view {id}");
        }

        public void DespawnAllPoints()
        {
            var points = _tail.SnakePointsCopy;
            foreach (var point in points)
            {
                PhotonNetwork.Destroy(point.gameObject);
            }
        }

        public void CalculateDataForAddLength(Action<SnakeTailCalculationData> onCalculationData)
        {
            int id = 0;
            foreach (var (prevId, _) in _calculationDataWaitingList)
            {
                if (id != prevId)
                    break;
                id++;
            }
            
            _calculationDataWaitingList.Add((id, onCalculationData));
            _photonView.RPC(nameof(CalculateDataRpc), RpcTarget.All, id);
        }

        [PunRPC]
        private void CalculateDataRpc(int id)
        {
            if (!_photonView.IsMine)
                return;

            var data = _tail.CalculateDataForAddLength();
            _photonView.RPC(nameof(CalculateDataCallback), RpcTarget.All, id, data.LastPoint);
        }

        [PunRPC]
        private void CalculateDataCallback(int id, Vector3 lastPoint)
        {
            var data = new SnakeTailCalculationData(lastPoint);
            
            for (int i = 0; i < _calculationDataWaitingList.Count; i++)
            {
                if (_calculationDataWaitingList[i].Item1 == id)
                {
                    var action = _calculationDataWaitingList[i].Item2;
                    _calculationDataWaitingList.RemoveAt(i);
                    action?.Invoke(data);
                    return;
                }
            }
        }
    }
}