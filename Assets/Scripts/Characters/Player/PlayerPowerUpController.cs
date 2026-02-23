using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerHealthController))]
public class PlayerPowerUpController : MonoBehaviour
{
    private PlayerHealthController _healthController;
    public bool HasKey { get; set; }

    private void Awake()
    {
        _healthController = GetComponent<PlayerHealthController>();
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
                StartCoroutine(ShieldRoutine(powerUp.amount, powerUp.duration));
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
        yield return new WaitForSeconds(duration);
        _healthController.RemoveShield(amount);
    }

    private IEnumerator InvincibilityRoutine(float duration)
    {
        _healthController.IsInvincible = true;
        yield return new WaitForSeconds(duration);
        _healthController.IsInvincible = false;
    }
}
