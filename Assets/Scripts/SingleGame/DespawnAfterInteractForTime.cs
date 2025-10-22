using System.Collections;
using Game;
using UnityEngine;

namespace SingleGame
{
    public class DespawnAfterInteractForTime : MonoBehaviour
    {
        public const float Duration = 10f;

        private void Start()
        {
            var killPoints = FindObjectsByType<KillPoint>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var killPoint in killPoints)
            {
                killPoint.InteractedWithSpeed += OnInteract;
            }
        }

        private void OnInteract(KillPoint killPoint, float speed)
        {
            killPoint.gameObject.SetActive(false);
            StartCoroutine(InteractRoutine(killPoint.gameObject));
        }

        private IEnumerator InteractRoutine(GameObject go)
        {
            yield return new WaitForSeconds(Duration);
            
            go.SetActive(true);
        }
    }
}