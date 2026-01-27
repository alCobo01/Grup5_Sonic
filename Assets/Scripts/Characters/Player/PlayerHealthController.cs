using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class PlayerHealthController : MonoBehaviour, IDamageable
{
    [Header("Shield values")]
    [SerializeField] private int maxShield = 50;

    private int _currentShield;
    private HealthBehaviour _health;

    private void Awake()
    {
        _health = GetComponent<HealthBehaviour>();
    }

    private void OnEnable() => _health.OnDeath += HandleDeath;
    private void OnDisable() => _health.OnDeath -= HandleDeath;

    // Modify shield methods
    private void ModifyShield(int amount) => _currentShield += amount;
    
    // Modify health methods
    public void AddHealth(int boost) => _health.ModifyHealth(boost);

    public void TakeDamage(int damage)
    {
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