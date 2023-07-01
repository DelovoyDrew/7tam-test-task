using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private PlayerMovement _movement;

    public void InitializePlayer(PlayerData data)
    {
        if (IsOwner)
        {
            _input.enabled = true;
            _input.Initialize(data.UI.Joystick);
            _movement.enabled = true;
        }
    }
}
