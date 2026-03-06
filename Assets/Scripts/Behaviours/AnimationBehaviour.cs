using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AnimationBehaviour : MonoBehaviour
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int AnimSpeedHash = Animator.StringToHash("AnimSpeed");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int IsAimingHash = Animator.StringToHash("IsAiming");
    private static readonly int RangeAttackHash = Animator.StringToHash("RangeAttack");
    private static readonly int MeleeAttackHash = Animator.StringToHash("MeleeAttack");

    [SerializeField] private Animator animator;
    [SerializeField] private float animSpeedMultiplier = 0.1f;
    
    private Rigidbody _rigidbody;
    private PlayerGroundChecker _groundChecker;
    private PlayerMovementBehaviour _movement;
    
    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        _rigidbody = GetComponent<Rigidbody>();
        _groundChecker = GetComponent<PlayerGroundChecker>();
        _movement = GetComponent<PlayerMovementBehaviour>();
    }

    private void Update()
    {
        // Update Speed
        Vector3 velocity = _rigidbody.linearVelocity;
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;
        animator.SetFloat(SpeedHash, currentSpeed, 0.1f, Time.deltaTime);

        // Update Run  Animation Speed based on movement
        if (_movement != null)
        {
            var playbackSpeed = 1f;
            playbackSpeed = currentSpeed > _movement.MaxSpeed * 0.75f ? 2f : Mathf.Max(1f, currentSpeed * animSpeedMultiplier);
            animator.SetFloat(AnimSpeedHash, playbackSpeed);
        }
        
        // Update Grounded state
        if (_groundChecker != null) animator.SetBool(IsGroundedHash, _groundChecker.IsGrounded);
        
    }
    public void Trigger(int hash) => animator.SetTrigger(hash);

    public void SetBool(int hash, bool state) => animator.SetBool(hash, state);

    public void TriggerMeleeAttack()
    {
        if (animator != null) animator.SetTrigger(MeleeAttackHash);
    }

    public void SetAiming(bool isAiming)
    {
        if (animator != null) animator.SetBool(IsAimingHash, isAiming);
    }

    public void TriggerJump()
    {
        if (animator != null) animator.SetTrigger(JumpHash);
    }
    public void TriggerRangeAttack()
    {
        if (animator != null) animator.SetTrigger(RangeAttackHash);
    }

    public void SetGrounded(bool isGrounded)
    {
        if (animator != null) animator.SetBool(IsGroundedHash, isGrounded);
    }
} 
