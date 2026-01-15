using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
public class PlayerAttackController : MonoBehaviour
{
    private PlayerInputController _inputController;
    
    private void Awake()
    {
        _inputController = GetComponent<PlayerInputController>();
        _inputController.OnAttackEvent += HandleAttack;
    }
    
    private void HandleAttack()
    {
        Debug.Log("Player Attack Triggered");
    }
}