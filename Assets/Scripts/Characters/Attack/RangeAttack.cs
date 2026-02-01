using UnityEngine;

[RequireComponent(typeof(AnimationBehaviour))]
public class RangeAttack : MonoBehaviour, IAttack
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;

    private AnimationBehaviour _animationBehaviour;

    private void Awake()
    {
        _animationBehaviour = GetComponent<AnimationBehaviour>();
    }

    public void Attack()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            var rb = bullet.GetComponent<Rigidbody>();
 
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }
        
        _animationBehaviour.TriggerRangeAttack();
    }
}
