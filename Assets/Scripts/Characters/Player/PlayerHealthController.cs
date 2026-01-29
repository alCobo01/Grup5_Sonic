using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class PlayerHealthController : MonoBehaviour, IDamageable
{
    public bool IsInvincible { get; set; }

    private int _currentShield;
    private HealthBehaviour _health;

    private void Awake()
    {
        _health = GetComponent<HealthBehaviour>();
    }

    private void OnEnable() => _health.OnDeath += HandleDeath;
    private void OnDisable() => _health.OnDeath -= HandleDeath;

    // Modify shield methods
    public void AddShield(int amount) => _currentShield += amount;
    public void RemoveShield(int amount) => _currentShield = Mathf.Max(0, _currentShield - amount);
    
    // Modify health methods
    public void AddHealth(int boost) => _health.ModifyHealth(boost);

    public void TakeDamage(int damage)
    {
        if (IsInvincible) return;

        // To compare with shield, we have to have positive values
        damage = Mathf.Abs(damage);
        if (damage <= _currentShield) _currentShield -= damage;
        else if (damage > _currentShield)
        {
            damage -= _currentShield;
            _currentShield = 0;
            _health.ModifyHealth(-damage);
        }
        else _health.ModifyHealth(-damage);
    }
    public void InstantKill() => _health.Kill();
    
    private void HandleDeath()
    {
        Debug.Log("Player Died");
        // game manager call
    }
}