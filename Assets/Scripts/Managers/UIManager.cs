using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealthController playerHealth;
    [SerializeField] private TMP_Text ringsText;
    [SerializeField] private TMP_Text livesText;

    private void Start() => RefreshUI();
    
    private void OnEnable()
    {
        PlayerHealthController.OnRingsChanged += HandleRingsChanged;
        PlayerHealthController.OnLivesChanged += HandleLivesChanged;

        RefreshUI();
    }

    private void OnDisable()
    {
        PlayerHealthController.OnRingsChanged -= HandleRingsChanged;
        PlayerHealthController.OnLivesChanged -= HandleLivesChanged;
    }

    private void RefreshUI()
    {
        HandleRingsChanged(playerHealth.CurrentRings);
        HandleLivesChanged(playerHealth.CurrentLives);
    }

    private void HandleRingsChanged(int rings) => ringsText.text = rings.ToString();
    
    private void HandleLivesChanged(int lives) => livesText.text = lives.ToString();
        
    
}
