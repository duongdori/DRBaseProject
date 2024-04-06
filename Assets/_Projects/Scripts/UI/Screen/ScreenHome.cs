using UnityEngine;

public class ScreenHome : ScreenBase
{
    public void OnBtnPlayClick()
    {
        OnHide();
        GameManager.Instance.StartLevel();
        UIManager.Instance.ShowScreen<ScreenInGame>();
    }
}
