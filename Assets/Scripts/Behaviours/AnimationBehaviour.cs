using UnityEngine;

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

    [SerializeField] private Animator _animator;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_animator == null) return;

        Vector3 velocity = _rigidbody.linearVelocity;
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;
        _animator.SetFloat(SpeedHash, currentSpeed, 0.1f, Time.deltaTime);
    }

    public void PlayDance()
    {
        if (_animator != null) _animator.SetTrigger(DanceHash);
    }

    public void TriggerMeleeAttack()
    {
        if (_animator != null) _animator.SetTrigger(MeleeAttackHash);
    }

    public void TriggerRangeAttack()
    {
        if (_animator != null) _animator.SetTrigger(RangeAttackHash);
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

    public void SetCrouch(bool isCrouching)
    {
        if (_animator != null) _animator.SetBool(IsCrouchedHash, isCrouching);
    }
} 
