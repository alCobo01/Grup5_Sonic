using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
[RequireComponent(typeof(AnimationBehaviour))]
public class EnemyHealth : MonoBehaviour, IDamageable
{
    private static readonly int DieHash = Animator.StringToHash("Die");
    
    private AnimationBehaviour _animationBehaviour;
    private HealthBehaviour _health;

    private void Awake()
    {
        _animationBehaviour = GetComponent<AnimationBehaviour>();
        _health = GetComponent<HealthBehaviour>();
    }

    public void TakeDamage(int damage)
    {
        if (_health.IsDead) return;

        damage = Mathf.Abs(damage);
        if (damage == 0) return;

        for (var i = 0; i < damage; i++) _health.LoseLife();
        if (_health.CurrentLives <= 0) Die();
    }

    public void InstantKill()
    {
        if (_health.IsDead) return;
        Die();
    }
    
    private void Die()
    {
        _animationBehaviour.Trigger(DieHash);
    }

    public void Destroy() => Destroy(gameObject);
    
}
