using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private NPCDialogue dialogueData;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText, nameText;
    [SerializeField] private Image portraitImage;
    
    public void Interact()
    {
        Debug.Log("NPC Interacted With");
    }
}