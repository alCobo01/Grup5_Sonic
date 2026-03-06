using UnityEngine;
using UnityEngine.Events;

public class BossBatlleTrigger : MonoBehaviour
{
    public static event UnityAction OnPlayerEnterTrigger;
    [SerializeField] private string playerTag = "Player";
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag)) OnPlayerEnterTrigger?.Invoke();
    }
}