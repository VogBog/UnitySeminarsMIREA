using System;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private MeshRenderer _renderer;
    
    private int _highlightCount = 0;
    
    public void Highlight()
    {
        _highlightCount = 2;
        _renderer.sharedMaterial = _highlightMaterial;
    }

    private void Update()
    {
        if (_highlightCount > 0)
        {
            _highlightCount--;
            if (_highlightCount == 0)
            {
                _renderer.sharedMaterial = _defaultMaterial;
            }
        }
    }
}
