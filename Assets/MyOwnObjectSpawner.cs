using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace DefaultNamespace
{
    public class MyOwnObjectSpawner : MonoBehaviour
    {
        [SerializeField] private int _maxObjectsCount = 7;
        
        private GameObject _prefab;
        private ARRaycastManager _raycastManager;
        private readonly List<ARRaycastHit> _hits = new();
        private int _objectsCount;

        private void Awake()
        {
            _raycastManager = FindFirstObjectByType<ARRaycastManager>();
            if(_raycastManager == null) throw new NullReferenceException("Cannot find ARRaycastManager");
        }

        private bool CanSpawnObject()
        {
            return Input.touchCount > 0 &&
                   Input.GetTouch(0).phase == TouchPhase.Began &&
                   _objectsCount < _maxObjectsCount &&
                   _prefab != null;
        }
            

        private void Update()
        {
            if (!CanSpawnObject())
                return;
            
            Vector2 touchPosition = Input.GetTouch(0).position;

            if (!_raycastManager.Raycast(touchPosition, _hits, TrackableType.PlaneWithinPolygon))
                return;
            
            Pose hitPose = _hits[0].pose;
            
            Instantiate(_prefab, hitPose.position, hitPose.rotation);
            _objectsCount++;
        }

        public void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
        }
    }
}