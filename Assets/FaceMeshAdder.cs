using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace DefaultNamespace
{
    public class FaceMeshAdder : MonoBehaviour
    {
        [SerializeField] private ARFace _face;
        [SerializeField] private int[] _indexes;
        [SerializeField] private GameObject _prefab;

        private readonly Dictionary<int, Transform> _objects = new();

        private void OnEnable() => _face.updated += SetObjectsPositions;
        private void OnDisable() => _face.updated -= SetObjectsPositions;

        private void SetObjectsPositions(ARFaceUpdatedEventArgs args)
        {
            foreach (var index in _indexes)
            {
                var obj = GetObject(index);
                obj.transform.position = _face.transform.TransformPoint(_face.vertices[index]);
            }
        }

        private Transform GetObject(int index)
        {
            if (_objects.TryGetValue(index, out var obj))
                return obj;

            var instance = Instantiate(_prefab);
            _objects.Add(index, instance.transform);

            return instance.transform;
        }
    }
}