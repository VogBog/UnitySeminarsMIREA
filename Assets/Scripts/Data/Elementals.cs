using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Data
{
    public class Elementals : MonoBehaviour
    {
        private readonly List<ElementalData> _allElements = new();
        
        [field: SerializeField] public ElementalData FireData { get; private set; }
        [field: SerializeField] public ElementalData IceData { get; private set; }
        [field: SerializeField] public ElementalData ThunderData { get; private set; }
        
        public static Elementals Instance { get; private set; }

        public static ElementalData Fire => Instance.FireData;
        public static ElementalData Ice => Instance.IceData;
        public static ElementalData Thunder => Instance.ThunderData;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            
            RegisterElements(FireData, IceData, ThunderData);
        }

        private void RegisterElements(params ElementalData[] allData)
        {
            _allElements.AddRange(allData);
        }

        [CanBeNull]
        public ElementalData GetData(string name)
        {
            return _allElements.Find(x => x.Name == name);
        }
        
        [CanBeNull]
        public static ElementalData GetByName([NotNull] string name) => Instance?.GetData(name);
    }
}