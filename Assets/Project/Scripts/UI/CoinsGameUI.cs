using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CoinsGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsCount;

    public void Initialize(NetworkVariable<uint> coins)
    {
        coins.OnValueChanged += UpdateCount;
    }

    private void UpdateCount(uint previous, uint current)
    {
        _coinsCount.text = current.ToString();
    }
}
