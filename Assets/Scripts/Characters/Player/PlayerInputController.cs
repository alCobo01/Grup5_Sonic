using static InputSystem_Actions;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInputController : MonoBehaviour, IPlayerActions
{
    public event UnityAction OnAttackEvent = delegate { };
    public event UnityAction OnInteractEvent = delegate { };
    
    private InputSystem_Actions _inputActions;
    
    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.SetCallbacks(this);
    }
    
    private void OnEnable() => _inputActions.Enable();
    private void OnDisable() => _inputActions.Disable();
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) OnAttackEvent.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed) OnInteractEvent?.Invoke();   
    }
}
