using DR.Utilities.Extensions;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenBase : MonoBehaviour
{
    #region CONST
    #endregion

    #region EDITOR PARAMS
    [SerializeField] protected CanvasGroup canvasGroup;
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
        this.isShowing = true;
    }

    public virtual void OnHide()
    {
        canvasGroup.SetActive(false);
        this.isShowing = false;
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

#if UNITY_EDITOR
#endif
    #endregion

    #region DEBUG
    #endregion
}
