using UnityEngine;

public class GameOverZoneTrigger : MonoBehaviour
{
    [Tooltip("Tag del jugador para detectar la colisión")]
    [SerializeField] private string playerTag = "Player";

    private bool _hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggered) return;

        if (other.CompareTag(playerTag))
        {
            _hasTriggered = true;
            MenuManager.Instance.ShowMenu<GameOverMenu>();
        }
    }
}