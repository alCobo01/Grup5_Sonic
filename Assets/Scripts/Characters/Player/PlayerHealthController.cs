using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HealthBehaviour))]
[RequireComponent(typeof(PlayerRingDrop))]
public class PlayerHealthController : MonoBehaviour, IDamageable, IRingWallet
{
    public bool IsInvincible { get; set; }
    
    [SerializeField] private float invincibilityDuration = 0.5f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private Rigidbody rb;
    
    public static event UnityAction OnDeath;
    public static event UnityAction<int> OnRingsChanged;
    public static event UnityAction<int> OnLivesChanged;
    public static event UnityAction OnShieldBroken;
    public static event Action ReloadPlayer = delegate { };
    
    private int _currentShield, _currentRings;
    private HealthBehaviour _health;
    private PlayerRingDrop _ringDrop;

    private void Awake()
    {
        _health = GetComponent<HealthBehaviour>();
        _ringDrop = GetComponent<PlayerRingDrop>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //Update stats for not starting on 0 on UI
        OnRingsChanged?.Invoke(_currentRings);
        OnLivesChanged?.Invoke(_health.CurrentLives);
    }

    // Modify shield methods
    public void AddShield(int amount) => _currentShield += amount;
    public void RemoveShield(int amount) => _currentShield = Mathf.Max(0, _currentShield - amount);
    
    // Modify health methods
    public void AddRings(int amount)
    {
        if (_health.IsDead) return;
        if (amount == 0) return;
        _currentRings = Mathf.Max(0, _currentRings + amount);
        OnRingsChanged?.Invoke(_currentRings);
    }

    public bool TrySpendRing()
    {
        if (_health.IsDead) return false;
        if (_currentRings < 1) return false;

        _currentRings -= 1;
        OnRingsChanged?.Invoke(_currentRings);
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
                StartCoroutine(InvincibilityCoroutine());
                return;
            }
            _currentShield = 0;
            OnShieldBroken?.Invoke();
        }

        //Knockback
        rb.AddForce(-transform.forward * knockbackForce, ForceMode.VelocityChange);

        // If rings reach 0, lose a life
        if (_currentRings <= 0)
        {
            _health.LoseLife();
            OnLivesChanged?.Invoke(_health.CurrentLives);

            if (_health.IsDead)
            {
                OnDeath?.Invoke();
                AudioManager.Instance.PlayMusicByIndex(3);
            }
            else
                ReloadPlayer.Invoke();
        }
        else
        {
            _ringDrop.DropRingsOnHit(_currentRings);
            _currentRings = 0;
            AudioManager.Instance.PlayLoseRings(transform);
            OnRingsChanged?.Invoke(_currentRings);
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        IsInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        IsInvincible = false;
    }

    public void InstantKill()
    {
        _health.LoseLife();
        OnLivesChanged?.Invoke(_health.CurrentLives);

        if (_health.IsDead) OnDeath?.Invoke();
    }
}
