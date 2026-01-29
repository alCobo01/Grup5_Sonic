using UnityEngine;

[RequireComponent(typeof(AnimationBehaviour))]
public class MeleeAttack : MonoBehaviour, IAttack
{
    private AnimationBehaviour _animationBehaviour;

    private void Awake()
    {
        _animationBehaviour = GetComponent<AnimationBehaviour>();
    }

    public void Attack() => _animationBehaviour.TriggerMeleeAttack();
}
