using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameNetworkManager : NetworkBehaviour
{
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
        bool isWinnerExist = false;

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
        foreach (var player in players)
        {
            if(!_players.Contains(player))
            {
                player.InitializePlayer(_data);
                player.OnDie += PlayerDie;
                _players.Add(player);
            }
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