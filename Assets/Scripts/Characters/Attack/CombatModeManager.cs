using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(PlayerAttackController))]

public class CombatModeManager : MonoBehaviour
{
    private static readonly int IsAimingHash = Animator.StringToHash("IsAiming");
    
    private PlayerInputController _inputController;
    private PlayerAttackController _attackController;
    //private AnimationBehaviour _animationBehaviour;
    
    private MeleeAttack _meleeAttack;
    private RangeAttack _rangeAttack;

    private void Awake()
    {
        _inputController = GetComponent<PlayerInputController>();
        _attackController = GetComponent<PlayerAttackController>();
       // _animationBehaviour = GetComponent<AnimationBehaviour>();
        
        _meleeAttack = GetComponent<MeleeAttack>();
        _rangeAttack = GetComponent<RangeAttack>();
    }

    private void OnEnable()
    {
        _inputController.OnAimEvent += HandleAim;
        _attackController.SetAttackStrategy(_meleeAttack);
    }

    private void HandleAim(bool isAiming)
    {
       // _animationBehaviour.SetBool(IsAimingHash, isAiming);
        
        switch (isAiming)
        {
            case true:
                _attackController.SetAttackStrategy(_rangeAttack);
                break;
            case false:
                _attackController.SetAttackStrategy(_meleeAttack);
                break;
        }
    }
}
