using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace DefaultNamespace
{
    public class FaceMeshAdder : MonoBehaviour
    {
        [SerializeField] private ARFace _face;
        [SerializeField] private int _sphereIndex;
        [SerializeField] private GameObject _spherePrefab;
        [SerializeField] private int[] _hornsIndexes;
        [SerializeField] private GameObject _hornPrefab;

        private readonly Dictionary<int, Transform> _objects = new();

        private void OnEnable() => _face.updated += SetObjectsPositions;
        private void OnDisable() => _face.updated -= SetObjectsPositions;

        private void SetObjectsPositions(ARFaceUpdatedEventArgs args)
        {
            SetObjectPosition(_sphereIndex);
            foreach(var horn in _hornsIndexes)
                SetObjectPosition(horn);
        }

        private void SetObjectPosition(int index)
        {
            var obj = GetObject(index);
            obj.transform.position = _face.transform.TransformPoint(_face.vertices[index]);
        }

        private Transform GetObject(int index)
        {
            if (_objects.TryGetValue(index, out var obj))
                return obj;

            var prefab = GetPrefab(index);
            var instance = Instantiate(prefab);
            _objects.Add(index, instance);

            return instance;
        }

        private Transform GetPrefab(int index)
        {
            if (index == _sphereIndex)
                return _spherePrefab.transform;
            return _hornPrefab.transform;
        }
    }
}