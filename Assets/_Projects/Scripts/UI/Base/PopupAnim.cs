using DG.Tweening;
using UnityEngine;

public class PopupAnim : PopupBase
{
    [SerializeField] protected RectTransform panel;

    public override void OnShow()
    {
        base.OnShow();
        
        popupRect.SetParent(UIManager.Instance.transform);
        
        UIManager.Instance.ShowOverlay();
        
        SetCenterAnchor();

        panel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutCubic);
    }

    public override void OnHide()
    {
        UIManager.Instance.HideOverlay();

        panel.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            base.OnHide();
            popupRect.SetParent(SafeArea.Instance.transform);
            SetCenterAnchor();
        });
    }
}
