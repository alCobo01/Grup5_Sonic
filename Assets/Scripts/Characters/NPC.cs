using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private NPCDialogue dialogueData;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText, nameText;
    [SerializeField] private Image portraitImage;

    private int _dialogueIndex;
    private bool _isTyping, _isDialogueActive;
    
    public void Interact()
    {
        if (_isDialogueActive) 
            NextLine();
        else 
            StartDialogue();
    }

    private void StartDialogue()
    {
        _isDialogueActive = true;
        _dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    private void NextLine()
    {
        if (_isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[_dialogueIndex]);
            _isTyping = false;
        }
        else
        {
            _dialogueIndex++;
            if (_dialogueIndex < dialogueData.dialogueLines.Length)
                StartCoroutine(TypeLine());
            else
                EndDialogue();
        }
    }

    private IEnumerator TypeLine()
    {
        _isTyping = true;
        dialogueText.SetText("");

        foreach(char letter in dialogueData.dialogueLines[_dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        _isTyping = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent(out PlayerInputController player))
        {
            if (_isDialogueActive)
                EndDialogue();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        _isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
    }
}
