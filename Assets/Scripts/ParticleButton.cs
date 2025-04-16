using System;
using UnityEngine;

public class ParticleButton : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    
    private void OnMouseDown()
    {
        particleSystem.Play();
    }
}
