using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace DefaultNamespace
{
    public class CameraSwitcher : MonoBehaviour
    {
        private ARCameraManager _manager;

        private void Awake()
        {
            _manager = FindFirstObjectByType<ARCameraManager>();
        }

        private void Update()
        {
            if (Input.touchCount < 1 || Input.GetTouch(0).phase != TouchPhase.Began)
                return;

            _manager.requestedFacingDirection = _manager.currentFacingDirection is CameraFacingDirection.User
                ? CameraFacingDirection.World
                : CameraFacingDirection.User;
        }
    }
}