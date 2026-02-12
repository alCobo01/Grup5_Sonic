using UnityEngine.SceneManagement;

public class MainMenu : BaseMenu
{
    public void Play() => SceneManager.LoadScene("Main Scene");
}
