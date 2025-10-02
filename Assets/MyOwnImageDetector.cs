using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace DefaultNamespace
{
    public class MyOwnImageDetector : MonoBehaviour
    {
        [SerializeField] private GameObject[] _prefabs;
        
        private ARTrackedImageManager _trackedImageManager;
        private readonly Dictionary<string, GameObject> _instances = new();

        private void Awake()
        {
            _trackedImageManager = FindFirstObjectByType<ARTrackedImageManager>();
            if(_trackedImageManager == null) throw new NullReferenceException("ARTrackedImageManager not found");

            foreach (var prefab in _prefabs)
            {
                var instance = Instantiate(prefab, transform);
                instance.SetActive(false);
                _instances.Add(prefab.name, instance);
            }
        }

        private void OnEnable() => _trackedImageManager.trackablesChanged.AddListener(ImageChanged);

        private void OnDisable() => _trackedImageManager.trackablesChanged.RemoveListener(ImageChanged);

        private void ImageChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
        {
            foreach(var image in args.added)
                UpdateInstanceByImage(image);
            foreach(var image in args.updated)
                UpdateInstanceByImage(image);

            foreach (var (_, image) in args.removed)
            {
                if(_instances.TryGetValue(image.referenceImage.name, out var instance))
                    instance.SetActive(false);
            }
        }

        private void UpdateInstanceByImage(ARTrackedImage image)
        {
            if (string.IsNullOrEmpty(image.referenceImage.name))
                return;
            
            if (!_instances.TryGetValue(image.referenceImage.name, out var instance))
                return;
            
            instance.transform.SetPositionAndRotation(image.pose.position, image.pose.rotation);
            instance.SetActive(true);
        }
    }
}