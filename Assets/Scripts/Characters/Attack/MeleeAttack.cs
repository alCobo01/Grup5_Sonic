using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(AnimationBehaviour))]
public class MeleeAttack : MonoBehaviour, IAttack
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private int damageAmount;
    
    private AnimationBehaviour _animationBehaviour;

    private void Awake()
    {
        _animationBehaviour = GetComponent<AnimationBehaviour>();
    }

    public void Attack()
    {
        _animationBehaviour.TriggerMeleeAttack();
        var hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (var enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damageAmount);
            }
        }
    }
}
