using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AnimationBehaviour))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private static readonly int ChaseHash = Animator.StringToHash("Chase");
    
    private NavMeshAgent _agent;
    private AnimationBehaviour _animationBehaviour;
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animationBehaviour = GetComponent<AnimationBehaviour>();
    }

    private void Update()
    {
        if (_agent.velocity.magnitude != 0f) _animationBehaviour.SetBool(ChaseHash, true);
    }
}
