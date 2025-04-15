using UnityEngine;

namespace Advanced.Scripts
{
    public class DraggableInputZoneForSetActive : DraggableInputZone
    {
        [SerializeField] private GameObject _object;
        
        protected override void OnActivate()
        {
            _object.SetActive(false);
        }

        protected override void OnDeactivate()
        {
            _object.SetActive(true);
        }
    }
}
