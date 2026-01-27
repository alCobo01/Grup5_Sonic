using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class PlayerHealthController : MonoBehaviour, IDamageable
{
    private HealthBehaviour _health;

    private void Awake()
    {
        _health = GetComponent<HealthBehaviour>();
    }

    private void OnEnable() => _health.OnDeath += HandleDeath;
    
    private void OnDisable() => _health.OnDeath -= HandleDeath;
    
    public void TakeDamage(int damage) => _health.ModifyHealth(-Mathf.Abs(damage));
    
    public void InstantKill() => _health.Kill();
    
    private void HandleDeath()
    {
        Debug.Log("Player Died");
        // game manager call
    }
}