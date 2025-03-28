using UnityEngine;

public class ButtonForSpawnObject : Button
{
    [SerializeField] private GameObject target;

    protected override void OnActivate()
    {
        target.SetActive(!target.activeSelf);
    }
}
