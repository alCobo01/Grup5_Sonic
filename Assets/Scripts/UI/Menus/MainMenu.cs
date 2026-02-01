using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("MainGame");
    }
    public void CloseGame()
    {
        Application.Quit();
        Debug.Log("Closed");
    }
}
