using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private static readonly int ChaseHash = Animator.StringToHash("Chase");

    [Header("Target")]
    [SerializeField] private string tagTarget = "Player";
    
    [Header("Detection")]
    [SerializeField] public float detectionRadius = 10f;
    [SerializeField] private float chaseStopBuffer = 0.25f;
    
    [Header("Patrol (Wander)")]
    [Tooltip("Si es true, el enemigo patrulla aleatoriamente cuando no detecta al jugador.")]
    [SerializeField] private bool enablePatrol = true;
    [Tooltip("Radio maximo desde la posicion inicial en el que el enemigo puede elegir puntos de patrulla.")]
    [SerializeField] private float wanderRadius = 8f;
    [Tooltip("Tiempo de espera (en segundos) en cada punto de patrulla antes de elegir uno nuevo.")]
    [SerializeField] private float wanderWaitTime = 2f;
    [Tooltip("Velocidad del NavMeshAgent durante la patrulla (se restaura la velocidad original al perseguir).")]
    [SerializeField] private float patrolSpeed = 1.5f;

    private NavMeshAgent _agent;
    private AnimationBehaviour _animationBehaviour;
    private Transform _target;
    private EnemyHealth _health;
    private bool _isDead;

    // Patrol state
    private Vector3 _spawnPosition;
    private float _wanderTimer;
    private float _originalSpeed;
    private bool _isPatrolling;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animationBehaviour = GetComponentInChildren<AnimationBehaviour>();
        _health = GetComponent<EnemyHealth>();

        _agent.updatePosition = true;
        _agent.updateRotation = true;

        _health.OnDeath += HandleDeath;
    }

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag(tagTarget).transform;
        _spawnPosition = transform.position;
        _originalSpeed = _agent.speed;
        _wanderTimer = 0f;
    }

    private void Update()
    {
        if (_isDead) return;

        var distance = Vector3.Distance(transform.position, _target.position);

        if (distance <= detectionRadius)
        {
            Chase(distance);
        }
        else
        {
            if (enablePatrol)
                Patrol();
            else
                Idle();
        }
    }

    private void Chase(float distance)
    {
        _isPatrolling = false;
        _agent.speed = _originalSpeed;
        _agent.isStopped = false;
        _agent.SetDestination(_target.position);

        var shouldChase = distance > _agent.stoppingDistance + chaseStopBuffer;
        _animationBehaviour.SetBool(ChaseHash, shouldChase);
    }

    private void Patrol()
    {
        _agent.speed = patrolSpeed;

        // Si ya llego al destino (o no tiene destino), esperar y luego elegir un nuevo punto
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance + 0.1f)
        {
            _animationBehaviour.SetBool(ChaseHash, false);
            _isPatrolling = false;
            
            _wanderTimer += Time.deltaTime;
            if (_wanderTimer >= wanderWaitTime)
            {
                _wanderTimer = 0f;
                SetRandomWanderDestination();
            }
        }
        else if (_isPatrolling)
        {
            // Esta en camino hacia un punto de patrulla: usar animacion de movimiento
            _animationBehaviour.SetBool(ChaseHash, true);
        }
    }

    private void Idle()
    {
        _agent.isStopped = true;
        _animationBehaviour.SetBool(ChaseHash, false);
    }

    private void SetRandomWanderDestination()
    {
        var randomDirection = UnityEngine.Random.insideUnitSphere * wanderRadius;
        randomDirection += _spawnPosition;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            _agent.isStopped = false;
            _agent.SetDestination(hit.position);
            _isPatrolling = true;
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

        // Dibujar radio de patrulla en amarillo
        if (enablePatrol)
        {
            Gizmos.color = Color.yellow;
            var center = Application.isPlaying ? _spawnPosition : transform.position;
            Gizmos.DrawWireSphere(center, wanderRadius);
        }
    }
}
