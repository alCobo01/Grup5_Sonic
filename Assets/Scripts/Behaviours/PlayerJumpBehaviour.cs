using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(PlayerGroundChecker))]
public class PlayerJumpBehaviour : MonoBehaviour
{
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    
    [Header("Jump")]
    [SerializeField] private float jumpPower = 5f;

    private Rigidbody _rb;
    private PlayerInputController _input;
    private AnimationBehaviour _animationBehaviour;
    private PlayerGroundChecker _groundChecker;
    private AnimationBehaviour _animation;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInputController>();
        _animationBehaviour = GetComponent<AnimationBehaviour>();
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

        AudioManager.Instance.PlayJump(transform);
        _rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        if (_animation) _animation.TriggerJump();
    }
}