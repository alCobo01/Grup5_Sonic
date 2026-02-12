using UnityEngine;
using System;

public class HealthBehaviour : MonoBehaviour
{
    [SerializeField] private int startingLives = 3;

    //Class properties
    public int CurrentLives { get; private set; }
    public bool IsDead => CurrentLives <= 0;

    private void Awake()
    {
        CurrentLives = startingLives;
    }

    public void LoseLife()
    {
        CurrentLives = Mathf.Max(0, CurrentLives - 1);
    }
}
