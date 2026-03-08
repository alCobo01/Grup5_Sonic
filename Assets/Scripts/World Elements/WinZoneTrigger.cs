using UnityEngine;
using UnityEngine.Events;

public class WinZoneTrigger : MonoBehaviour
{
    public static event UnityAction OnWin;
    
    private static readonly int UsedHash = Animator.StringToHash("Used");
    
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
            OnWin?.Invoke();
        }
    }
    
    private void ShowWinMenu()
    {
        AudioManager.Instance.PlayWinSequence(transform, "WinSoundEffect");
        _hasTriggered = true;
        MenuManager.Instance.ShowMenu<WinMenu>();
    }
}