using System;
using System.Collections;
using SingleGame;
using UnityEngine;

namespace Game
{
    public class KillPoint : MonoBehaviour, IInteractableObservable
    {
        public const float RadiusForEverySpeedUnit = 1f;
        
        public event Action<IInteractableObservable, GameObject> Interacted;
        
        public void Interact(Player player)
        {
            float speed = player.Movement.Speed;
            player.Movement.AddSpeed(-15f);

            float radius = RadiusForEverySpeedUnit * speed;
            var colliders = Physics.OverlapSphere(transform.position, radius / 2f);

            foreach (var collider in colliders)
            {
                if(!collider.TryGetComponent<Player>(out var otherPlayer) ||
                   otherPlayer == player)
                    continue;
                
                otherPlayer.GetDamage();
                otherPlayer.GetDamage();
            }
            
            Interacted?.Invoke(this, gameObject);

            if(gameObject.activeSelf)
                StartCoroutine(AnimationRoutine(radius));
        }

        private IEnumerator AnimationRoutine(float scale)
        {
            transform.localScale = new Vector3(scale, .1f, scale);

            yield return new WaitForSeconds(0.5f);

            transform.localScale = Vector3.one;
        }
    }
}