using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private Colored[] objects;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int objectsCount;

    private List<Transform> points;
    private List<Colored> coloreds;

    private void Start()
    {
        points = new List<Transform>(spawnPoints);
        coloreds = new List<Colored>();
        for(int i = 0; i < objectsCount; i++)
        {
            SpawnObject();
        }
    }

    public void SpawnObject()
    {
        var prefab = objects[Random.Range(0, objects.Length)];
        var obj = Instantiate(prefab);
        var randomIndex = Random.Range(0, points.Count);
        var pos = points[randomIndex];
        points.RemoveAt(randomIndex);
        obj.transform.position = pos.position;
        obj.Initialize(colors, this);
        coloreds.Add(obj);
    }

    public void ColoredChangeColor()
    {
        var color = coloreds[0].GetCurrentColor();
        foreach(var colored in coloreds)
        {
            if(colored.GetCurrentColor() != color)
            {
                return;
            }
        }
        Debug.Log("КОНЕЦ");
    }
}
