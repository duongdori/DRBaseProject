using System;
using System.Collections.Generic;
using DR.Utilities.Extensions;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    #region Const paramters
    #endregion

    #region Editor paramters
    [SerializeField] private ScreenBase[] arrScreens = Array.Empty<ScreenBase>();
    private Dictionary<string, ScreenBase> _dicScreens = new();
    
    [SerializeField] private PopupBase[] arrPopups = Array.Empty<PopupBase>();
    private Dictionary<string, PopupBase> _dicPopups = new();
    
    [SerializeField] private CanvasGroup overlay;
    #endregion

    #region Normal parameters
    #endregion

    #region Encapsulate
    #endregion


    public void OnInit()
    {
        foreach (var screen in arrScreens)
        {
            var screenName = screen.GetName();

            if (!_dicScreens.TryAdd(screenName, screen))
            {
                Debug.LogError("Invalid screen name " + screenName + " of gameObject " + screen.gameObject.name);
            }
        }

        foreach (var screen in arrScreens)
        {
            screen.OnInit();
        }

        foreach (var popup in arrPopups)
        {
            var popupName = popup.GetName();
            _dicPopups.TryAdd(popupName, popup);
        }

        foreach (var popup in arrPopups)
        {
            popup.OnInit();
        }
    }


    public void OnRelease()
    {
        for (int i = 0; i < arrScreens.Length; i++)
        {
            arrScreens[i].OnRelease();
        }
    }

    public T ShowScreen<T>() where T : ScreenBase
    {
        var screenName = typeof(T).FullName;

        if (_dicScreens.ContainsKey(screenName))
        {
            _dicScreens[screenName].OnShow();
            return _dicScreens[screenName] as T;
        }

        Debug.LogError("Invalid screen " + screenName);
        return null;
    }

    public T HideScreen<T>() where T : ScreenBase
    {
        var screenName = typeof(T).FullName;

        if (_dicScreens.ContainsKey(screenName))
        {
            _dicScreens[screenName].OnHide();
            return _dicScreens[screenName] as T;
        }

        Debug.LogError("Invalid screen " + screenName);
        return null;
    }

    public T GetScreen<T>() where T : ScreenBase
    {
        var screenName = typeof(T).FullName;

        if (_dicScreens.TryGetValue(screenName, out var screen))
        {
            return screen as T;
        }

        Debug.LogError("Invalid screen " + screenName);
        return null;
    }

    public T ShowPopup<T>() where T : PopupBase
    {
        var popupName = typeof(T).FullName;

        if (!_dicPopups.ContainsKey(popupName)) return null;
        
        _dicPopups[popupName].OnShow();
        return _dicPopups[popupName] as T;
    }

    public T HidePopup<T>() where T : PopupBase
    {
        var popupName = typeof(T).FullName;

        if (!_dicPopups.ContainsKey(popupName)) return null;
        
        _dicPopups[popupName].OnHide();
        return _dicPopups[popupName] as T;
    }

    public T GetPopup<T>() where T : PopupBase
    {
        var popupName = typeof(T).FullName;

        if (_dicPopups.TryGetValue(popupName, out var popup))
        {
            return popup as T;
        }

        return null;
    }
    
    public void ShowOverlay()
    {
        overlay.SetActive(true);
    }
    
    public void HideOverlay()
    {
        overlay.SetActive(false, 0.3f);
    }

#if UNITY_ANDROID
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for (int i = arrPopups.Length - 1; i >= 0; i--)
            {
                if (arrPopups[i].OnBack())
                    return;
            }

            for (int i = arrScreens.Length - 1; i >= 0; i--)
            {
                if (arrScreens[i].OnBack())
                    return;
            }
        }
    }
#endif

#if UNITY_EDITOR
    public void OnValidate()
    {
        arrScreens = gameObject.GetComponentsInChildren<ScreenBase>();
        arrPopups = gameObject.GetComponentsInChildren<PopupBase>();
        overlay = transform.Find("Overlay").GetComponent<CanvasGroup>();
    }
#endif


}
