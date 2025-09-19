using UnityEngine;

namespace Game
{
    public class CarMapRunner : MonoBehaviour
    {
        public int CheckPointIndex { get; private set; }
        public int RoundsCount { get; private set; }
        public bool IsFinished { get; private set; }

        public void GoThroughCheckPoint(int index)
        {
            if (index > CheckPointIndex)
            {
                CheckPointIndex = index;
            }
            else if (index == 0)
            {
                CheckPointIndex = 0;
                RoundsCount++;
            }
        }

        public bool TryFinish(int maxRoundsCount)
        {
            if (RoundsCount < maxRoundsCount || IsFinished)
                return false;
            
            IsFinished = true;

            var playerController = GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Stop();
            }

            return true;
        }
    }
}