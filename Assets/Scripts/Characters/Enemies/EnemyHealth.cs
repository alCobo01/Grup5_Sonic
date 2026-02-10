using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class EnemyHealth : MonoBehaviour, IDamageable
{
    private HealthBehaviour _health;
    public void TakeDamage(int damage)
    {
        _health.LoseLife();
        if (_health.CurrentLives <= 0) Die();
    }

    public void InstantKill() => Die();
    
    private void Die()
    {
        //animations or vfx
        Destroy(gameObject);
    }
}