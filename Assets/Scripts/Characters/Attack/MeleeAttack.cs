using UnityEngine;
using System.Collections.Generic;

//[RequireComponent(typeof(AnimationBehaviour))]
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
        if (_animationBehaviour == null)
        {
            _animationBehaviour = GetComponentInParent<AnimationBehaviour>();
        }
    }

    public void Attack()
    {
        _animationBehaviour.TriggerMeleeAttack();
        DealDamage();
    }

    private void DealDamage()
    {
        var pivot = attackPoint != null ? attackPoint : transform;
        var origin = pivot.position + pivot.forward * attackRange;
        var hitColliders = Physics.OverlapSphere(origin, attackRadius, targetLayer, QueryTriggerInteraction.Collide);
        var damagedTargets = new HashSet<IDamageable>();

        foreach (var hit in hitColliders)
        {
            var damageable = hit.GetComponentInParent<IDamageable>();
            if (damageable == null || damagedTargets.Contains(damageable)) continue;

            damageable.TakeDamage(damageAmount);
            damagedTargets.Add(damageable);
        }
    }
}
