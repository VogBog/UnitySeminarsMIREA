using System;
using UnityEngine;

public class TemplatesSpawner : MonoBehaviour
{
    [Serializable]
    public struct TemplateObj
    {
        public GameObject Tempalte;
        public Transform SpawnPoint;
    }

    [SerializeField] private TemplateObj[] _templates;

    private void Start()
    {
        foreach(var templateObj in _templates)
        {
            var instance = Instantiate(templateObj.Tempalte);
            instance.transform.position = templateObj.SpawnPoint.position;
            var template = instance.AddComponent<ObjectTemplate>();
            template.Init(templateObj.Tempalte);
        }
        Destroy(gameObject);
    }
}
