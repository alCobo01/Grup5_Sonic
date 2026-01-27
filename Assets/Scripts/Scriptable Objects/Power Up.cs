using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Scriptable Objects/PowerUp")]
public class PowerUp : ScriptableObject
{
    public string name;
    public Sprite hudIcon;
    public GameObject prefab;
    public AudioClip pickUpSound;


}
