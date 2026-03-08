using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject container;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;

    [Header("Stage Colors")]
    [SerializeField] private Color stage1Color = Color.green;
    [SerializeField] private Color stage2Color = Color.yellow;
    [SerializeField] private Color stage3Color = Color.red;

    private void OnEnable()
    {
        BossBattle.OnBattleStarted += HandleBattleStarted;
        BossBattle.OnBattleEnded += HandleBattleEnded;
        BossBattle.OnBossHealthChanged += HandleHealthChanged;
        BossBattle.OnStageChanged += HandleStageChanged;
        PlayerHealthController.OnDeath += HandleGameOver;
    }

    private void OnDisable()
    {
        BossBattle.OnBattleStarted -= HandleBattleStarted;
        BossBattle.OnBattleEnded -= HandleBattleEnded;
        BossBattle.OnBossHealthChanged -= HandleHealthChanged;
        BossBattle.OnStageChanged -= HandleStageChanged;
        PlayerHealthController.OnDeath -= HandleGameOver;
    }

    private void Start()
    {
        container.SetActive(false);
    }

    private void HandleBattleStarted()
    {
        container.SetActive(true);
    }

    private void HandleBattleEnded()
    {
        container.SetActive(false);
    }

    private void HandleGameOver()
    {
        container.SetActive(false);
    }

    private void HandleHealthChanged(int currentLives, int maxLives)
    {
        healthSlider.maxValue = maxLives;
        healthSlider.value = currentLives;
    }

    private void HandleStageChanged(Stage stage)
    {
        fillImage.color = stage switch
        {
            Stage.Stage1 => stage1Color,
            Stage.Stage2 => stage2Color,
            Stage.Stage3 => stage3Color,
            _ => stage1Color
        };
    }
}
