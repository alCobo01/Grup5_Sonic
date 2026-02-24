using System.Collections;
using UnityEngine;

public class LostRing : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float flickerDuration = 2f;

    private MeshRenderer _renderer;

    private void Awake() => _renderer = GetComponent<MeshRenderer>();
    private void Start() => StartCoroutine(DespawnRoutine());
    
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
