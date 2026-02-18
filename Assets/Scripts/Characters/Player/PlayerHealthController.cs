using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR.Haptics;

[RequireComponent(typeof(HealthBehaviour))]
public class PlayerHealthController : MonoBehaviour, IDamageable, IRingWallet
{
    public static event UnityAction OnDeath;
    public static event UnityAction<int> OnRingsChanged;
    public static event UnityAction<int> OnLivesChanged;
    public static event UnityAction OnLifeLost;
    
    public bool IsInvincible { get; set; }
    public int CurrentRings { get; private set; }
    public int CurrentLives => _health != null ? _health.CurrentLives : 0;

    private int _currentShield;
    private HealthBehaviour _health;

    private void Awake()
    {
        _health = GetComponent<HealthBehaviour>();
        CurrentRings = 2;
    }
    
    // Modify shield methods
    public void AddShield(int amount) => _currentShield += amount;
    public void RemoveShield(int amount) => _currentShield = Mathf.Max(0, _currentShield - amount);
    
    // Modify health methods
    public void AddRings(int amount)
    {
        if (_health.IsDead) return;
        if (amount == 0) return;
        CurrentRings = Mathf.Max(0, CurrentRings + amount);
        OnRingsChanged?.Invoke(CurrentRings);
    }

    public void ResetRings()
    {
        CurrentRings = 0;
        OnRingsChanged?.Invoke(CurrentRings);
    }

    public bool TrySpendRing()
    {
        if (_health.IsDead) return false;
        if (CurrentRings < 1) return false;

        CurrentRings -= 1;
        OnRingsChanged?.Invoke(CurrentRings);
        return true;
    }

    public void AddLives(int amount)
    {
        if (_health.IsDead) return;
        _health.CurrentLives += amount;
        OnLivesChanged?.Invoke(_health.CurrentLives);
    }

    public void TakeDamage(int damage)
    {
        if (IsInvincible || _health.IsDead) return;
        damage = Mathf.Abs(damage);
        
        // Shield Logic
        if (_currentShield > 0)
        {
            if (damage < _currentShield)
            {
                _currentShield -= damage;
                return;
            }
            
            damage -= _currentShield;
            _currentShield = 0;
        }
        
        // Damage reduces rings
        CurrentRings = Mathf.Max(0, CurrentRings - damage);
        OnRingsChanged?.Invoke(CurrentRings);

        // If rings reach 0, lose a life
        if (CurrentRings <= 0)
        {
            _health.LoseLife();
            OnLivesChanged?.Invoke(_health.CurrentLives);
            
            if (_health.IsDead) OnDeath?.Invoke();
            else OnLifeLost?.Invoke();
        }
        
        Debug.Log($"Rings: {CurrentRings}, lives: {_health.CurrentLives}");
    }

    public void InstantKill()
    {
        _health.LoseLife();
        OnLivesChanged?.Invoke(_health.CurrentLives);

        if (_health.IsDead) OnDeath?.Invoke();
        else OnLifeLost?.Invoke();
    } 
}
