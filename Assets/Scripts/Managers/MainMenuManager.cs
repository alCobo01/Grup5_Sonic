using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(2560, 1440, FullScreenMode.MaximizedWindow);
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }
}