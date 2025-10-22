using FirebaseDesktopHelper.Configs;
using UnityEngine;

namespace Global
{
    [CreateAssetMenu(fileName = "Firebase config")]
    public class FirebaseConfig : ScriptableObject, IFirebaseConfig
    {
        [field: SerializeField] public string RealtimeDatabaseUrl { get; private set; }
        [field: SerializeField] public string ApiKey { get; private set; }
    }
}