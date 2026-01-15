using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class AnimationBehaviour : MonoBehaviour
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int DanceHash = Animator.StringToHash("Dance");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int IsCrouchedHash = Animator.StringToHash("IsCrouched");
    private static readonly int MeleeAttackHash = Animator.StringToHash("MeleeAttack");
    private static readonly int RangeAttackHash = Animator.StringToHash("RangeAttack");
    private static readonly int IsAimingHash = Animator.StringToHash("IsAiming");

    private Animator _animator;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 velocity = _rigidbody.linearVelocity;
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;
        _animator.SetFloat(SpeedHash, currentSpeed, 0.1f, Time.deltaTime);
    }

    public void PlayDance()
    {
        _animator.SetTrigger(DanceHash);
    }

    public void TriggerMeleeAttack()
    {
        _animator.SetTrigger(MeleeAttackHash);
    }

    public void TriggerRangeAttack()
    {
        _animator.SetTrigger(RangeAttackHash);
    }

    public void SetAiming(bool isAiming)
    {
        _animator.SetBool(IsAimingHash, isAiming);
    }

    public void TriggerJump()
    {
        _animator.SetTrigger(JumpHash);
    }

    public void SetGrounded(bool isGrounded)
    {
        _animator.SetBool(IsGroundedHash, isGrounded);
    }

    public void SetCrouch(bool isCrouching)
    {
        _animator.SetBool(IsCrouchedHash, isCrouching);
    }
} 
