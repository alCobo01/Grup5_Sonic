using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(HealthBehaviour))]
[RequireComponent(typeof(AnimationBehaviour))]
public class EnemyHealth : MonoBehaviour, IDamageable
{
    public static event UnityAction OnDeathStat;
    public event UnityAction OnDeath;
    public event UnityAction OnDamaged;
    
    private static readonly int DieHash = Animator.StringToHash("Die");
    
    [SerializeField] private GameObject dieVFX;
    [SerializeField] private float destroyDelay = -1f;
    
    private AnimationBehaviour _animationBehaviour;
    private HealthBehaviour _health;

    public bool IsDying { get; private set; }
    private bool _isBeingDestroyed;

    private void Awake()
    {
        _animationBehaviour = GetComponent<AnimationBehaviour>();
        _health = GetComponent<HealthBehaviour>();
    }

    private void OnDestroy() => _isBeingDestroyed = true;

    public bool IsInvulnerable { get; set; }
    
    public void TakeDamage(int damage)
    {
        if (IsInvulnerable) return;
        if (_health.IsDead) return;

        damage = Mathf.Abs(damage);
        if (damage == 0) return;

        for (var i = 0; i < damage; i++) _health.LoseLife();
        
        OnDamaged?.Invoke();
        
        if (_health.CurrentLives <= 0) Die();
    }

    public void InstantKill()
    {
        if (_health.IsDead) return;
        Die();
    }
    
    public void Die()
    {
        if (IsDying) return;
        IsDying = true;

        if (_isBeingDestroyed || !gameObject.scene.isLoaded) return;
        
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
