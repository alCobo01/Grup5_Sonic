using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AnimationBehaviour))]
public class MeleeAttack : MonoBehaviour, IAttack
{
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackRadius = 0.75f;
    [SerializeField] private LayerMask targetLayer = ~0;
    [SerializeField] private Transform attackPoint;

    private AnimationBehaviour _animationBehaviour;

    private void Awake()
    {
        _animationBehaviour = GetComponent<AnimationBehaviour>();
    }

    public void Attack()
    {
        _animationBehaviour.TriggerMeleeAttack();
        DealDamage();
    }

    private void DealDamage()
    {
        Transform pivot = attackPoint != null ? attackPoint : transform;
        Vector3 origin = pivot.position + pivot.forward * attackRange;
        Collider[] hitColliders = Physics.OverlapSphere(origin, attackRadius, targetLayer, QueryTriggerInteraction.Collide);
        HashSet<IDamageable> damagedTargets = new HashSet<IDamageable>();

        foreach (Collider hit in hitColliders)
        {
            IDamageable damageable = hit.GetComponentInParent<IDamageable>();
            if (damageable == null || damagedTargets.Contains(damageable)) continue;

            damageable.TakeDamage(damageAmount);
            damagedTargets.Add(damageable);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Transform pivot = attackPoint != null ? attackPoint : transform;
        Vector3 origin = pivot.position + pivot.forward * attackRange;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, attackRadius);
    }
}
