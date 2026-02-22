using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseMenu : MonoBehaviour
{
    public static event Action RestartCheckPoint = delegate { };
    public virtual void Open() => gameObject.SetActive(true);
    public void Close() => gameObject.SetActive(false);
    
    public void Restart()
    {
        Time.timeScale = 1f;
        RestartCheckPoint.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CloseGame()
    {
        RestartCheckPoint.Invoke();
        Application.Quit(); 
    }
}
   

   
