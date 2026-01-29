using UnityEngine;

public class PowerUpPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private PowerUp powerUpData;

    public void Interact(GameObject interactor)
    {
        if (interactor != null && interactor.TryGetComponent(out PlayerPowerUpController powerUpController))
        {
            powerUpController.ActivatePowerUp(powerUpData);
            // play pickup sound
            Destroy(gameObject);
        }
    }
}
