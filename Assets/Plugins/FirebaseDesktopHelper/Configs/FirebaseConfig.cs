using UnityEngine;

namespace FirebaseDesktopHelper.Configs
{
    [CreateAssetMenu]
    public class FirebaseConfig : ScriptableObject, IFirebaseConfig
    {
        [field: SerializeField] public string RealtimeDatabaseUrl { get; private set; }
        [field: SerializeField] public string ApiKey { get; private set; }
    }
}