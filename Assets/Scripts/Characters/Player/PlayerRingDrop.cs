using UnityEngine;

public class PlayerRingDrop : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject ringPrefab;
    [SerializeField] private float explosionForce = 5f;
    [SerializeField] private float upwardForce = 2f;
    [SerializeField] private Transform spawnPoint;

    [Header("Ring drop config")] 
    [SerializeField] private float dropPercentage = 0.5f;
    [SerializeField] private int maxRingsToDrop = 20;
    [SerializeField] private int minRingsToDrop = 1;

    public void DropRingsOnHit(int amount)
    {
        var ringsToDrop = Mathf.RoundToInt(amount * dropPercentage);
        ringsToDrop = Mathf.Clamp(ringsToDrop, minRingsToDrop, maxRingsToDrop);
        
        var origin = spawnPoint.position;
        for (var i = 0; i < ringsToDrop; i++)
        {
            SpawnSingleRing(origin);
        }
    }
    
    private void SpawnSingleRing(Vector3 origin)
    {
        var ring = Instantiate(ringPrefab, origin, Random.rotation);
        
        // Ignore collision between all ring colliders and all player colliders
        var ringColliders = ring.GetComponents<Collider>();
        var playerColliders = GetComponentsInChildren<Collider>();
        foreach (var rc in ringColliders)
        {
            foreach (var pc in playerColliders)
            {
                Physics.IgnoreCollision(rc, pc, true);
            }
        }
        
        var rb = ring.GetComponent<Rigidbody>();
        
        var randomDir = Random.onUnitSphere;
        randomDir.y = Mathf.Abs(randomDir.y) + upwardForce;
        randomDir.Normalize();
        
        // Add explosive and rotation force (random turn)
        rb.AddForce(randomDir * explosionForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * explosionForce, ForceMode.Impulse);
    }
}