using System;
using DG.Tweening;
using DR.Utilities.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class PopupNointernet : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform panel;
    [SerializeField] private Button btnOk;
    [SerializeField] private bool isShow = false;
    public bool IsShow => isShow;
    
    private void Awake()
    {
        canvasGroup.SetActive(false);
        btnOk.onClick.AddListener(OnBtnOkClicked);
    }

    private void Update()
    {
        if (isShow)
        {
            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
                Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                OnHide();
            }
        }

        if (Application.internetReachability == NetworkReachability.NotReachable && !isShow)
        {
            OnShow();
        }
    }

    private void OnShow()
    {
        if (isShow) return;
        
        isShow = true;
        
        canvasGroup.SetActive(isShow);
        panel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutCubic);
    }

    private void OnHide()
    {
        if (!isShow) return;
        isShow = false;
 
        canvasGroup.DOFade(0f, 0.5f);
        panel.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            canvasGroup.SetActive(isShow);
        });
    }


    private void OnBtnOkClicked()
    {
        try
        {
#if UNITY_ANDROID
            using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (var intentObject = new AndroidJavaObject("android.content.Intent", "android.settings.WIFI_SETTINGS"))
                {
                    currentActivityObject.Call("startActivity", intentObject);
                }
            }
#elif UNITY_IOS
        OnHide();
#endif
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
