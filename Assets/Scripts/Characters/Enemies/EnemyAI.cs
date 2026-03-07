using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private static readonly int ChaseHash = Animator.StringToHash("Chase");

    [SerializeField] private string tagTarget = "Player";
    [SerializeField] public float detectionRadius = 10f;
    
    private NavMeshAgent _agent;
    private AnimationBehaviour _animationBehaviour;
    private Transform _target;
    private EnemyHealth _health;
    private bool _isDead;
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animationBehaviour = GetComponentInChildren<AnimationBehaviour>();
        _health = GetComponent<EnemyHealth>();
        
        _agent.updatePosition = true;
        _agent.updateRotation = true;

        _health.OnDeath += HandleDeath;
    }

    private void Start() => _target = GameObject.FindGameObjectWithTag(tagTarget).transform;

    private void Update()
    {
        if (_isDead) return;

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

    private void HandleDeath()
    {
        _isDead = true;
        _agent.isStopped = true;
        _agent.ResetPath();
        
        _animationBehaviour.SetBool(ChaseHash, false);
    }

    private void OnDisable() => _health.OnDeath -= HandleDeath;
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
