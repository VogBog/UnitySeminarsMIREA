using System.Collections;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SpeedUpWall : MonoBehaviour, IInteractable
    {
        [SerializeField] private Material _enabledMat, _disabledMat;
        private MeshRenderer _renderer;
        
        public const float SpeedUp = 1f;
        public const float ReloadTime = 5f;

        public bool Enabled { get; private set; } = true;

        private void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
            EventBus.EventBus.SubscribeOnInteracted(OnInteracted);
            
            Enabled = true;
            _renderer.sharedMaterial = _enabledMat;
        }
        
        public void Interact(Player player)
        {
            if (!Enabled)
                return;
            
            player.Movement.AddSpeed(SpeedUp);

            Enabled = false;
            StartCoroutine(EnableRoutine());
        }

        private void OnInteracted(Player player, IInteractable interactable)
        {
            if (interactable == this && Enabled)
            {
                Enabled = false;
                StartCoroutine(EnableRoutine());
            }
        }

        private IEnumerator EnableRoutine()
        {
            _renderer.sharedMaterial = _disabledMat;
            
            yield return new WaitForSeconds(ReloadTime);
            
            _renderer.sharedMaterial = _enabledMat;
            Enabled = true;
        }
    }
}