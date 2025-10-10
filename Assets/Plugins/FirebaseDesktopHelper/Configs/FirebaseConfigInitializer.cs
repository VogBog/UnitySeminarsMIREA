using UnityEngine;

namespace FirebaseDesktopHelper.Configs
{
    public class FirebaseConfigInitializer : MonoBehaviour
    {
        [SerializeField] private ScriptableObject _config;

        private void Awake()
        {
            if (_config is not IFirebaseConfig config)
                return;
            
            FirebaseRestRequests.SetData(config);
            Destroy(this);
        }

        private void OnValidate()
        {
            if (_config is not IFirebaseConfig)
                _config = null;
        }
    }
}