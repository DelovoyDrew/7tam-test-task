using System.Threading;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] private MatchmakingService _matchmakingService;
    [SerializeField] private LobbyScreenUI _lobbyUI = new LobbyScreenUI();

    private const string NULL_NAME = "Name cant be null";

    public void GetName()
    {
        var name = _lobbyUI.GetPlayerName();
        if (IsNameValid(name, NULL_NAME))
        {
            MatchmakingService.Instance.GetName(name);
            _lobbyUI.CloseGetNamePopup();
            _lobbyUI.OpenLobbyNamePopup();
        }
    }

    public async void CreateLobby(Button createLobbyButton)
    {
        var lobbyName = _lobbyUI.GetLobbyName();

        if (IsNameValid(lobbyName, NULL_NAME))
        {
            createLobbyButton.gameObject.SetActive(false);
            await _matchmakingService.CreateLobby(lobbyName);

            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.SceneManager.LoadScene(Scenes.GetNameByType(Scenes.Types.Game), UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }
    }

    public async void JoinLobby(Button createLobbyButton)
    {
        var lobbyName = _lobbyUI.GetLobbyName();

        if (IsNameValid(lobbyName, NULL_NAME))
        {
            createLobbyButton.gameObject.SetActive(false);
            await _matchmakingService.TryJoinLobby(lobbyName);
        }
    }

    private bool IsNameValid(string value, string errorMessage)
    {
        if (value == null || value.Length == 0)
        {
            Debug.LogError(errorMessage);
            return false;
        }

        return true;
    }
}
