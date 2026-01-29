using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerHealthController))]
public class PlayerPowerUpController : MonoBehaviour
{
    private PlayerHealthController _healthController;

    private void Awake()
    {
        _healthController = GetComponent<PlayerHealthController>();
    }

    public void ActivatePowerUp(PowerUp powerUp)
    {
        Debug.Log($"Collected PowerUp: {powerUp.powerUpName} ({powerUp.type})");

        switch (powerUp.type)
        {
            case PowerUpType.Health:
                _healthController.AddHealth(powerUp.amount);
                break;
            case PowerUpType.Shield:
                StartCoroutine(ShieldRoutine(powerUp.amount, powerUp.duration));
                break;
            case PowerUpType.Speed:
                StartCoroutine(SpeedBoostRoutine(powerUp.duration, powerUp.multiplier));
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
        Debug.Log($"Shield activated! Amount: {amount} for {duration} seconds.");

        yield return new WaitForSeconds(duration);

        _healthController.RemoveShield(amount);
        Debug.Log("Shield ended.");
    }

    private IEnumerator SpeedBoostRoutine(float duration, float multiplier)
    {
        // here goes speed modification
        Debug.Log($"Speed Boost activated! Multiplier: {multiplier} for {duration} seconds.");
        yield return new WaitForSeconds(duration);
        Debug.Log("Speed Boost ended.");
    }

    private IEnumerator InvincibilityRoutine(float duration)
    {
        _healthController.IsInvincible = true;
        Debug.Log($"Invincibility activated! Duration: {duration} seconds.");
        
        yield return new WaitForSeconds(duration);
        
        _healthController.IsInvincible = false;
        Debug.Log("Invincibility ended.");
    }
}
