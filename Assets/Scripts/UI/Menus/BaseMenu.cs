using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseMenu : MonoBehaviour
{
    public virtual void Open() => gameObject.SetActive(true);
    public virtual void Close() => gameObject.SetActive(false);
    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    public void CloseGame() => Application.Quit();
}
   

   
