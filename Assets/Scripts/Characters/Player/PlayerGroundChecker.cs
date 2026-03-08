using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.9f;
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask groundLayer;

    public bool IsGrounded { get; private set; }

    private void Update()
    {
        IsGrounded = Check();
    }

    private bool Check()
    {
        Vector3 origin = transform.position + Vector3.up * (groundCheckRadius + 0.1f);
        return Physics.SphereCast(origin, groundCheckRadius, Vector3.down, out _, groundCheckDistance, groundLayer);
    }
}