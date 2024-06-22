using UnityEngine;
using UnityEngine.UI;
using DRG.Audio;
using AudioType = DRG.Audio.AudioType;

public class PopupConfirm : PopupAnim
{
    [SerializeField] private Button btnNo;
    [SerializeField] private Button btnYes;

    public override void OnInit()
    {
        btnNo.onClick.AddListener(OnBtnNoClick);
        btnYes.onClick.AddListener(OnBtnYesClick);
        base.OnInit();
    }

    private void OnBtnNoClick()
    {
        AudioManager.Instance.PlaySound(AudioType.BUTTON_CLICK);
        OnHide();
    }
    private void OnBtnYesClick()
    {
        AudioManager.Instance.PlaySound(AudioType.BUTTON_CLICK);
        OnHide();

        GameManager.Instance.BackToHome();
    }
}
