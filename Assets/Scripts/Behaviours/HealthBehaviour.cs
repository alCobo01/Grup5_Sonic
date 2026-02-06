using UnityEngine;
using System;

public class HealthBehaviour : MonoBehaviour
{
    [Header("Health values")]
    [SerializeField] private int maxHealth;

    //Class properties
    public int CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    private void Awake() => CurrentHealth = maxHealth;
    
    public void ModifyHealth(int amount)
    {
        if (IsDead) return;
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);
    }

    public void Kill()
    {
        if (IsDead) return;
        CurrentHealth = 0;
    }
}
