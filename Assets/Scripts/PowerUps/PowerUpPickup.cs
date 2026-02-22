using UnityEngine;

public enum PowerUpCollectMode
{
    Touch,
    Break
}

public class PowerUpPickup : MonoBehaviour, IDamageable
{
    [SerializeField] private PowerUp powerUpData;
    [SerializeField] private PowerUpCollectMode collectMode;
    [SerializeField] private PlayerPowerUpController breakCollector;
    [SerializeField] private GameObject brokenPrefab;
    
    private bool _hasBroken;
    
    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance.PlayPickRings(transform);
        if (_hasBroken) return;
        if (collectMode != PowerUpCollectMode.Touch) return;
        Collect(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.PlayPickRings(transform);
        if (_hasBroken) return;
        if (collectMode != PowerUpCollectMode.Touch) return;
        Collect(collision.gameObject);
    }

    public void TakeDamage(int damage)
    {
        AudioManager.Instance.PlayPickRings(transform);
        if (collectMode != PowerUpCollectMode.Break) return;
        if (damage == 0) return;
        CollectFromFallback();
    }

    public void InstantKill()
    {
        AudioManager.Instance.PlayPickRings(transform);
        if (collectMode != PowerUpCollectMode.Break) return;
        CollectFromFallback();
    }

    private void Collect(GameObject interactor)
    {
        AudioManager.Instance.PlayPickRings(transform);
        if (_hasBroken) return;
        var powerUpController = interactor.GetComponentInParent<PlayerPowerUpController>();
        powerUpController.ActivatePowerUp(powerUpData);
        Destroy(gameObject);
    }

    private void CollectFromFallback()
    {
        AudioManager.Instance.PlayPickRings(transform);
        if (_hasBroken) return;
        _hasBroken = true;
        breakCollector.ActivatePowerUp(powerUpData);

        SwapToBrokenVisuals();
        Destroy(gameObject);
    }

    private void SwapToBrokenVisuals()
    {
        AudioManager.Instance.PlayPickRings(transform);
        if (!brokenPrefab) return;
        Instantiate(brokenPrefab, transform.position, transform.rotation);
    }
}
