using UnityEngine;

[RequireComponent(typeof(AnimationBehaviour))]
public class ContactDamage : MonoBehaviour
{
  
    
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private LayerMask targetLayer;

    private AnimationBehaviour _animationBehaviour;

    private void Awake() => _animationBehaviour = GetComponent<AnimationBehaviour>();

    private void OnTriggerEnter(Collider other) => TryDealDamage(other.gameObject);
    private void OnCollisionEnter(Collision other) => TryDealDamage(other.gameObject);

    private void TryDealDamage(GameObject target)
    {
        if (((1 << target.layer) & targetLayer) == 0) return;
        
        var damageable = target.GetComponentInParent<IDamageable>();
        damageable?.TakeDamage(damageAmount);
        
        _animationBehaviour.TriggerMeleeAttack();
    }
}
