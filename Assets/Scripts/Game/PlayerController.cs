using System;
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        public int PlayerIndex = 1;

        public event Action Pressing, UnPressed;

        public void SetEnabled(bool enabled)
        {
            this.enabled = enabled;
        }

        private void Update()
        {
            var code = GetPlayerCodes(PlayerIndex);
            if(Input.GetKey(code))
                Pressing?.Invoke();
            else if(Input.GetKeyUp(code))
                UnPressed?.Invoke();
        }

        public static KeyCode GetPlayerCodes(int playerIndex) => playerIndex switch
        {
            1 => GetPlayer1Codes(),
            2 => GetPlayer2Codes(),
            3 => GetPlayer3Codes(),
            4 => GetPlayer4Codes(),
            _ => throw new IndexOutOfRangeException()
        };

        public static KeyCode GetPlayer1Codes() => KeyCode.W;

        public static KeyCode GetPlayer2Codes() => KeyCode.UpArrow;

        public static KeyCode GetPlayer3Codes() => KeyCode.P;

        public static KeyCode GetPlayer4Codes() => KeyCode.Y;
    }
}