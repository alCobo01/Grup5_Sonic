using System;
using UnityEngine;

public class Trampoline : MonoBehaviour, ILauncher
{
    public enum TrampolineMode { Flat, Sideways }

    [Header("Mode")]
    [SerializeField] private TrampolineMode mode = TrampolineMode.Flat;

    [Header("Launch Settings")]
    [SerializeField] private float launchForce = 15f;

    [Header("Layer")]
    [SerializeField] private LayerMask playerLayer;

    public event Action<Vector3, float> OnPlayerLaunched;

    public void Launch(Rigidbody rb)
    {
        AudioManager.Instance.PlayTrampolines(transform);
        Vector3 direction = GetLaunchDirection();

        CancelVelocityOnAxis(rb, direction);

        rb.AddForce(direction * launchForce, ForceMode.VelocityChange);

        OnPlayerLaunched?.Invoke(direction, launchForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        if (other.TryGetComponent(out Rigidbody rb))
        {
            Launch(rb);
            
            if (other.TryGetComponent(out AnimationBehaviour animation))
            {
                animation.TriggerJump();
            }
        }
    }

    private Vector3 GetLaunchDirection()
    {
        return mode switch
        {
            TrampolineMode.Flat => Vector3.up,
            TrampolineMode.Sideways => transform.up,
            _ => Vector3.up
        };
    }

    private void CancelVelocityOnAxis(Rigidbody rb, Vector3 axis)
    {
        float projected = Vector3.Dot(rb.linearVelocity, axis);

        if (projected < 0f)
            rb.linearVelocity -= axis * projected;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 dir = mode == TrampolineMode.Flat ? Vector3.up : transform.up;
        Gizmos.DrawRay(transform.position, dir * (launchForce * 0.1f));
    }
}