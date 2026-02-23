using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealthController playerHealth;
    [SerializeField] private TMP_Text ringsText;
    [SerializeField] private TMP_Text livesText;
    
    private void OnEnable()
    {
        PlayerHealthController.OnRingsChanged += HandleRingsChanged;
        PlayerHealthController.OnLivesChanged += HandleLivesChanged;
    }

    private void OnDisable()
    {
        PlayerHealthController.OnRingsChanged -= HandleRingsChanged;
        PlayerHealthController.OnLivesChanged -= HandleLivesChanged;
    }
    
    private void HandleRingsChanged(int rings)
    {
        ringsText.text = rings.ToString();
        ringsText.color = (rings <= 0) ? Color.red : Color.white;
    }
    
    private void HandleLivesChanged(int lives) => livesText.text = lives.ToString();
}
