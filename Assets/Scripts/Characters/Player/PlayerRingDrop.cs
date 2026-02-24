using UnityEngine;

public class PlayerRingDrop : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject ringPrefab;
    [SerializeField] private int ringsToDrop = 10;
    [SerializeField] private float explosionForce = 5f;
    [SerializeField] private float upwardForce = 2f;
    [SerializeField] private Transform spawnPoint;

    public void DropRingsOnHit()
    {
        var origin = spawnPoint.position;
        for (int i = 0; i < ringsToDrop; i++)
        {
            SpawnSingleRing(origin);
        }
    }
    
    private void SpawnSingleRing(Vector3 origin)
    {
        var ring = Instantiate(ringPrefab, origin, Random.rotation);
        
        var ringCollider = ring.GetComponent<Collider>();
        var playerCollider = GetComponent<Collider>();
        var rb = ring.GetComponent<Rigidbody>();
        
        var randomDir = Random.onUnitSphere;
        randomDir.y = Mathf.Abs(randomDir.y) + upwardForce;
        randomDir.Normalize();
        
        // Add explosive and rotation force (random turn)
        rb.AddForce(randomDir * explosionForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * explosionForce, ForceMode.Impulse);
        
    }
}