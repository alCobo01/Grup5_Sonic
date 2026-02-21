using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AnimationBehaviour : MonoBehaviour
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    public Animator Animator { get; private set; }
    private Rigidbody _rigidbody;
    private PlayerGroundChecker _groundChecker;
    private PlayerMovementBehaviour _movement;

    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }
        _rigidbody = GetComponent<Rigidbody>();
        _groundChecker = GetComponent<PlayerGroundChecker>();
        _movement = GetComponent<PlayerMovementBehaviour>();
    }

    private void Update()
    {
        if (_animator == null) return;

        // Update Speed
        Vector3 velocity = _rigidbody.linearVelocity;
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;
        _animator.SetFloat(SpeedHash, currentSpeed, 0.1f, Time.deltaTime);

        // Update Run  Animation Speed based on movement
        float playbackSpeed = 1f;
        if (_movement != null && currentSpeed > _movement.MaxSpeed * 0.75f)
        {
            playbackSpeed = 2f;
        }
        else
        {
            playbackSpeed = Mathf.Max(1f, currentSpeed * animSpeedMultiplier);
        }
        _animator.SetFloat(AnimSpeedHash, playbackSpeed);

        // Update Grounded state
        if (_groundChecker != null)
        {
            _animator.SetBool(IsGroundedHash, _groundChecker.IsGrounded);
        }
    }

    public void TriggerMeleeAttack()
    {
        if (_animator != null) _animator.SetTrigger(MeleeAttackHash);
    }


    public void SetAiming(bool isAiming)
    {
        if (_animator != null) _animator.SetBool(IsAimingHash, isAiming);
    }

    public void TriggerJump()
    {
        if (_animator != null) _animator.SetTrigger(JumpHash);
    }

    public void SetGrounded(bool isGrounded)
    {
        if (_animator != null) _animator.SetBool(IsGroundedHash, isGrounded);
    }
} 
