using UnityEngine;

public class WinZoneTrigger : MonoBehaviour
{
    private static readonly int UsedHash = Animator.StringToHash("Chase");
    
    [Tooltip("Tag del jugador para detectar la colisión")]
    [SerializeField] private string playerTag = "Player";
    private Animator _animator;
    private bool _hasTriggered = false;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggered) return;
        if (other.CompareTag(playerTag))
        {
            _animator.SetTrigger(UsedHash);
            Invoke(nameof(ShowWinMenu), 2.5f); 
        }
    }
    
    private void ShowWinMenu()
    {
        _hasTriggered = true;
        MenuManager.Instance.ShowMenu<WinMenu>();
    }
}