using System.Collections;
using Tests;
using UnityEngine;

namespace GridMap.Tiles
{
    public class TileWithParticles : EmptyMonoBeh
    {
        [SerializeField] private ParticleSystem _particleSystem;

        public override void OnShow()
        {
            _particleSystem.Play();
        }

        public override void OnHide()
        {
            StartCoroutine(HideRoutine());
        }

        private IEnumerator HideRoutine()
        {
            _particleSystem.Stop();

            yield return new WaitForSeconds(1.5f);
            
            InvokeHide();
        }
    }
}