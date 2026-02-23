using System;
using UnityEngine;

public class Accelerator : MonoBehaviour, ILauncher
{
    [Header("Boost Settings")]
    [SerializeField] private float boostSpeed = 40f;
    [SerializeField] private bool overrideCurrentSpeed = true;

    [Header("Layer")]
    [SerializeField] private LayerMask playerLayer;

    public event Action<Vector3, float> OnPlayerBoosted;

    public void Launch(Rigidbody rb)
    {
        AudioManager.Instance.PlayAccelerator(transform);
        Vector3 direction = -transform.right;

        float currentSpeed = Vector3.Dot(rb.linearVelocity, direction);

        if (!overrideCurrentSpeed && currentSpeed >= boostSpeed)
            return;

        float verticalSpeed = rb.linearVelocity.y;

        rb.linearVelocity = direction * boostSpeed + Vector3.up * verticalSpeed;

        OnPlayerBoosted?.Invoke(direction, boostSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        if (other.TryGetComponent(out Rigidbody rb))
            Launch(rb);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, -transform.right * 2f);

        Gizmos.color = new Color(1f, 0.6f, 0f, 0.4f);
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(1f, 1f, 0.2f));
    }
}