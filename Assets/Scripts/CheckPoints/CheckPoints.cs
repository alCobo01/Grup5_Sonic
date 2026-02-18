using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerLayer.value != 0 && (playerLayer.value & (1 << collision.gameObject.layer)) == 0)
        {
            Debug.Log("Layer mismatch. Player layer: " + collision.gameObject.layer + " Mask: " + playerLayer.value);
        }

        if (collision.CompareTag("Player") || collision.gameObject.GetComponentInParent<PlayerMovementBehaviour>() != null)
        {
            //GameManager.Instance.LastCheckPoint(gameObject);
        }
    }
}
