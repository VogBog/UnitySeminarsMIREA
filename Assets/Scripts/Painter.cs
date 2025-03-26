using UnityEngine;

public class Painter : MonoBehaviour
{
    private GameObject _curPrefab;
    private Camera _camera;
    private bool _canDraw = true;

    private void Start()
    {
        _camera = Camera.main;
    }

    public void SetPrefab(GameObject prefab)
    {
        _curPrefab = prefab;
        _canDraw = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canDraw)
        {
            PaintFromMouse();
        }
        _canDraw = true;
    }

    public void PaintFromMouse()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out var hit, 100f))
        {
            Paint(hit.point);
        }
    }

    public void Paint(Vector3 pos)
    {
        if (_curPrefab == null)
            return;

        var instance = Instantiate(_curPrefab);
        instance.transform.position = pos;
    }
}
