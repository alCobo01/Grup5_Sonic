using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<PlayerPowerUpController>();

        if (player != null)
        {
            AudioManager.Instance.PlayPickEmerald(player.transform);
            player.HasKey = true;
            Destroy(gameObject);
        }
    }
}
