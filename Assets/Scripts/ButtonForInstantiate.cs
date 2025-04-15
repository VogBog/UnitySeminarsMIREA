using UnityEngine;

public class ButtonForInstantiate : Button
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject prefab;
    
    protected override void OnActivate()
    {
        Instantiate(prefab, spawnPoint.position, Quaternion.identity).SetActive(true);
    }
}
