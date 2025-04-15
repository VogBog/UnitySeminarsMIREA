using UnityEngine;

namespace Advanced.Scripts
{
    public class DraggableItem : Button
    {
        private Rigidbody _rb;
        private Camera _camera;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _camera = Camera.main;
        }

        protected override void OnActivate()
        {
            _rb.isKinematic = true;
            transform.SetParent(_camera.transform);
        }

        private void OnDeactivate()
        {
            transform.SetParent(null);
            _rb.isKinematic = false;
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                OnDeactivate();
            }
        }
    }
}
