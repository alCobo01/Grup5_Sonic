using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LostRing : MonoBehaviour
{
    [SerializeField] public float lifeTime = 5f;
    [SerializeField] private float flickerDuration = 2f;
    [SerializeField] private float collectableDelay = 0.5f;

    private MeshRenderer _renderer;
    private List<Collider> _colliders, _playerColliders;
    private Collider _triggerCollider;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _colliders = GetComponents<Collider>().ToList();

        _triggerCollider = _colliders.Find(c => c.isTrigger);
        _triggerCollider.enabled = false;
        
        _playerColliders = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<Collider>().ToList();
    }

    private void Start()
    {
        StartCoroutine(EnableCollectionRoutine());
        StartCoroutine(DespawnRoutine());
    }

    private IEnumerator EnableCollectionRoutine()
    {
        yield return new WaitForSeconds(collectableDelay);

        // Re-enable the trigger collider so the player can pick up the ring
        _triggerCollider.enabled = true;
        
        foreach (var ringCol in _colliders)
        {
            foreach (var playerCol in _playerColliders)
            {
                Physics.IgnoreCollision(ringCol, playerCol, false);
            }
        }
    }
    
    private IEnumerator DespawnRoutine()
    {
        yield return new WaitForSeconds(lifeTime - flickerDuration);

        var elapsed = 0f;
        while (elapsed < flickerDuration)
        {
            _renderer.enabled = !_renderer.enabled;
            var flickerSpeed = Mathf.Lerp(0.15f, 0.05f, elapsed / flickerDuration);
            yield return new WaitForSeconds(flickerSpeed);
            elapsed += flickerSpeed;
        }
        
        Destroy(gameObject);
    }
}
