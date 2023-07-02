using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    public Vector3 ShootPosition => _shooterData.ShootPoint.position;

    [field: SerializeField] private ShooterData _shooterData = new ShooterData();

    [ServerRpc]
    public void ShootServerRpc(Vector3 position, Vector3 direction)
    {
        var bullet = Instantiate(_shooterData.Bullet, position, Quaternion.Euler(direction));
        bullet.GetComponent<NetworkObject>().Spawn();
        bullet.Initialize(_shooterData.ShootForce, direction);
    }
}

[System.Serializable]
public class ShooterData
{
    [field: SerializeField] public Transform ShootPoint { get; private set;}
    [field: SerializeField] public float ShootForce { get; private set; }
    [field: SerializeField] public Bullet Bullet { get; private set; }

}