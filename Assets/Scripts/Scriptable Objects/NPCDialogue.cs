using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "Scriptable Objects/NPCDialogue")]
public class NPCDialogue : ScriptableObject
{
    public string[] dialogueLines;
    public float typingSpeed = 0.05f;
}
