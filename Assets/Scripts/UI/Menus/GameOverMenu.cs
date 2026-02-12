using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : BaseMenu
{
    private void OnEnable() 
    {
        Player.UseGameOverMenu += ShowMenu;
    }
    private void OnDisable() 
    {
        Player.UseGameOverMenu -= ShowMenu;
    }
    public void TryAgain() 
    {
        Restart();
        HideMenu();
    }
}
