using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(PlayerGroundChecker))]
public class PlayerJumpBehaviour : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float jumpPower = 5f;

    private Rigidbody _rb;
    private PlayerInputController _input;
    private PlayerGroundChecker _groundChecker;
    private AnimationBehaviour _animation;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInputController>();
        _groundChecker = GetComponent<PlayerGroundChecker>();
        _animation = GetComponent<AnimationBehaviour>();

        _input.OnJumpEvent += HandleJump;
    }

    private void OnDestroy()
    {
        _input.OnJumpEvent -= HandleJump;
    }

    private void HandleJump()
    {
        if (!_groundChecker.IsGrounded) return;

        _rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        if (_animation) _animation.TriggerJump();
    }
}