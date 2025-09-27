using System.Collections;
using Damage;
using DG.Tweening;
using UnityEngine;

namespace SceneObjects
{
    [RequireComponent(typeof(Collider))]
    public class TargetObj : MonoBehaviour, IDamageable
    {
        public void TakeDamage(GetDamageData data)
        {
            StartCoroutine(TakeDamageAnimationRoutine());
        }

        private IEnumerator TakeDamageAnimationRoutine()
        {
            transform.DOScale(Vector3.one * 1.3f, .5f).SetEase(Ease.OutCubic);
            
            yield return new WaitForSeconds(.5f);

            transform.DOScale(Vector3.one * .7f, .5f).SetEase(Ease.InCubic);
            
            yield return new WaitForSeconds(.5f);

            transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutElastic);
        }
    }
}