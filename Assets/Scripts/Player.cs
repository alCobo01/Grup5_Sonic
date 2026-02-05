using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Miquel is tha best :3")]
    [Header("Movement")]
    public float speed = 6f;

    [Header("Acceleration")]
    public float maxSpeed = 8f;
    public float acceleration = 20f;
    public float deceleration = 25f;


    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;
    public bool invertY = false;

    [Header("References")]
    public Camera playerCamera;

    private Rigidbody rb;
    private float yaw;
    private float pitch;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Evita que el Rigidbody rote o vuele
        rb.freezeRotation = true;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Look();
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
    }

    private void Move()
    {
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
            // Frenado suave
            Vector3 brake = Vector3.ClampMagnitude(-horizontalVelocity, deceleration * Time.fixedDeltaTime);
            rb.AddForce(brake, ForceMode.VelocityChange);
        }
    }

}
