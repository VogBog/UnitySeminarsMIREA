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
        }

        private void Update()
        {
            if (Input.touchCount < 1)
                return;
            
            Vector2 touchPosition = Input.GetTouch(0).position;

            if (!_raycastManager.Raycast(touchPosition, _hits, TrackableType.PlaneWithinPolygon))
                return;
            
            Pose hitPose = _hits[0].pose;
            
            if(_spawnedObject == null)
                _spawnedObject = Instantiate(_prefab, hitPose.position, hitPose.rotation);
            else
                _spawnedObject.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
        }
    }
}