using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HealthBehaviour))]
[RequireComponent(typeof(AnimationBehaviour))]
public class EnemyHealth : MonoBehaviour, IDamageable
{
    public static event UnityAction OnDeathStat;
    public event UnityAction OnDeath;
    
    private static readonly int DieHash = Animator.StringToHash("Die");
    
    [SerializeField] private GameObject dieVFX;
    [SerializeField] private float destroyDelay = -1f;
    
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
        AudioManager.Instance.PlayEnemyDeath(transform);
        _animationBehaviour.Trigger(DieHash);
        OnDeath?.Invoke();
        OnDeathStat?.Invoke();
        
        var instantiedVFX = Instantiate(dieVFX, transform.position, transform.rotation);
        Destroy(instantiedVFX, 2f);

        if (destroyDelay >= 0f)
        {
            Invoke(nameof(Destroy), destroyDelay);
        }
    }

    public void Destroy() => Destroy(gameObject);
}
