using UnityEngine;

public class ObjectTemplate : MonoBehaviour
{
    private GameObject _prefab;

    private Painter _painter;

    private void Start()
    {
#pragma warning disable CS0618 // ��� ��� ���� �������
        _painter = FindObjectOfType<Painter>();
#pragma warning restore CS0618 // ��� ��� ���� �������
    }

    public void Init(GameObject prefab)
    {
        _prefab = prefab;
    }

    private void OnMouseDown()
    {
        _painter.SetPrefab(_prefab);
    }
}
