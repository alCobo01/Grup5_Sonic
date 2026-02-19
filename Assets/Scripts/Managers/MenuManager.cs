using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private List<BaseMenu> menus = new();
    private bool _isGamePaused;

    private void Awake()
    {
        Instance = this;
    }

    //Subscribe and desubscribe from events
    private void OnEnable()
    {
        PlayerHealthController.OnDeath += HandleDeath;
        PlayerInputController.OnPauseGameEvent += HandlePause;
    } 

    private void OnDisable()
    {
        PlayerHealthController.OnDeath -= HandleDeath;
        PlayerInputController.OnPauseGameEvent -= HandlePause;
    } 

    // Generic show and close menu methods
    public void ShowMenu<T>() where T : BaseMenu
    {
        var menu = menus.Find(menu => menu is T);
        UpdateTimeScale(true);
        menu.Open();
    }

    public void CloseMenu<T>() where T : BaseMenu
    {
        var menu = menus.Find(menu => menu is T);
        UpdateTimeScale(false);
        menu.Close();
    }
    
    //Handle menus state
    private void HandlePause()
    {
        if (!_isGamePaused)
        {
            ShowMenu<PauseMenu>();
            _isGamePaused = true;
        }
        else
        {
            CloseMenu<PauseMenu>();
            _isGamePaused = false;
        }
    }

    private void HandleDeath() => ShowMenu<GameOverMenu>();
    
    private static void UpdateTimeScale(bool shouldPause)
    {
        Time.timeScale = shouldPause ? 0f : 1f;
        Cursor.lockState = shouldPause ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = shouldPause;
    }
}