using UnityEngine;

public class ButtonOnOffObject : Button
{
    [SerializeField] private GameObject[] _objects;
    
    protected override void OnActivate()
    {
        foreach (var obj in _objects)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }
}
