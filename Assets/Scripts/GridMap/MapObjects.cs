using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using GridMap.EmptyMonoBehs;
using Pool;
using UnityEngine;

namespace GridMap
{
    public class MapObjects : MonoBehaviour
    {
        [SerializeField] private FireParticles _fire;
        [SerializeField] private IceFloor _iceFloor;
        [SerializeField] private WaterFloor _waterFloor;
        [SerializeField] private ElectroWaterFloor _electroWaterFloor;
        [SerializeField] private SteamAir _steamAir;

        private ObjectPool _pool;
        private readonly List<(byte, Component, Type)> _components = new();

        private void Awake()
        {
            StartCoroutine(InitializeRoutine());
        }

        private IEnumerator InitializeRoutine()
        {
            _pool = this.FindFirstObjectByTypeOrException<ObjectPool>();
            var map = this.FindFirstObjectByTypeOrException<GridMap>();

            while (_pool.GetPool() == null || !map.Initialized)
                yield return null;
            
            RegisterComponentRange(
                typeof(FireParticles), _fire,
                GridMapValues.Fire5Seconds, GridMapValues.Fire25Seconds,
                Mathf.RoundToInt(32 * map.CellsPerUnit));
            
            RegisterComponent(
                typeof(IceFloor), _iceFloor,
                GridMapValues.IceFloor, Mathf.RoundToInt(64 * map.CellsPerUnit));
            
            RegisterComponent(
                typeof(WaterFloor),
                _waterFloor,
                GridMapValues.Water,
                Mathf.RoundToInt(32 * map.CellsPerUnit));
            
            RegisterComponent(
                typeof(SteamAir),
                _steamAir,
                GridMapValues.Steam,
                Mathf.RoundToInt(16 * map.CellsPerUnit));
            
            RegisterComponentRange(
                typeof(ElectroWaterFloor),
                _electroWaterFloor,
                GridMapValues.ThunderWater5Seconds,
                GridMapValues.ThunderWater30Seconds,
                Mathf.RoundToInt(32 * map.CellsPerUnit));
        }

        public void RegisterComponent(Type type, Component component, byte id, int count)
        {
            _pool.RegisterAndInstantiatePrefab(type, new(component, null, null), count);
            _components.Add((id, component, type));
        }

        public void RegisterComponentRange(Type type, Component component, byte idFirst, byte idLast, int count)
        {
            _pool.RegisterAndInstantiatePrefab(type, new(component, null, null), count);
            for (byte i = idFirst; i <= idLast; i++)
            {
                _components.Add((i, component, type));
            }
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