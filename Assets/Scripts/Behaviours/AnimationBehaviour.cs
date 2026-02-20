using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class AnimationBehaviour : MonoBehaviour
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    public Animator Animator { get; private set; }
    private Rigidbody _rigidbody;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var velocity = _rigidbody.linearVelocity;
        var horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
        var currentSpeed = horizontalVelocity.magnitude;
        Animator.SetFloat(SpeedHash, currentSpeed, 0.1f, Time.deltaTime);
    }

    public void Trigger(int hash) => Animator.SetTrigger(hash);
    public void SetBool(int hash, bool state) => Animator.SetBool(hash, state);
} 
