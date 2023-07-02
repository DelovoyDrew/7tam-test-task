using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using UnityEngine;
using Unity.Netcode;
using System.Collections;
using Unity.Services.Authentication;

public class MatchmakingService : Singleton<MatchmakingService>
{
    [SerializeField] private UnityTransport _transport;
    [SerializeField] private LobbyData _lobbyData;

    private static Lobby _connectedLobby;
    public string Name{ get; private set; }    

    private const int HEARTBIT_DELAY = 15;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if(_connectedLobby != null)
            Debug.Log(_connectedLobby.Id);
    }

    public void GetName(string name)
    {
        Name = name;
    }

    public async Task TryJoinLobby(string lobbyName)
    {
        var lobby = await TryFindLobbyByName(lobbyName);

        if (lobby == null)
        {
            Debug.Log("Lobby not found");
            return;
        }

        try
        {
            var allocation = await RelayService.Instance.JoinAllocationAsync(lobby.Data[_lobbyData.JoinCodeKey].Value);
            SetTransformAsClient(allocation);
            NetworkManager.Singleton.StartClient();
            _connectedLobby = lobby;
        }
        catch { }
    }

    public async Task<Lobby> CreateLobby(string lobbyName)
    {
        Lobby lobby = await TryFindLobbyByName(lobbyName);

        if (lobby != null)
        {
            Debug.Log("Lobby with this name already exist");
            return null;
        }

        try
        {
            var allocation = await RelayService.Instance.CreateAllocationAsync(_lobbyData.MaxPlayersCount);
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            var options = new CreateLobbyOptions
            {
                Data = new Dictionary<string, DataObject> { { _lobbyData.JoinCodeKey, new DataObject(_lobbyData.Visibility, joinCode) } }
            };

            lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, _lobbyData.MaxPlayersCount, options);

            StartCoroutine(HeartBitLobbyCoroutine(lobby.Id));

            _transport.SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);
            NetworkManager.Singleton.StartHost();
            _connectedLobby = lobby;
            return lobby;
        }
        catch
        {
            return null;
        }

    }

    private async Task<Lobby> TryFindLobbyByName(string lobbyName)
    {
        Lobby targetLobby = null;

        var lobbies = await Lobbies.Instance.QueryLobbiesAsync();

        foreach (var lobby in lobbies.Results)
        {
            if (lobby.Name == lobbyName)
            {
                targetLobby = lobby;
                break;
            }
        }

        return targetLobby;
    }

    private static IEnumerator HeartBitLobbyCoroutine(string lobbyId)
    {
        var delay = new WaitForSecondsRealtime(HEARTBIT_DELAY);

        while (_connectedLobby != null)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }

    private void SetTransformAsClient(Unity.Services.Relay.Models.JoinAllocation allocation)
    {
        _transport.SetClientRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);
    }

    public void Leave()
    {
        StopAllCoroutines();
        if (_connectedLobby != null)
        {
            var playerId = AuthenticationService.Instance.PlayerId;
            if (_connectedLobby.HostId == playerId)
            {
                Lobbies.Instance.DeleteLobbyAsync(_connectedLobby.Id);

            }
            else
            {
                Lobbies.Instance.RemovePlayerAsync(_connectedLobby.Id, playerId);
            }
            _connectedLobby = null;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
