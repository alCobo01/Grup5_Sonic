using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(PlayerGroundChecker))]
public class PlayerMovementBehaviour : MonoBehaviour
{
    [Header("Acceleration")]
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 25f;

    public float MaxSpeed => maxSpeed;

    private Rigidbody _rb;
    private PlayerInputController _input;
    private PlayerCrouchBehaviour _crouch;

    private Vector2 _moveInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _input = GetComponent<PlayerInputController>();
        _crouch = GetComponent<PlayerCrouchBehaviour>();

        _input.OnMoveEvent += input => _moveInput = input;
    }

    private void OnDestroy()
    {
        _input.OnMoveEvent -= input => _moveInput = input;
    }

    private void FixedUpdate()
    {
        if (_crouch && _crouch.IsCrouching) return;

        Vector3 wishDir = (transform.right * _moveInput.x
                         + transform.forward * _moveInput.y).normalized;

        Vector3 horizontalVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

        if (wishDir.magnitude > 0f)
        {
            Vector3 velocityDiff = wishDir * maxSpeed - horizontalVel;
            Vector3 accel = Vector3.ClampMagnitude(velocityDiff, acceleration * Time.fixedDeltaTime);
            _rb.AddForce(accel, ForceMode.VelocityChange);
        }
        else
        {
            Vector3 brake = Vector3.ClampMagnitude(-horizontalVel, deceleration * Time.fixedDeltaTime);
            _rb.AddForce(brake, ForceMode.VelocityChange);
        }
    }
}