using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuObject;
    public static event Action RestartCheckPoint = delegate { };
    public static event Action<bool> PausePlayer = delegate { };

    public virtual void ShowMenu()
    {
        menuObject.SetActive(true);
        PausePlayer.Invoke(true);
        Time.timeScale = 0;
    }
    public virtual void Restart()
    {
        PausePlayer.Invoke(false);
        Time.timeScale = 1f;
        RestartCheckPoint.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    protected virtual void HideMenu() 
    {
        menuObject.SetActive(false);
        PausePlayer.Invoke(false);
        Time.timeScale = 1f;
    }
    public void CloseGame()
    {
        RestartCheckPoint.Invoke();
        Application.Quit();
    }
}
   

   
