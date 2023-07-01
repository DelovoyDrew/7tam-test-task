using Unity.Netcode;
using UnityEngine;

public class GameNetworkManager : NetworkBehaviour
{
    [SerializeField] private PlayerData _data = new PlayerData();

    private void Start()
    {
        var players = FindObjectsOfType<Player>();
        foreach (var player in players) 
        {
            if(player.IsOwner)
            {
                player.InitializePlayer(_data);
                break;
            }
        }
    }
}

[System.Serializable]
public class PlayerData
{
    [field: SerializeField] public GameUI UI { get; private set; }
}