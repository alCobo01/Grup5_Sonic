using UnityEngine;

public class WinMenu : BaseMenu
{
    [SerializeField] private GameObject statsUI;
    
    public override void Open()
    {
        base.Open();
        statsUI.SetActive(false);
    }
}
