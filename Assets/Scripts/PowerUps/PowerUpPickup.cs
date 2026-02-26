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
    [SerializeField] private GameObject brokenPrefab;
    [SerializeField] private GameObject brokenVFX;

    private PlayerPowerUpController _breakCollector;
    private bool _hasBroken;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && player.TryGetComponent(out PlayerPowerUpController powerUpController))
            _breakCollector = powerUpController;
    }

    private void PlayCollectSound()
    {
        if (!string.IsNullOrEmpty(powerUpData.sfxName))
            AudioManager.Instance.PlaySFX(powerUpData.sfxName, transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasBroken) return;
        if (collectMode != PowerUpCollectMode.Touch) return;
        if (!other.CompareTag("Player")) return;
        Collect(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasBroken) return;
        if (collectMode != PowerUpCollectMode.Touch) return;
        if (!collision.gameObject.CompareTag("Player")) return;
        Collect(collision.gameObject);
    }

    public void TakeDamage(int damage)
    {
        if (collectMode != PowerUpCollectMode.Break) return;
        if (damage == 0) return;
        CollectFromFallback();
    }

    public void InstantKill()
    {
        if (collectMode != PowerUpCollectMode.Break) return;
        CollectFromFallback();
    }

    private void Collect(GameObject interactor)
    {
        if (_hasBroken) return;
        PlayCollectSound();
        if (interactor.TryGetComponent(out PlayerPowerUpController powerUpController))
            powerUpController.ActivatePowerUp(powerUpData);
        Destroy(gameObject);
    }

    private void CollectFromFallback()
    {
        if (_hasBroken) return;
        _hasBroken = true;
        PlayCollectSound();
        _breakCollector.ActivatePowerUp(powerUpData);
        SwapToBrokenVisuals();
        Destroy(gameObject);
    }

    private void SwapToBrokenVisuals()
    {
        if (!brokenPrefab) return;
        var vfx = Instantiate(brokenVFX, transform.position, transform.rotation);
        Destroy(vfx, 2f);
        Instantiate(brokenPrefab, transform.position, transform.rotation);
    }
}