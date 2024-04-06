using DR.Utilities.Extensions;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PopupBase : MonoBehaviour
{
    #region CONST
    #endregion

    #region EDITOR PARAMS
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] protected RectTransform popupRect;
    [SerializeField] protected Animator uiAnimator;

    #endregion

    #region PARAMS
    [SerializeField] protected bool isShowing;
    #endregion

    #region PROPERTIES
    public bool IsShowing => isShowing;
    #endregion

    #region EVENTS
    #endregion

    #region METHODS
    public virtual void OnInit()
    {
        OnHide();
    }


    public virtual void OnShow()
    {
        canvasGroup.SetActive(true);
        isShowing = true;
    }


    public virtual void OnHide()
    {
        canvasGroup.SetActive(false);
        isShowing = false;
    }

    public virtual void OnRelease()
    {

    }

    public virtual bool OnBack()
    {
        return false;
    }

    public virtual string GetName()
    {
        return this.GetType().FullName;
    }
    
    protected void SetCenterAnchor()
    {
        popupRect.anchorMin = Vector2.zero;
        popupRect.anchorMax = Vector2.one;
        popupRect.pivot = new Vector2(0.5f, 0.5f);
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        // if (canvas == null)
        // {
        //     canvas = gameObject.GetComponent<Canvas>();
        //     if (canvas == null)
        //     {
        //         canvas = gameObject.AddComponent<Canvas>();
        //     }
        // }

        // if (graphicRaycaster == null)
        // {
        //     graphicRaycaster = gameObject.GetComponent<GraphicRaycaster>();
        //     if (graphicRaycaster == null)
        //     {
        //         graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
        //     }
        // }
    }
#endif
    #endregion

    #region DEBUG
    #endregion
}
