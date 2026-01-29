using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Scriptable Objects/PowerUp")]
public class PowerUp : ScriptableObject
{
    public string powerUpName;
    public PowerUpType type;
    [Tooltip("Amount of Health or Shield to add")]
    public int amount;
    [Tooltip("Duration for timed power ups like Speed or Invincibility")]
    public float duration;
    [Tooltip("Multiplier for Speed power up")]
    public float multiplier;
}
