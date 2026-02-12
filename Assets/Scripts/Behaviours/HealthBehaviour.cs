using UnityEngine;
using System;

public class HealthBehaviour : MonoBehaviour
{
    [Header("Health values")]
    [SerializeField] private int maxHealth;

    //Events for other classes to subscribe
    public event Action<int> OnHealthChange = delegate { };
    public event Action OnDeath = delegate { };

    //Class properties
    public int CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    private void Awake() => CurrentHealth = maxHealth;

    private void Start() => OnHealthChange?.Invoke(CurrentHealth);

    public void ModifyHealth(int amount)
    {
        if (IsDead) return;
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);
        OnHealthChange?.Invoke(CurrentHealth);
        if (IsDead) OnDeath?.Invoke();
    }

    public void Kill()
    {
        if (IsDead) return;
        CurrentHealth = 0;
        OnHealthChange?.Invoke(0);
        OnDeath?.Invoke();
    }
}
