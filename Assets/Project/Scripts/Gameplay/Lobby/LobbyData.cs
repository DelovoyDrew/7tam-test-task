using Unity.Services.Lobbies.Models;
using UnityEngine;

[System.Serializable]
public class LobbyData 
{
    [field: SerializeField] public string JoinCodeKey { get; private set; }
    [field: SerializeField] public int MaxPlayersCount { get; private set; }
    [field: SerializeField] public DataObject.VisibilityOptions Visibility { get; private set; }
}
