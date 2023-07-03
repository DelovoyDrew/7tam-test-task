using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameNetworkManager : NetworkBehaviour
{
    [SerializeField] private List<Transform> _spawns;

    [SerializeField] private PlayerData _data = new PlayerData();

    private List<Player> _players = new List<Player>();

    private void Start()
    {
        InitializePlayers();
        TryInitializePlayersServerRpc();
    }

    public void PlayerDie()
    {
        int alivePlayers = 0;
        Player playerWinner = null;

        foreach (var player in _players)
        {
            if (player.IsAlive)
            {
                alivePlayers++;
                playerWinner = player;
            }
        }

        if (alivePlayers <= 1)
        {
            FinishGameClientRpc(playerWinner?.Name, playerWinner?.Coins.ToString());
        }
    }

    [ClientRpc]
    private void FinishGameClientRpc(string winnerName, string coins)
    {
        _data.UI.FinishScreen.Activate(winnerName, coins);

        foreach (var player in _players)
        {         
            if (player.IsOwner)
                player.FinishGame();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void TryInitializePlayersServerRpc()
    {
        var players = FindObjectsOfType<Player>();
        bool canStartGame = players.Length >= 2;

        foreach (var player in players)
        {
            if(!_players.Contains(player))
            {
                player.InitializePlayer(_data);
                player.SetPlayerPositionClientRpc(_spawns[players.Length - 1].transform.position);
                player.OnDie += PlayerDie;
                _players.Add(player);
            }
            player.GetName(player.Name);
            player.EnableInputClientRpc(canStartGame);
        }
    }

    private void InitializePlayers()
    {
        var players = FindObjectsOfType<Player>();
        foreach (var player in players)
        {
            player.InitializePlayer(_data);
        }
    }
}

[System.Serializable]
public class PlayerData
{
    [field: SerializeField] public GameUI UI { get; private set; }
}