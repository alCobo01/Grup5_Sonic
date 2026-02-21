using UnityEngine;

public class GameOverMenu : BaseMenu
{
    [SerializeField] private GameObject statsUI;
    
    public override void Open()
    {
        base.Open();
        statsUI.SetActive(false);
    }
}
