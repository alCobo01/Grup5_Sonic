using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        PlayerPowerUpController player = other.GetComponentInParent<PlayerPowerUpController>();

        if (player != null)
        {
            AudioManager.Instance.PlayPickEmerald(player.transform);
            player.HasKey = true;
            Debug.Log("[KeyPickup] Llave recogida! Plataformas condicionales activadas.");
            Destroy(gameObject);
        }
    }
}
