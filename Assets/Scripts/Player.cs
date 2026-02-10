using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Miquel is tha best :3")]

    [Header("Acceleration")]
    public float maxSpeed = 8f;
    public float acceleration = 20f;
    public float deceleration = 25f;

    [Header("Jump")]
    public float jumpPower = 5f;
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Crouch & Boost")]
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float maxCrouchChargeTime = 4f;
    public float minBoostSpeed = 2f;
    public float maxBoostSpeed = 40f;

    private bool isCrouching = false;
    private float crouchTimer = 0f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;
    public bool invertY = false;

    [Header("Camera")]
    public float fov = 60f;
    public float fovIncreaseSpeed = 80f;
    public float fovIncreaseThreshold = 2f;

    private float yaw;
    private float pitch;

    [Header("References")]
    public Camera playerCamera;
    private Rigidbody rb;

    [Header("Check Ground")]
    public float groundCheckDistance = 0.9f;
    public LayerMask groundLayer;

    private bool isGrounded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Evita que el Rigidbody rote o vuele
        rb.freezeRotation = true;
        playerCamera.fieldOfView = fov;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Look();
        Jump();
        HandleCrouch();

        isGrounded = CheckGround();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        yaw += mouseX;
        pitch += invertY ? mouseY : -mouseY;

        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        float velocity = rb.linearVelocity.magnitude;
        Debug.Log($"Current velocity: {velocity}");
        if (velocity >= maxSpeed - fovIncreaseThreshold)
        {
            playerCamera.fieldOfView = fovIncreaseSpeed;
        }
        else
        {
            playerCamera.fieldOfView = fov;
        }
    }

    private void Move()
    {
        if (isCrouching)
            return;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 wishDir = (transform.right * x + transform.forward * z).normalized;

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (wishDir.magnitude > 0)
        {
            Vector3 targetVelocity = wishDir * maxSpeed;
            Vector3 velocityDiff = targetVelocity - horizontalVelocity;

            Vector3 accel = Vector3.ClampMagnitude(velocityDiff, acceleration * Time.fixedDeltaTime);
            rb.AddForce(accel, ForceMode.VelocityChange);
        }
        else
        {
            Vector3 brake = Vector3.ClampMagnitude(-horizontalVelocity, deceleration * Time.fixedDeltaTime);
            rb.AddForce(brake, ForceMode.VelocityChange);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            // Adds force to the player rigidbody to jump
            if (isGrounded)
            {
                rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
                isGrounded = false;
            }
        }
    }

    private void HandleCrouch()
    {
        if (Input.GetKey(crouchKey))
        {
            isCrouching = true;
            crouchTimer += Time.deltaTime;
        }

        if (Input.GetKeyUp(crouchKey))
        {
            BoostBasedOnCharge();
            crouchTimer = 0f;
            isCrouching = false;
        }
    }

    private void BoostBasedOnCharge()
    {
        float chargePercent = Mathf.Clamp01(crouchTimer / maxCrouchChargeTime);

        if (chargePercent <= 0.05f)
            return;

        float boostSpeed = Mathf.Lerp(minBoostSpeed, maxBoostSpeed, chargePercent);

        Vector3 boostDirection = transform.forward;
        boostDirection.y = 0f;
        boostDirection.Normalize();

        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        rb.AddForce(boostDirection * boostSpeed, ForceMode.VelocityChange);
    }



    private bool CheckGround()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        //Debug.DrawRay(origin, Vector3.down * groundCheckDistance, Color.red);

        return Physics.Raycast(
            origin,
            Vector3.down,
            groundCheckDistance,
            groundLayer
        );
    }
}
