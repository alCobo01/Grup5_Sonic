using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinMenu : BaseMenu
{
    [Header("Dependencies")]
    [SerializeField] private List<GameObject> uiToDisable;
    [SerializeField] private GameStats gameStats;

    [Header("Stats text to change")] 
    [SerializeField] private TMP_Text ringsText;
    [SerializeField] private TMP_Text enemiesText;
    [SerializeField] private TMP_Text timeText;
    
    public override void Open()
    {
        base.Open();
        uiToDisable.ForEach(u => u.SetActive(false));
        UpdateStats();
    }

    private void UpdateStats()
    {
        var time = TimeSpan.FromSeconds(gameStats.timeElapsed);
        timeText.text = $"{time.Minutes:D2}:{time.Seconds:D2}:{time.Milliseconds / 10:D2}";

        ringsText.text = gameStats.ringsCollected.ToString();
        enemiesText.text = gameStats.enemiesKilled.ToString();
    }
}
