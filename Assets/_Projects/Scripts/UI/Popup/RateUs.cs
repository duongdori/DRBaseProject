using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RateUs : PopupAnim
{
    [SerializeField] private Button[] btnArray = Array.Empty<Button>();
    [SerializeField] private Image[] imgArray = Array.Empty<Image>();
    [SerializeField] private Sprite goldStar;
    [SerializeField] private Sprite silverStar;
    [SerializeField] private string androidId;
    [SerializeField] private string iosId;
    public Button btnRate;
    
    private int _rateCount;
    private bool _isRateUs;
    
    private void Start()
    {
        _isRateUs = DataManager.Instance.GetData<DataUser>().HasRateUs();
        
        // UIManager.onRate += OnShow;
        for (int i = 0; i < btnArray.Length; i++)
        {
            int starIndex = i;
            var btn = btnArray[starIndex];

            btn.onClick.AddListener(() =>
            {
                OnChooseStar(starIndex);
            });
        }
        btnRate.onClick.AddListener(() =>
       {
           AudioManager.Instance.PlaySound(AudioType.CLICK);
           RateForUs(_rateCount);
       });

#if UNITY_ANDROID
        androidId = Application.identifier;
#endif
    }

    public override void OnShow(float fadeTime = 0, Ease ease = Ease.Linear, Action onComplete = null)
    {
        if(_isRateUs) return;
        _isRateUs = true;
        
        DataManager.Instance.GetData<DataUser>().SetRateUs(_isRateUs);
        
        base.OnShow(fadeTime, ease, onComplete);
        
        _rateCount = 0;

        for (int i = 0; i < imgArray.Length; i++)
        {
            imgArray[i].sprite = silverStar;
        }
        btnRate.interactable = false;
    }

    private void OnChooseStar(int star)
    {
        _rateCount = star;
        StartCoroutine(I_Choose());
    }

    private IEnumerator I_Choose()
    {
        for (int i = 0; i < imgArray.Length; i++)
        {
            imgArray[i].sprite = i <= _rateCount ? goldStar : silverStar;
            
            yield return new WaitForSeconds(0.1f);
        }

        // for (int i = 0; i < rateCount + 1; i++)
        // {
        //     imgArray[i].sprite = goldStar;
        // }
        
        btnRate.interactable = true;
    }

    public void RateForUs(int rateCount)
    {
        _rateCount = rateCount;
        StartCoroutine(I_Rate(rateCount));
    }

    private IEnumerator I_Rate(int rateCount)
    {
        float delay = rateCount * 0.1f + 0.5f;

        // PlayerPrefs.SetInt("rate", 1);

        if (rateCount >= 4)
        {
#if UNITY_ANDROID
            Application.OpenURL("market://details?id=" + androidId);
#elif UNITY_IPHONE
            Application.OpenURL("itms-apps://itunes.apple.com/app/id"+iosId);
#endif

        }
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(0.15f);
        OnHide();
    }
}
