using Unity.Netcode;
using UnityEngine;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] private MatchmakingService _matchmakingService;
    [SerializeField] private LobbyScreenUI _lobbyUI = new LobbyScreenUI();

    private const string NULL_NAME = "Name cant be null";

    public async void CreateLobby()
    {
        var lobbyName = _lobbyUI.GetLobbyName();

        if (IsNameValid(lobbyName, NULL_NAME))
        {
            await _matchmakingService.CreateLobby(lobbyName);
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.SceneManager.LoadScene(Scenes.GetNameByType(Scenes.Types.Game), UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }
    }

    public async void JoinLobby()
    {
        var lobbyName = _lobbyUI.GetLobbyName();

        if (IsNameValid(lobbyName, NULL_NAME))
            await _matchmakingService.TryJoinLobby(lobbyName);
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
