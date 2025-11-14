using UnityEngine;

namespace Game
{
    public class SimpleGameFinisher : IGameFinisher
    {
        public CarMapRunner[] GetRunnersForRecord()
            => Object.FindObjectsByType<CarMapRunner>(FindObjectsSortMode.InstanceID);

        public bool CanFinish() => true;

        public void OnBeforeLoadingScene()
        {
        }
    }
}