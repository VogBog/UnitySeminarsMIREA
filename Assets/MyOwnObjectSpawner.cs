using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace DefaultNamespace
{
    public class MyOwnObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        
        private ARRaycastManager _raycastManager;
        private GameObject _spawnedObject;
        private readonly List<ARRaycastHit> _hits = new();

        private void Awake()
        {
            _raycastManager = FindFirstObjectByType<ARRaycastManager>();
            if(_raycastManager == null) throw new NullReferenceException("Cannot find ARRaycastManager");
        }

        private void Update()
        {
            if (Input.touchCount < 1)
                return;
            
            var touchPosition = Input.GetTouch(0).position;

            if (!_raycastManager.Raycast(touchPosition, _hits, TrackableType.PlaneWithinPolygon))
                return;
            
            var hitPose = _hits[0].pose;
            
            _spawnedObject ??= Instantiate(_prefab, hitPose.position, hitPose.rotation);
            _spawnedObject.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
        }
    }
}