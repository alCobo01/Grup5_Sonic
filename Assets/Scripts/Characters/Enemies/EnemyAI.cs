using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private static readonly int ChaseHash = Animator.StringToHash("Chase");

    [SerializeField] private string tagTarget = "Player";
    [SerializeField] private float detectionRadius = 10f;
    
    private NavMeshAgent _agent;
    private AnimationBehaviour _animationBehaviour;
    private Transform _target;
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animationBehaviour = GetComponentInChildren<AnimationBehaviour>();
        
        _agent.updatePosition = true;
        _agent.updateRotation = true;
    }

    private void Start() => _target = GameObject.FindGameObjectWithTag(tagTarget).transform;

    private void Update()
    {
        var distance = Vector3.Distance(transform.position, _target.position);
        
        if (distance <= detectionRadius)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_target.position);
            
            _animationBehaviour.SetBool(ChaseHash, _agent.velocity.magnitude > 0.1f);
        }
        else
        {
            _agent.isStopped = true;
            _animationBehaviour.SetBool(ChaseHash, false);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
