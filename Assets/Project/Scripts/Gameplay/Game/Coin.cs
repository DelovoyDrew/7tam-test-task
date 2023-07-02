using Unity.Netcode;
using UnityEngine;
using TMPro;

public class Coin : NetworkBehaviour
{
    [field: SerializeField] public uint NominalValue { get; private set; }

    [SerializeField] private TextMeshPro _nominalValueText;

    private void Awake()
    {
        _nominalValueText.text = NominalValue.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsHost)
            return;

        if (collision.transform.gameObject.TryGetComponent(out ICoinCollectable coinCollectable))
        {
            coinCollectable.CollectCoin(this);
            GetComponent<NetworkObject>().Despawn();
        }
           
    }
}
