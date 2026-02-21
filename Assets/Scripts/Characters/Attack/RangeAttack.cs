using UnityEngine;

[RequireComponent(typeof(AnimationBehaviour))]
public class RangeAttack : MonoBehaviour, IAttack
{
    private static readonly int RangeAttackHash = Animator.StringToHash("RangeAttack");
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private LayerMask targetLayer = ~0;

    private AnimationBehaviour _animationBehaviour;
    private IRingWallet _ringWallet;

    private void Awake()
    {
        _animationBehaviour = GetComponent<AnimationBehaviour>();
        if (firePoint == null) firePoint = transform;
        _ringWallet = GetComponentInParent<IRingWallet>();
    }

    public void Attack()
    {
        if (!_ringWallet.TrySpendRing()) return;
        
        if (bulletPrefab != null && firePoint != null)
        {
            var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            var rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
                rb.linearVelocity = firePoint.forward * bulletSpeed;

            if (bullet.TryGetComponent(out DamageProjectile projectile))
                projectile.Configure(damageAmount, targetLayer, gameObject);
        }
        
        _animationBehaviour.Trigger(RangeAttackHash);
    }
}
