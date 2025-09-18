using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    [Serializable]
    public class LobbyView
    {
        [SerializeField] private LobbyPlayerView[] _players;
        [SerializeField] private Button _playBtn;

        public event Action PlayBtnClicked; 
        
        public void Initialize(Lobby lobby)
        {
            _playBtn.gameObject.SetActive(false);
            
            lobby.ActivePlayersChanged += UpdatePlayers;
            _playBtn.onClick.AddListener(OnPlayBtnClicked);
            
            UpdatePlayers(Array.Empty<PlayerData>());
        }

        private void UpdatePlayers(PlayerData[] players)
        {
            int i;
            for (i = 0; i < players.Length; i++)
            {
                _players[i].SetData(players[i], true);
            }

            for (; i < _players.Length; i++)
            {
                _players[i].SetInactive();
            }
            
            _playBtn.gameObject.SetActive(players.Length > 1);
        }

        private void OnPlayBtnClicked()
        {
            PlayBtnClicked?.Invoke();
        }
    }
}