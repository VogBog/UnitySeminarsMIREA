using UnityEngine;

public class ButtonForSpawnObject : Button
{
    [SerializeField] private GameObject target;
    public bool activate;

    protected override void OnActivate()
    {
        target.gameObject.SetActive(activate);
    }
}
