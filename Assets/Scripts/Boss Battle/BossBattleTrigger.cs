using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BossBatlleTrigger : MonoBehaviour
{
    public static event UnityAction OnPlayerEnterTrigger;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private GameObject wallPrefab;

    private bool _triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;
        if (!other.gameObject.CompareTag(playerTag)) return;

        _triggered = true;
        StartCoroutine(ActiveWall());
        OnPlayerEnterTrigger?.Invoke();
    }

    private IEnumerator ActiveWall()
    {
        yield return new WaitForSeconds(1);
        wallPrefab.SetActive(true);
    }
}
