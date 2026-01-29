using UnityEngine;

[RequireComponent(typeof(AnimationBehaviour))]
public class RangeAttack : MonoBehaviour, IAttack
{
    private AnimationBehaviour _animationBehaviour;

    private void Awake()
    {
        _animationBehaviour = GetComponent<AnimationBehaviour>();
    }

    public void Attack() => _animationBehaviour.TriggerRangeAttack();
}
