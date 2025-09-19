using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : MonoBehaviour, IInteractable
    {
        [SerializeField] private Canvas _canvas;
        
        public PlayerMovement Movement { get; private set; }

        public int Health { get; private set; } = 3;

        public event Action<Player> Died; 

        public void Initialize(PlayersSpawner spawner)
        {
            spawner.End += OnGameEnd;
            
            Movement = GetComponent<PlayerMovement>();
            
            Movement.Initialize(this);
        }

        public void GetDamage()
        {
            Health--;
            if (Health == 0)
            {
                Died?.Invoke(this);
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(HurtAnimationRoutine());
            }
        }

        public void SetCameraToCanvas(Camera camera)
        {
            _canvas.worldCamera = camera;
            _canvas.planeDistance = 5f;
        }

        private IEnumerator HurtAnimationRoutine()
        {
            for (int i = 0; i < 3; i++)
            {
                transform.localScale = Vector3.one * 1.3f;
                
                yield return new WaitForSeconds(0.3f);

                transform.localScale = Vector3.one * .8f;

                yield return new WaitForSeconds(0.3f);
            }
            
            transform.localScale = Vector3.one;
        }

        private void OnGameEnd()
        {
            Movement.enabled = false;
        }

        public void Interact(Player player)
        {
            if(Movement.IsStopped)
                GetDamage();
        }
    }
}