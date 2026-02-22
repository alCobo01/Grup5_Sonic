using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private static readonly int UsedHash = Animator.StringToHash("Used");
    
    [SerializeField] private LayerMask playerLayer;
    private Animator _animator;
    
    private void Awake() => _animator = GetComponent<Animator>();
    
    private void OnTriggerEnter(Collider collision)
    {
        if (playerLayer.value != 0 && (playerLayer.value & (1 << collision.gameObject.layer)) == 0)
        {
            Debug.Log("Layer mismatch. Player layer: " + collision.gameObject.layer + " Mask: " + playerLayer.value);
        }

        if (collision.CompareTag("Player") || collision.gameObject.GetComponentInParent<PlayerMovementBehaviour>() != null)
        {
            _animator.SetTrigger(UsedHash);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.LastCheckPoint(gameObject);
            }
            else
            {
                Debug.LogError("GameManager instance not found!");
            }
        }
    }
}
