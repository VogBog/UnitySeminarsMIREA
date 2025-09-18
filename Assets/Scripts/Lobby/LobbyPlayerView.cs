using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class LobbyPlayerView : MonoBehaviour
    {
        [SerializeField] private Image _elementalImage;
        [SerializeField] private TMP_Text _elementalName;

        [SerializeField] private TMP_Text _activePlayerTip;

        public const string FirstPlayerTip = "E or Right button";
        public const string SecondPlayerTip = "RShift or Right button";
        public const string OtherPlayerTip = "Right button (gamepad)";

        public void SetData(PlayerData playerData, bool isActivePlayer)
        {
            gameObject.SetActive(true);
            _elementalImage.color = playerData.Data.Color;
            _elementalName.text = playerData.Data.Name;
            
            _activePlayerTip.gameObject.SetActive(isActivePlayer);

            _activePlayerTip.text = playerData.Index switch
            {
                1 => FirstPlayerTip,
                2 => SecondPlayerTip,
                _ => OtherPlayerTip
            };
        }

        public void SetInactive()
        {
            gameObject.SetActive(false);
        }
    }
}