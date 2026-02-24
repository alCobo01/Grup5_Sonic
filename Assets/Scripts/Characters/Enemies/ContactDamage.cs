using UnityEngine;

[RequireComponent(typeof(AnimationBehaviour))]
public class ContactDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private LayerMask targetLayer;

    private AnimationBehaviour _animationBehaviour;
    private float _elapsedTime;

    private void Awake() => _animationBehaviour = GetComponent<AnimationBehaviour>();
    private void Update() => _elapsedTime += Time.deltaTime;

    private void OnTriggerEnter(Collider other) => TryDealDamage(other.gameObject);
    private void OnCollisionEnter(Collision other) => TryDealDamage(other.gameObject);

    private void TryDealDamage(GameObject target)
    {
        if (((1 << target.layer) & targetLayer) == 0) return;
        if (_elapsedTime < attackCooldown) return;
        
        var damageable = target.GetComponentInParent<IDamageable>();
        damageable?.TakeDamage(damageAmount);
        
        _animationBehaviour.TriggerMeleeAttack();
        _elapsedTime = 0;
    }
}
