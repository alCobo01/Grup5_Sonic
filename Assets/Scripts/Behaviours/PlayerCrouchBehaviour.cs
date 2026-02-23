using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputController))]
public class PlayerCrouchBehaviour : MonoBehaviour
{
    [Header("Crouch & Boost")]
    [SerializeField] private float maxCrouchChargeTime = 4f;
    [SerializeField] private float minBoostSpeed = 2f;
    [SerializeField] private float maxBoostSpeed = 40f;

    public bool IsCrouching { get; private set; }

    private Rigidbody _rb;
    private PlayerInputController _input;
    private float _crouchTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInputController>();

        _input.OnCrouchHeldEvent += HandleCrouchHeld;
        _input.OnCrouchReleasedEvent += HandleCrouchReleased;
    }

    private void OnDestroy()
    {
        _input.OnCrouchHeldEvent -= HandleCrouchHeld;
        _input.OnCrouchReleasedEvent -= HandleCrouchReleased;
    }

    private void HandleCrouchHeld()
    {
        IsCrouching = true;
        _crouchTimer += Time.deltaTime;
    }

    private void HandleCrouchReleased()
    {
        BoostBasedOnCharge();
        AudioManager.Instance.PlayBoost(transform);
        _crouchTimer = 0f;
        IsCrouching = false;
    }

    private void BoostBasedOnCharge()
    {
        float chargePercent = Mathf.Clamp01(_crouchTimer / maxCrouchChargeTime);
        if (chargePercent <= 0.05f) return;

        float boostSpeed = Mathf.Lerp(minBoostSpeed, maxBoostSpeed, chargePercent);
        Vector3 direction = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;

        _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
        _rb.AddForce(direction * boostSpeed, ForceMode.VelocityChange);
    }
}