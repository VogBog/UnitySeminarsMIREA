using System;
using System.Collections.Generic;
using Extensions;
using Pool;
using Tests;
using UnityEngine;

namespace GridMap
{
    public class MapObjects : MonoBehaviour
    {
        [SerializeField] private EmptyMonoBeh _testPrefab;
        
        private readonly List<(byte, Component, Type)> _components = new();

        private void Awake()
        {
            var pool = this.FindFirstObjectByTypeOrException<ObjectPool>();
            
            pool.RegisterAndInstantiatePrefab(
                typeof(EmptyMonoBeh),
                new (_testPrefab, null, null),
                8);
            
            _components.Add((1, _testPrefab, typeof(EmptyMonoBeh)));
        }

        public byte ComponentToByte(Component component)
        {
            foreach (var pair in _components)
            {
                if(pair.Item2 == component)
                    return pair.Item1;
            }

            return 0;
        }

        public Component ByteToComponent(byte b)
        {
            foreach (var pair in _components)
            {
                if(pair.Item1 == b)
                    return pair.Item2;
            }

            return null;
        }

        public Type ByteToType(byte b)
        {
            foreach (var pair in _components)
            {
                if(pair.Item1 == b)
                    return pair.Item3;
            }
            
            return null;
        }

        public byte TypeToByte(Type type)
        {
            foreach (var pair in _components)
            {
                if(pair.Item3 == type)
                    return pair.Item1;
            }

            return 0;
        }
    }
}