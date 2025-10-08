using System.Collections;
using Global;
using UnityEngine;

namespace Leaderboard
{
    public class AllPlayersNameInput
    {
        public readonly struct PlayerData
        {
            public readonly string Name;
            public readonly float Time;

            public PlayerData(string name, float time)
            {
                Name = name;
                Time = time;
            }
        }

        private string _name;

        public PlayerData[] Data { get; private set; }

        public IEnumerator WaitForAllInputRoutine(PlayerNameInput nameInput, StaticParameters.PlayerFinishData[] finishData)
        {
            if (finishData == null)
                yield break;
            
            var result = new PlayerData[finishData.Length];
            _name = string.Empty;
            nameInput.Submitted += OnNameSubmitted;

            for (int i = 0; i < finishData.Length; i++)
            {
                _name = string.Empty;
                nameInput.ShowInputField(i);

                yield return new WaitWhile(() => string.IsNullOrEmpty(_name));
                
                result[i] = new(_name, finishData[i].Time);
            }
            
            nameInput.Submitted -= OnNameSubmitted;
            Data = result;
        }

        private void OnNameSubmitted(string name)
        {
            _name = name;
        }
    }
}