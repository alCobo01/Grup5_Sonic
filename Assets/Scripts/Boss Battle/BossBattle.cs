using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BossBattle : MonoBehaviour
{
    public static event UnityAction OnBattleStarted;
    public static event UnityAction OnBattleEnded;
    public static event UnityAction<int, int> OnBossHealthChanged;  // currentLives, maxLives
    public static event UnityAction<Stage> OnStageChanged;
    
    private static readonly int LaughHash = Animator.StringToHash("Laugh");
    private static readonly int DieHash = Animator.StringToHash("Die");
    
    [Header("Dependencies")]
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] private EnemyHealth eggmanHealthController;
    [SerializeField] private string tagTarget = "Player";

    [Header("Stages lives settings")] 
    [SerializeField] private int stage2StartLives;
    [SerializeField] private int stage3StartLives;
    
    [Header("Stages speed settings")]
    [SerializeField] private float stage1Speed;
    [SerializeField] private float stage2Speed;
    [SerializeField] private float stage3Speed;

    [Header("Stages enemies quantity")] 
    [SerializeField] private int stage1EnemyAmount;
    [SerializeField] private int stage2EnemyAmount;
    [SerializeField] private int stage3EnemyAmount;

    [Header("Stages ring spawn quantity")]
    [SerializeField] private int stage2RingAmount = 5;
    [SerializeField] private int stage3RingAmount = 10;
    
    [Header("Prefabs")] 
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject ringPrefab;
    [SerializeField] private GameObject cartelPrefab;

    private const float StageTransitionDuration = 2f;
    private const float PathUpdateInterval = 0.2f;

    [Header("Invincibility Cooldown")]
    [SerializeField] private float invincibilityCooldown = 1.5f;
    
    [Header("Sprint Settings")]
    [SerializeField] private float sprintSpeedMultiplier = 2f;
    [SerializeField] private float sprintDuration = 1f;
    [SerializeField] private float sprintCooldownMin = 4f;
    [SerializeField] private float sprintCooldownMax = 8f;
    
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;
    
    [Header("Death Flicker")]
    [SerializeField] private float flickerDuration = 3f;
    [SerializeField] private float flickerStartSpeed = 0.2f;
    [SerializeField] private float flickerEndSpeed = 0.05f;
    
    private readonly List<Vector3> _spawnPointsPositions = new();
    private readonly List<EnemyHealth> _spawnedEnemies = new();
    private HealthBehaviour _eggmanHealth;
    private AnimationBehaviour _animationBehaviour;
    private NavMeshAgent _agent;
    private Transform _target;
    private Stage _currentStage;
    private bool _battleStarted;
    private bool _isTransitioning;
    private int _maxLives;
    private float _invincibilityTimer;
    private float _currentBaseSpeed;
    private Coroutine _sprintCoroutine;
    private float _pathUpdateTimer;

    private void Awake()
    { 
        _agent = GetComponent<NavMeshAgent>();
        _animationBehaviour = GetComponent < AnimationBehaviour>();
        _agent.updatePosition = true;
        _agent.updateRotation = false;
        _agent.stoppingDistance = 0f;
        _agent.autoBraking = false;
        
        _eggmanHealth = eggmanHealthController.GetComponent<HealthBehaviour>();
        _maxLives = _eggmanHealth.CurrentLives;
        _currentStage = Stage.WaitingToStart;
    }

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag(tagTarget).transform;
        spawnPoints.ForEach(s => _spawnPointsPositions.Add(s.transform.position));
        
        BossBatlleTrigger.OnPlayerEnterTrigger += HandleStartBattle;
        eggmanHealthController.OnDamaged += HandleDamage;
        eggmanHealthController.OnDeath += HandleDeath;
    }

    private void Update()
    {
        if (!_battleStarted) return;
        
        if (_invincibilityTimer > 0f)
        {
            _invincibilityTimer -= Time.deltaTime;
            if (_invincibilityTimer <= 0f && !_isTransitioning)
            {
                eggmanHealthController.IsInvulnerable = false;
            }
        }
        
        if (_isTransitioning) return;

        _pathUpdateTimer -= Time.deltaTime;
        if (_pathUpdateTimer <= 0f)
        {
            _pathUpdateTimer = PathUpdateInterval;
            _agent.SetDestination(_target.position);
        }

        RotateTowardsPlayer();
    }

    private void RotateTowardsPlayer()
    {
        var direction = _target.position - transform.position;
        direction.y = 0f;
        
        if (direction.sqrMagnitude < 0.01f) return;
        
        var targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        BossBatlleTrigger.OnPlayerEnterTrigger -= HandleStartBattle;
        
        eggmanHealthController.OnDamaged -= HandleDamage;
        eggmanHealthController.OnDeath -= HandleDeath;
    }

    #region handleEvents
    private void HandleDamage()
    {
        if (_isTransitioning) return;
        var currentLives = _eggmanHealth.CurrentLives;
        
        OnBossHealthChanged?.Invoke(currentLives, _maxLives);

        switch (_currentStage)
        {
            case Stage.Stage1 when currentLives <= stage2StartLives:
                StartCoroutine(TransitionToNextStage(stage2RingAmount));
                return;
            case Stage.Stage2 when currentLives <= stage3StartLives:
                StartCoroutine(TransitionToNextStage(stage3RingAmount));
                return;
        }
        
        eggmanHealthController.IsInvulnerable = true;
        _invincibilityTimer = invincibilityCooldown;
    }
    
    private void HandleStartBattle()
    {
        StartBattle();
        BossBatlleTrigger.OnPlayerEnterTrigger -= HandleStartBattle;
    }

    private void HandleDeath()
    {
        _battleStarted = false;
        StopSprintCycle();
        _agent.isStopped = true;
        _agent.ResetPath();
        
        DestroyAllEnemies();
        OnBossHealthChanged?.Invoke(0, _maxLives);
        OnBattleEnded?.Invoke();
        
        StartCoroutine(DeathFlickerRoutine());
    }
    #endregion

    private IEnumerator DeathFlickerRoutine()
    {
        var renderers = eggmanHealthController.GetComponentsInChildren<Renderer>();
        var elapsed = 0f;
        
        while (elapsed < flickerDuration)
        {
            var t = elapsed / flickerDuration;
            var flickerSpeed = Mathf.Lerp(flickerStartSpeed, flickerEndSpeed, t);
            
            foreach (var r in renderers) r.enabled = !r.enabled;
            
            yield return new WaitForSeconds(flickerSpeed);
            elapsed += flickerSpeed;
        }
        
        SpawnCartelOnTerrain();
        Destroy(eggmanHealthController.gameObject);
    }

    private IEnumerator SprintCycleRoutine()
    {
        while (true)
        {
            var cooldown = Random.Range(sprintCooldownMin, sprintCooldownMax);
            yield return new WaitForSeconds(cooldown);
            
            _agent.speed = _currentBaseSpeed * sprintSpeedMultiplier;
            
            yield return new WaitForSeconds(sprintDuration);
            
            _agent.speed = _currentBaseSpeed;
        }
    }

    private void StartBattle()
    {
        _animationBehaviour.Trigger(LaughHash);
        AudioManager.Instance.PlayEggmanLaugh(transform);
        
        OnBattleStarted?.Invoke();
        OnBossHealthChanged?.Invoke(_eggmanHealth.CurrentLives, _maxLives);

        StartCoroutine(WaitForStarting());
    }

    private IEnumerator WaitForStarting()
    {
        yield return new WaitForSeconds(3.3f);
        StartNextStage();
        _battleStarted = true;
    }
    
    private IEnumerator TransitionToNextStage(int ringAmount)
    {
        _isTransitioning = true;
        _invincibilityTimer = 0f;
        
        StopSprintCycle();
        
        _agent.isStopped = true;
        _agent.ResetPath();
        eggmanHealthController.IsInvulnerable = true;
        
        DestroyAllEnemies();
        SpawnRings(ringAmount);

        yield return new WaitForSeconds(StageTransitionDuration);
        
        // Start next stage
        eggmanHealthController.IsInvulnerable = false;
        _agent.isStopped = false;
        StartNextStage();
        
        _isTransitioning = false;
    }
    
    private void SpawnEnemies(int amount)
    {
        var shuffled = new List<Vector3>(_spawnPointsPositions);
        ShuffleList(shuffled);
        
        for (var i = 0; i < amount; i++)
        {
            var position = shuffled[i % shuffled.Count];
            var enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        
            if (enemy.TryGetComponent<EnemyHealth>(out var enemyHealth)) _spawnedEnemies.Add(enemyHealth);
            if (enemy.TryGetComponent<EnemyAI>(out var enemyAI)) enemyAI.detectionRadius = 40f;
        }
    }

    private void SpawnRings(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            var position = _spawnPointsPositions[Random.Range(0, _spawnPointsPositions.Count)];
            var offset = new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
            var ring = Instantiate(ringPrefab, position + offset, Quaternion.identity);

            if (ring.TryGetComponent<LostRing>(out var ringController))
            {
                ringController.lifeTime = 10f;
            }
        }
    }

    private void StartNextStage()
    {
        StopSprintCycle();
        
        switch (_currentStage)
        {
            case Stage.WaitingToStart:
                _currentStage = Stage.Stage1;
                _agent.speed = stage1Speed;
                SpawnEnemies(stage1EnemyAmount);
                break;
            case Stage.Stage1:
                _currentStage = Stage.Stage2;
                _agent.speed = stage2Speed;
                SpawnEnemies(stage2EnemyAmount);
                break;
            case Stage.Stage2:
                _currentStage = Stage.Stage3;
                _agent.speed = stage3Speed;
                SpawnEnemies(stage3EnemyAmount);
                break;
        }
        
        _currentBaseSpeed = _agent.speed;
        _sprintCoroutine = StartCoroutine(SprintCycleRoutine());
        
        OnStageChanged?.Invoke(_currentStage);
    }

    private void StopSprintCycle()
    {
        if (_sprintCoroutine != null)
        {
            StopCoroutine(_sprintCoroutine);
            _sprintCoroutine = null;
        }
        _agent.speed = _currentBaseSpeed;
    }
    
    private void DestroyAllEnemies()
    {
        for (var i = _spawnedEnemies.Count - 1; i >= 0; i--)
        {
            var enemy = _spawnedEnemies[i];
            if (enemy != null && enemy.gameObject != null) enemy.Die();
        }
        _spawnedEnemies.Clear();
    }

    private void SpawnCartelOnTerrain()
    {
        var origin = transform.position + Vector3.up * 10f;
        
        if (Physics.Raycast(origin, Vector3.down, out var hit, 50f, LayerMask.GetMask("Ground")))
        {
            Instantiate(cartelPrefab, hit.point, Quaternion.Euler(-90, 0, 0));
        }
    }
    
    private static void ShuffleList<T>(List<T> list)
    {
        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
