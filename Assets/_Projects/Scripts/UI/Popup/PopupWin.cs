using UnityEngine;

public class PopupWin : PopupBase
{
    public void OnBtnNextClick()
    {
        OnHide();
        UIManager.Instance.HideScreen<ScreenInGame>();
        UIManager.Instance.ShowScreen<ScreenHome>();
        GameManager.Instance.LoadCurrentLevel();
    }

  
}
