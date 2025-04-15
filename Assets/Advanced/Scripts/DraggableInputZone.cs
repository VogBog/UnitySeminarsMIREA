using UnityEngine;

namespace Advanced.Scripts
{
    public abstract class DraggableInputZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Button") &&
                other.TryGetComponent<DraggableItem>(out _))
            {
                OnActivate();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Button") &&
                other.TryGetComponent<DraggableItem>(out _))
            {
                OnDeactivate();
            }
        }

        protected abstract void OnActivate();
        protected abstract void OnDeactivate();
    }
}
