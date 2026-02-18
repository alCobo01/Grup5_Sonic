using System.Collections;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private int projectileDamage = 1;
    [SerializeField] private LayerMask targetLayer = ~0;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float attackCooldown = 1.25f;
    [SerializeField] private Transform target;
    
    private float _attackRangeSqr;
    private WaitForSeconds _cooldownWait;

    private void Awake()
    {
        if (firePoint == null) firePoint = transform;
        _attackRangeSqr = attackRange * attackRange;
        _cooldownWait = new WaitForSeconds(attackCooldown);
    }

    private void OnEnable() => StartCoroutine(AttackLoop());

    private IEnumerator AttackLoop()
    {
        while (enabled)
        {
            if (IsInRange())
            {
                Shoot();
                yield return _cooldownWait;
            }
            else yield return null;
        }
    }

    private bool IsInRange() => 
        (target.position - firePoint.position).sqrMagnitude <= _attackRangeSqr;

    private void Shoot()
    {
        var direction = (target.position - firePoint.position).normalized;
        var proj = Instantiate(projectilePrefab, firePoint.position, 
            Quaternion.LookRotation(direction));

        if (proj.TryGetComponent(out Rigidbody rb))
            rb.linearVelocity = direction * projectileSpeed;

        if (proj.TryGetComponent(out DamageProjectile dp))
            dp.Configure(projectileDamage, targetLayer, gameObject);
    }
}