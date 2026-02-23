using static InputSystem_Actions;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInputController : MonoBehaviour, IPlayerActions
{
    public event UnityAction OnAttackEvent = delegate { };
    public event UnityAction OnInteractEvent = delegate { };
    public static event UnityAction OnPauseGameEvent;
    public event UnityAction<bool> OnAimEvent = delegate { };
    public event UnityAction<Vector2> OnMoveEvent = delegate { };
    public event UnityAction<Vector2> OnLookEvent = delegate { };
    public event UnityAction OnJumpEvent = delegate { };
    public event UnityAction OnCrouchHeldEvent = delegate { };
    public event UnityAction OnCrouchReleasedEvent = delegate { };

    private bool _crouchHeld;
    private InputSystem_Actions _inputActions;

    [SerializeField] private AudioClip _jumpClip;
    
    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.SetCallbacks(this);
    }
    
    private void OnEnable() => _inputActions.Enable();
    private void OnDisable() => _inputActions.Disable();

    private void Update()
    {
        if (_crouchHeld) OnCrouchHeldEvent.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) OnAttackEvent.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed) OnInteractEvent.Invoke();   
    }
    
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed) OnAimEvent.Invoke(true);
        if (context.canceled) OnAimEvent.Invoke(false);
    }

    public void OnPauseGame(InputAction.CallbackContext context)
    {
        if (context.performed) OnPauseGameEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context) => OnMoveEvent.Invoke(context.ReadValue<Vector2>());

    public void OnLook(InputAction.CallbackContext context) => OnLookEvent.Invoke(context.ReadValue<Vector2>());

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnJumpEvent.Invoke();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed) { _crouchHeld = true; }
        if (context.canceled)
        {
            _crouchHeld = false;
            OnCrouchReleasedEvent.Invoke();
        }
    }
}
