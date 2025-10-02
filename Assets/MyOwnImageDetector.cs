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

            foreach (var prefab in _prefabs)
            {
                var obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                _instances.Add(prefab.name, obj);
            }
        }

        private void OnEnable()
        {
            _trackedImageManager.trackablesChanged.AddListener(ImageChanged);
        }

        private void OnDisable()
        {
            _trackedImageManager.trackablesChanged.RemoveListener(ImageChanged);
        }

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
            
            if (!_instances.TryGetValue(image.referenceImage.name, out var obj))
                return;

            obj.transform.position = image.pose.position;
            obj.transform.rotation = image.pose.rotation;
            obj.SetActive(true);
        }
    }
}