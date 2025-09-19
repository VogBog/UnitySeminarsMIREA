using System;
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        public int PlayerIndex = 1;
        
        public event Action<Vector2> Move;

        public void WaitForStart(GameUI ui)
        {
            enabled = false;
            ui.RaceStarted += () => enabled = true;
        }

        public void Stop()
        {
            enabled = false;
        }

        private void FixedUpdate()
        {
            var moveAxis = GetMoveAxis();
            Move?.Invoke(moveAxis);
        }

        private Vector2 GetMoveAxis()
        {
            var (up, left, down, right) = GetPlayerCodes(PlayerIndex);
            float horizontal = 0f;
            float vertical = 0f;

            if (Input.GetKey(up)) vertical += 1f;
            if(Input.GetKey(down)) vertical -= 1f;
            if(Input.GetKey(left)) horizontal -= 1f;
            if(Input.GetKey(right)) horizontal += 1f;
            
            var vector = new Vector2(horizontal, vertical);

            return vector;
        }

        public static (KeyCode, KeyCode, KeyCode, KeyCode) GetPlayerCodes(int playerIndex) => playerIndex switch
        {
            1 => GetPlayer1Codes(),
            2 => GetPlayer2Codes(),
            3 => GetPlayer3Codes(),
            4 => GetPlayer4Codes(),
            _ => throw new IndexOutOfRangeException()
        };
        
        public static (KeyCode, KeyCode, KeyCode, KeyCode) GetPlayer1Codes()
            => (KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D);
        
        public static (KeyCode, KeyCode, KeyCode, KeyCode) GetPlayer2Codes()
            => (KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow);
        
        public static (KeyCode, KeyCode, KeyCode, KeyCode) GetPlayer3Codes()
            => (KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L);
        
        public static (KeyCode, KeyCode, KeyCode, KeyCode) GetPlayer4Codes()
            => (KeyCode.T, KeyCode.F, KeyCode.G, KeyCode.H);
    }
}