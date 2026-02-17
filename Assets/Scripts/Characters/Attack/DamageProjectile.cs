using UnityEngine;

public class DamageProjectile : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private LayerMask targetLayer = ~0;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private bool destroyOnHit = true;

    private GameObject _owner;
    private bool _hasDealtDamage;

    private void Awake()
    {
        if (lifeTime > 0f)
            Destroy(gameObject, lifeTime);
    }

    public void Configure(int damage, LayerMask layerMask, GameObject owner)
    {
        damageAmount = Mathf.Abs(damage);
        targetLayer = layerMask;
        _owner = owner;
    }

    private void OnTriggerEnter(Collider other) => TryDealDamage(other.gameObject);
    private void OnCollisionEnter(Collision other) => TryDealDamage(other.gameObject);

    private void TryDealDamage(GameObject target)
    {
        if (_hasDealtDamage) return;
        if (_owner != null && (target == _owner || target.transform.IsChildOf(_owner.transform))) return;
        if (((1 << target.layer) & targetLayer) == 0) return;

        var damageable = target.GetComponentInParent<IDamageable>();
        if (damageable == null) return;

        damageable.TakeDamage(damageAmount);
        _hasDealtDamage = true;

        if (destroyOnHit)
            Destroy(gameObject);
    }
}
