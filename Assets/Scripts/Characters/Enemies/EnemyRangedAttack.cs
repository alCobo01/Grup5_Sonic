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
    [SerializeField] private string tagTarget = "Player";
    
    private float _attackRangeSqr;
    private WaitForSeconds _cooldownWait;
    private Transform _target;
    private EnemyHealth _enemyHealth;
    private AnimationBehaviour _animationBehaviour;

    private void Awake()
    {
        if (firePoint == null) firePoint = transform;
        _attackRangeSqr = attackRange * attackRange;
        _cooldownWait = new WaitForSeconds(attackCooldown);
        _enemyHealth = GetComponent<EnemyHealth>();
        _animationBehaviour = GetComponent<AnimationBehaviour>();
    }

    private void Start() => _target = GameObject.FindGameObjectWithTag(tagTarget).transform;
    
    private void OnEnable()
    {
        if (_enemyHealth != null) _enemyHealth.OnDeath += HandleDeath;
        StartCoroutine(WaitForInitialize());
    }

    private void OnDisable()
    {
        if (_enemyHealth != null) _enemyHealth.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        StopAllCoroutines();
        enabled = false;
    }

    private IEnumerator WaitForInitialize()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(AttackLoop());
    }
    
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
        (_target.position - firePoint.position).sqrMagnitude <= _attackRangeSqr;

    private void Shoot()
    {
        AudioManager.Instance.PlayEnemyShot(transform);
        _animationBehaviour.Trigger(Animator.StringToHash("Attack"));
        var direction = (_target.position - firePoint.position).normalized;
        var proj = Instantiate(projectilePrefab, firePoint.position, 
            Quaternion.LookRotation(direction));

        if (proj.TryGetComponent(out Rigidbody rb))
            rb.linearVelocity = direction * projectileSpeed;

        if (proj.TryGetComponent(out DamageProjectile dp))
            dp.Configure(projectileDamage, targetLayer, gameObject);
    }
}