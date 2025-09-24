using UnityEngine;

namespace Data
{
    public class Elementals : MonoBehaviour
    {
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
        }
    }
}