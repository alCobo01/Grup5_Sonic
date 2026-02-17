using UnityEngine;

[RequireComponent(typeof(AnimationBehaviour))]
public class RangeAttack : MonoBehaviour, IAttack
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private LayerMask targetLayer = ~0;

    private AnimationBehaviour _animationBehaviour;

    private void Awake()
    {
        _animationBehaviour = GetComponent<AnimationBehaviour>();
        if (firePoint == null) firePoint = transform;
    }

    public void Attack()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            var rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
                rb.linearVelocity = firePoint.forward * bulletSpeed;

            if (bullet.TryGetComponent(out DamageProjectile projectile))
                projectile.Configure(damageAmount, targetLayer, gameObject);
        }
        
        _animationBehaviour.TriggerRangeAttack();
    }
}
