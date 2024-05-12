using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class PopupSetting : PopupAnim
{
    public Button btnSound;
    public Button btnVibrate;
    public Button btnFlash;
    public Button btnClose;

    public GameObject iconSoundOn;
    public GameObject iconSoundOff;

    public GameObject iconVibrateOn;
    public GameObject iconVibrateOff;

    public GameObject iconFlashOn;
    public GameObject iconFlashOff;

    private bool _isSoundOn = true;
    private bool _isVibrateOn = true;
    private bool _isFlash = true;

    public Action OnHideSettingCallback;
    
    
    public override void OnInit()
    {
        _isSoundOn = false;// DataManager.Instance.GetData<DataUser>().HasSound();
        _isVibrateOn = false; // DataManager.Instance.GetData<DataUser>().HasVibration();
        _isFlash = false; // DataManager.Instance.GetData<DataUser>().HasFlash();

        btnSound.onClick.AddListener(() =>
        {
            OnSoundButtonClick();
            AudioManager.Instance.PlaySound(AudioType.CLICK);
        }
        );


        btnVibrate.onClick.AddListener(() =>
        {
            OnVibrateButtonClick();
            AudioManager.Instance.PlaySound(AudioType.CLICK);
        });


        btnClose.onClick.AddListener(() =>
        {
            OnHide();
       
            AudioManager.Instance.PlaySound(AudioType.CLICK);
        });


        btnFlash.onClick.AddListener(() => {
            OnFlashButtonClick();
            AudioManager.Instance.PlaySound(AudioType.CLICK);
        });


        base.OnInit();
    }


    public void SubscribeCallback( Action callback )
    {
        OnHideSettingCallback += callback;
    }

    public override void OnShow(float fadeTime = 0, Ease ease = Ease.Linear, Action onComplete = null)
    {
        base.OnShow(fadeTime, ease, onComplete);
        Refresh();
    }

    private void Refresh()
    {
        iconSoundOn.SetActive(_isSoundOn);
        iconSoundOff.SetActive(!_isSoundOn);

        iconVibrateOn.SetActive(_isVibrateOn);
        iconVibrateOff.SetActive(!_isVibrateOn);

        iconFlashOn.SetActive(_isFlash);
        iconFlashOff.SetActive(!_isFlash);
    }


    private void OnVibrateButtonClick()
    {
        _isVibrateOn = !_isVibrateOn;
        DataManager.Instance.GetData<DataUser>().SetVibration(_isVibrateOn);
        Refresh();
    }


    private void OnSoundButtonClick()
    {
        _isSoundOn = !_isSoundOn;
        DataManager.Instance.GetData<DataUser>().SetSound(_isSoundOn);
        Refresh();
    }

    private void OnFlashButtonClick()
    {
        _isFlash = !_isFlash;
        //DataManager.Instance.GetData<DataUser>().SetFlash(isFlash);
        Refresh();
    }

    public override void OnHide(float fadeTime = 0, Ease ease = Ease.Linear, Action onComplete = null)
    {
        base.OnHide(fadeTime, ease, onComplete);
        OnHideSettingCallback?.Invoke();
        OnHideSettingCallback = null;
    }
}
