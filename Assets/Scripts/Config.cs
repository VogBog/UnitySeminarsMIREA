using UnityEngine;

public class Config : ScriptableObject
{
    [field: SerializeField] public string FirebaseUrl { get; private set; }
    [field: SerializeField] public string FirebaseApiKey { get; private set; }        
}
