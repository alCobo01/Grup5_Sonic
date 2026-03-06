using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class NPC : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [SerializeField] private NPCDialogue dialogueWithoutGem;  // Diálogo si NO tiene la gema
    [SerializeField] private NPCDialogue dialogueWithGem;     // Diálogo si SÍ tiene la gema

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    private NPCDialogue _currentDialogue;
    private int _dialogueIndex;
    private bool _isTyping, _isDialogueActive;

    public void Interact(GameObject interactor)
    {
        if (_isDialogueActive)
            NextLine();
        else
            StartDialogue(interactor);
    }

    private void StartDialogue(GameObject interactor)
    {
        // Detectar si el jugador tiene la gema
        bool hasGem = false;
        if (interactor.TryGetComponent(out PlayerPowerUpController powerUpController))
            hasGem = powerUpController.HasKey;

        // Elegir el diálogo correspondiente
        _currentDialogue = hasGem ? dialogueWithGem : dialogueWithoutGem;

        // Fallback: si falta alguno de los dos, usar el que esté asignado
        if (_currentDialogue == null)
            _currentDialogue = dialogueWithGem ?? dialogueWithoutGem;

        if (_currentDialogue == null)
        {
            Debug.LogWarning($"[NPC] {gameObject.name} no tiene ningún NPCDialogue asignado.");
            return;
        }

        _isDialogueActive = true;
        _dialogueIndex = 0;

        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    private void NextLine()
    {
        if (_isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(_currentDialogue.dialogueLines[_dialogueIndex]);
            _isTyping = false;
        }
        else
        {
            _dialogueIndex++;
            if (_dialogueIndex < _currentDialogue.dialogueLines.Length)
                StartCoroutine(TypeLine());
            else
                EndDialogue();
        }
    }

    private IEnumerator TypeLine()
    {
        _isTyping = true;
        dialogueText.SetText("");

        foreach (var letter in _currentDialogue.dialogueLines[_dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(_currentDialogue.typingSpeed);
        }

        _isTyping = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out PlayerInputController player)) return;
        if (_isDialogueActive) EndDialogue();
    }

    private void EndDialogue()
    {
        StopAllCoroutines();
        _isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
    }
}