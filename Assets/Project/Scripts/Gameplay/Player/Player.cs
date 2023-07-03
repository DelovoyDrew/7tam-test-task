using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour, IDamageable, ICoinCollectable
{
    public bool IsAlive => _health.Value > 0;
    public uint Coins => _coins.Value;
    public string Name => _name.Value;

    public Action OnDie;

    [SerializeField] private float _maxHP;
    [SerializeField] private HpBar _hpBar;

    [SerializeField] private PlayerInput _input;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerShooting _shooting;

    [SerializeField] private TextMeshProUGUI _nameText;

    private NetworkVariable<NetworkString> _name = new NetworkVariable<NetworkString>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<float> _health = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<uint> _coins = new NetworkVariable<uint>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public void InitializePlayer(PlayerData data)
    {
        if (IsOwner)
        {
            _name.Value = MatchmakingService.Instance.Name;
            _input.Initialize(data.UI.Joystick, data.UI.ShootButton);
            _input.enabled = true;
            _movement.enabled = true;
            _input.OnShoot += () =>
            {
                _shooting.ShootServerRpc(_shooting.ShootPosition, _movement.MoveDirection);
            };
            data.UI.HpBar.Initialize(_maxHP, _health);
            data.UI.CoinsGameUI.Initialize(_coins);
        }
        else
        {
            _hpBar.Initialize(_maxHP, _health);
        }

        if(NetworkManager.Singleton.IsHost)
        {
            _health.Value = _maxHP;
            _health.OnValueChanged += TryDie;
        }

        _nameText.text = _name.Value;
    }

    [ClientRpc]
    public void SetPlayerPositionClientRpc(Vector3 position)
    {
        transform.position = position;
    }

    public void GetName(string name)
    {
        _name.Value = name;
        _nameText.text = _name.Value;
    }

    [ClientRpc]
    public void EnableInputClientRpc(bool isEnabled)
    {
        if (IsOwner)
            _input.Enable(isEnabled);
    }

    public void CollectCoin(Coin coin)
    {
        _coins.Value += coin.NominalValue;
    }

    public void TakeDamage(float damage)
    {
        _health.Value -= damage;
    }

    private void TryDie(float previousValue, float value)
    {
        if (_health.Value > 0)
            return;

        _health.Value = 0;
        Die();
    }

    private void Die()
    {
        OnDie?.Invoke();
        DieClientRpc();
    }

    [ClientRpc]
    private void DieClientRpc()
    {
        gameObject.gameObject.SetActive(false);
        _input.Enable(false);
    }

    public void FinishGame()
    {
        _input.Enable(false);
    }
}

