using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private static readonly int UsedHash = Animator.StringToHash("Used");
    
    [SerializeField] private LayerMask playerLayer;
    
    private Animator _animator;
    private bool _used;
    
    private void Awake() => _animator = GetComponent<Animator>();
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") || collision.gameObject.GetComponentInParent<PlayerMovementBehaviour>() != null)
        {
            if (!_used)
            {
                AudioManager.Instance.PlayCheckpoint(transform);
                _animator.SetTrigger(UsedHash);
                _used = true;
            }
            GameManager.Instance.LastCheckPoint(gameObject);
        }
    }
}
