using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : BaseMenu
{
    private bool gamePaused = false;
    public void TogglePause() 
    {
        if (gamePaused)
        { 
            HideMenu(); 
        }           
        else 
        { 
            ShowMenu();
        }
        gamePaused = !gamePaused; }
    private void OnEnable() { Player.UsePauseMenu += TogglePause; }
    private void OnDisable() { Player.UsePauseMenu -= TogglePause; }
}
