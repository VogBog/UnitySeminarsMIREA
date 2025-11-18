using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Player
{
    public class CamerasSwitcher : MonoBehaviour
    {
        public Camera ActiveCamera { get; private set; }

        public void SetActiveCamera([CanBeNull] Camera camera)
        {
            if (camera == null)
                return;
            
            if (ActiveCamera != null)
            {
                ActiveCamera.gameObject.SetActive(false);
            }
            
            ActiveCamera = camera;
            camera.gameObject.SetActive(true);
        }
        
        public void SwitchCamera(Player from)
        {
            var players = FindObjectsByType<Player>(FindObjectsSortMode.None);
            var followTo = players.FirstOrDefault(x => x != from);

            if (followTo == null)
            {
                SetCameraToVoid();
                return;
            }

            var cam = followTo.GetComponentInChildren<Camera>(true);
            if (cam == null)
            {
                SetCameraToVoid();
                return;
            }
            
            SetActiveCamera(cam);
        }

        private void SetCameraToVoid()
        {
            ActiveCamera.transform.SetParent(null);
        }
    }
}