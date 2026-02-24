using UnityEngine;

[CreateAssetMenu(fileName = "GameStats", menuName = "Scriptable Objects/GameStats")]
public class GameStats : ScriptableObject 
{
    public int enemiesKilled;
    public int ringsCollected;
    public float timeElapsed;

    public void ResetStats() {
        enemiesKilled = 0;
        ringsCollected = 0;
        timeElapsed = 0;
    }
}