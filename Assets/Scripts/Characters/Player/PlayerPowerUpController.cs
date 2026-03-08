using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerHealthController))]
public class PlayerPowerUpController : MonoBehaviour
{
    [SerializeField] private GameObject shieldVFX;
    
    private PlayerHealthController _healthController;
    public bool HasKey { get; set; }
    private Coroutine _shieldCoroutine;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && player.TryGetComponent(out PlayerHealthController healthController))
        {
            _healthController = healthController;
        }
    }

    private void OnEnable()
    {
        PlayerHealthController.OnShieldBroken += HandleShieldBroken;
    }

    private void OnDisable()
    {
        PlayerHealthController.OnShieldBroken -= HandleShieldBroken;
    }
    
    public void ActivatePowerUp(PowerUp powerUp)
    {
        switch (powerUp.type)
        {
            case PowerUpType.Health:
                _healthController.AddLives(powerUp.amount);
                break;
            case PowerUpType.Ring:
                _healthController.AddRings(powerUp.amount);
                break;
            case PowerUpType.Shield:
                if (_shieldCoroutine != null) StopCoroutine(_shieldCoroutine);
                _shieldCoroutine = StartCoroutine(ShieldRoutine(powerUp.amount, powerUp.duration));
                break;
            case PowerUpType.Invincibility:
                StartCoroutine(InvincibilityRoutine(powerUp.duration));
                break;
            default:
                Debug.Log("Something isn't working!");
                break;
        }
    }

    private IEnumerator ShieldRoutine(int amount, float duration)
    {
        _healthController.AddShield(amount);
        shieldVFX.SetActive(true);
        yield return new WaitForSeconds(duration);
        shieldVFX.SetActive(false);
        _healthController.RemoveShield(amount);
        _shieldCoroutine = null;
    }

    private void HandleShieldBroken()
    {
        if (_shieldCoroutine != null)
        {
            StopCoroutine(_shieldCoroutine);
            _shieldCoroutine = null;
        }
        shieldVFX.SetActive(false);
    }

    private IEnumerator InvincibilityRoutine(float duration)
    {
        _healthController.IsInvincible = true;
        yield return new WaitForSeconds(duration);
        _healthController.IsInvincible = false;
    }
}
