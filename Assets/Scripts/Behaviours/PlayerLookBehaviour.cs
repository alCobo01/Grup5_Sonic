using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputController))]
public class PlayerLookBehaviour : MonoBehaviour
{
    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 80f;
    [SerializeField] private bool invertY = false;

    [Header("Camera")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float fov = 60f;
    [SerializeField] private float fovBoost = 80f;
    [SerializeField] [Range(0f, 1f)] private float fovBoostThresholdPercent = 0.75f;
    [SerializeField] private float fovTransitionSpeed = 5f;

    private Rigidbody _rb;
    private PlayerInputController _input;
    private PlayerMovementBehaviour _movement;

    private float _yaw;
    private float _pitch;
    private Vector2 _lookInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInputController>();
        _movement = GetComponent<PlayerMovementBehaviour>();

        playerCamera.fieldOfView = fov;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _input.OnLookEvent += input => _lookInput = input;
    }

    private void OnDestroy()
    {
        _input.OnLookEvent -= input => _lookInput = input;
    }

    private void Update()
    {
        Rotate();
        UpdateFov();
    }

    private void Rotate()
    {
        _yaw += _lookInput.x * mouseSensitivity * Time.deltaTime;
        _pitch += (invertY ? _lookInput.y : -_lookInput.y) * mouseSensitivity * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, -maxLookAngle, maxLookAngle);

        transform.rotation = Quaternion.Euler(0f, _yaw, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }

    private void UpdateFov()
    {
        float speed = _rb.linearVelocity.magnitude;
        float maxSpeed = _movement != null ? _movement.MaxSpeed : 8f;

        float targetFov = speed >= maxSpeed * fovBoostThresholdPercent
            ? fovBoost
            : fov;

        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFov, Time.deltaTime * fovTransitionSpeed);
    }
}