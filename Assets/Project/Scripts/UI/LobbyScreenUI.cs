using UnityEngine;
using TMPro;

[System.Serializable]
public class LobbyScreenUI
{
    [SerializeField] private RectTransform _getPlayerNamePopup;
    [SerializeField] private RectTransform _getLobbyNamePopup;

    [field: SerializeField] private TMP_InputField _playerNameInput;
    [field: SerializeField] private TMP_InputField _lobbyNameInput;

    public void CloseGetNamePopup()
    {
        _getPlayerNamePopup.gameObject.SetActive(false);
    }

    public void OpenLobbyNamePopup()
    {
        _getLobbyNamePopup.gameObject.SetActive(true);
    }

    public string GetPlayerName()
    {
        return _playerNameInput.text;
    }

    public string GetLobbyName()
    {
        return _lobbyNameInput.text;
    }
}
