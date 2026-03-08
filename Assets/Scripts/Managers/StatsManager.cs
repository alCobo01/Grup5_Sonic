using System;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private GameStats gameStats;
    private bool _isTimeRunning;
    
    private void Awake()
    {
        gameStats.ResetStats();
        StartTimer();
    }

    private void OnEnable()
    {
        EnemyHealth.OnDeathStat += HandleEnemyDeath;
        PlayerHealthController.OnRingsChanged += HandleRingCollected;
        WinZoneTrigger.OnWin += StopTimer;
    }

    private void OnDisable()
    {
        EnemyHealth.OnDeathStat -= HandleEnemyDeath;
        PlayerHealthController.OnRingsChanged -= HandleRingCollected;
        WinZoneTrigger.OnWin -= StopTimer;
    }

    private void Update()
    {
        if (_isTimeRunning) gameStats.timeElapsed += Time.deltaTime;
    }
    
    private void HandleEnemyDeath() => gameStats.enemiesKilled += 1;
    private void HandleRingCollected(int amount) => gameStats.ringsCollected += 1;
    
    private void StartTimer() => _isTimeRunning = true;
    private void StopTimer() => _isTimeRunning = false;
}