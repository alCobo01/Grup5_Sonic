using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Scriptable Objects/PowerUp")]
public class PowerUp : ScriptableObject
{
    public PowerUpType type;
    [Tooltip("Amount of Health or Shield to add")]
    public int amount;
    [Tooltip("Duration for timed power ups like Speed or Invincibility")]
    public float duration;

    [Header("Audio")]
    [Tooltip("Sonido que se reproduce al recoger este power up")]
    public string sfxName;
}