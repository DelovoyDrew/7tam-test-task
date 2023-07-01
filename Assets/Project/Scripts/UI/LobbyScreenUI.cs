using UnityEngine;
using TMPro;

[System.Serializable]
public class LobbyScreenUI
{
    [field: SerializeField] private TMP_InputField _playerNameInput;
    [field: SerializeField] private TMP_InputField _lobbyNameInput;

    public string GetPlayerName()
    {
        return _playerNameInput.text;
    }

    public string GetLobbyName()
    {
        return _lobbyNameInput.text;
    }
}
