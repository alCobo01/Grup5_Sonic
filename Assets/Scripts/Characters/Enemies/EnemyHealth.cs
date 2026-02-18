using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class EnemyHealth : MonoBehaviour, IDamageable
{
    private HealthBehaviour _health;

    private void Awake()
    {
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
        //animations or vfx
        Destroy(gameObject);
    }
}
