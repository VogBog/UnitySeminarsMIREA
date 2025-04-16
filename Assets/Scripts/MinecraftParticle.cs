using System;
using System.Collections;
using UnityEngine;

public class MinecraftParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private GameObject cube;

    private bool isAnimating;

    private void OnMouseDown()
    {
        if (isAnimating)
            return;
        isAnimating = true;

        StartCoroutine(AnimationRoutine());
    }

    IEnumerator AnimationRoutine()
    {
        cube.SetActive(false);
        particleSystem.Play();
        
        yield return new WaitForSeconds(4f);
        
        cube.SetActive(true);
        isAnimating = false;
    }
}
