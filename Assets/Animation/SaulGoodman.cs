using UnityEngine;

public class SaulGoodman : MonoBehaviour
{
    [SerializeField] private Texture[] _sprites;
    [SerializeField] private float _fps;

    private float _secondsForFrame;
    private Material _material;

    private float _time;
    private int _index;

    private void Awake()
    {
        var renderer = GetComponent<MeshRenderer>();
        _material = renderer.sharedMaterial;
        _secondsForFrame = 1.0f / _fps;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time >= _secondsForFrame)
        {
            _time = 0f;
            _index = (_index + 1) % _sprites.Length;
            _material.mainTexture = _sprites[_index];
        }
    }
}
