using System;
using System.Text;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Chat : NetworkBehaviour
{
    [SerializeField] private GameObject _chatUI;
    [SerializeField] private TMP_Text _chatText;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _sendButton;
    
    private readonly StringBuilder _messages = new();
    private string _ownerName = "Unknown";

    private void Start()
    {
        _sendButton.onClick.AddListener(OnSendButtonPressed);
    }

    public void SetOwnerName(string ownerName)
    {
        string[] colors = new string[] { "red", "purple", "blue", "orange" };
        int index = Random.Range(0, colors.Length);
        string color = colors[index];
        _ownerName = $"<color={color}>{ownerName}</color>";
    }

    public void SetUIActive(bool isActive)
    {
        _chatUI.SetActive(isActive);
    }

    public bool ChangeUIActive()
    {
        _chatUI.SetActive(!_chatUI.activeSelf);
        return _chatUI.activeSelf;
    }

    private void OnSendButtonPressed()
    {
        SendChatMessageServerRpc(_ownerName, _inputField.text);
        _inputField.text = "";
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendChatMessageServerRpc(string owner, string message)
    {
        string totalMess = $"{owner}: {message}";
        SendChatMessageClientRpc(totalMess);
    }

    [ClientRpc]
    private void SendChatMessageClientRpc(string message)
    {
        _messages.AppendLine(message);
        if (_messages.Length > 5000)
        {
            _messages.Remove(0, _messages.Length - 5000);
        }
        _chatText.text = _messages.ToString();
    }
}
