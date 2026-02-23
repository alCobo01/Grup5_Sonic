using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        PlayerPowerUpController player = other.GetComponentInParent<PlayerPowerUpController>();

        if (player != null)
        {
            player.HasKey = true;
            Debug.Log("[KeyPickup] Llave recogida! Plataformas condicionales activadas.");
            Destroy(gameObject);
        }
    }
}
