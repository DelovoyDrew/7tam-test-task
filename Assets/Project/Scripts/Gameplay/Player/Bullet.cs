using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class Bullet : NetworkBehaviour, IAttackable
{
    [SerializeField] private float _damage;
    [SerializeField] private Rigidbody2D _rigidbody;

    private const float SELF_DESTROY_DELAY = 3;

    private bool _isInitialized = false;
    private bool _isAttacked = false;

    private NetworkObject _networkObject;

    public void Initialize(float force, Vector3 direction)
    {
        _isInitialized = true;
        _rigidbody.AddForce(direction * force, ForceMode2D.Impulse);

        _networkObject = GetComponent<NetworkObject>();
        StartCoroutine(AutoDestroy());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isInitialized || _isAttacked)
            return;

        if (collision.transform.gameObject.TryGetComponent(out IDamageable damageable))
        {
            Attack(damageable);
            _isAttacked = true;
            _networkObject.Despawn();
        }
    }

    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(SELF_DESTROY_DELAY);  
        _networkObject.Despawn();
    }

    public void Attack(IDamageable damageable)
    {
        Debug.Log("Bullet damage");
        damageable.TakeDamage(_damage);
    }
}
