using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossBattle : MonoBehaviour
{
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
    
    [Header("Prefabs")] 
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject ringPrefab;

    private List<Vector3> _spawnPointsPositions = new();
    private List<EnemyHealth> _spawnedEnemies;
    private NavMeshAgent _agent;
    private Transform _target;
    private Coroutine _enemySpawnRoutine;
    private Stage _currentStage;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = true;
        _agent.updateRotation = true;
        
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

    #region handleEvents
    private void HandleDamage()
    {
        
    }
    
    private void HandleStartBattle()
    {
        StartBattle();
        BossBatlleTrigger.OnPlayerEnterTrigger -= HandleStartBattle;
    }

    private void HandleDeath()
    {
        DestroyAllEnemies();
    }
    #endregion
    
    private void StartBattle()
    {
        StartNextStage();
        for (var i = 0; i < ; i++)
        {
            
        }
    }
    
    private void SpawnEnemy()
    {
        var position = _spawnPointsPositions[UnityEngine.Random.Range(0, _spawnPointsPositions.Count)];
        var enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        
        if (enemy.TryGetComponent<EnemyHealth>(out var enemyHealth)) _spawnedEnemies.Add(enemyHealth);
    }

    private void StartNextStage()
    {
        _currentStage = _currentStage switch
        {
            Stage.WaitingToStart => Stage.Stage1,
            Stage.Stage1 => Stage.Stage2,
            Stage.Stage2 => Stage.Stage3,
            _ => _currentStage
        };
    }

    private void DestroyAllEnemies() => _spawnedEnemies.ForEach(e => e.Die());
    
}
