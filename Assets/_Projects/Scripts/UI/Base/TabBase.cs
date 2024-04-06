using DR.Utilities.Extensions;
using UnityEngine;

public class TabBase : MonoBehaviour
{
    #region CONST
    #endregion

    #region EDITOR PARAMS
    [SerializeField]
    private CanvasGroup groupTab;
    #endregion

    #region PARAMS    
    #endregion

    #region PROPERTIES
    #endregion

    #region EVENTS
    #endregion

    #region METHODS
    public virtual void OnInit()
    {

    }

    public virtual void OnShowTab()
    {
        groupTab.SetActive(true);
    }

    public virtual void OnHideTab()
    {
        groupTab.SetActive(false);
    }
    
#if UNITY_EDITOR
    public void OnValidate()
    {
        if (groupTab != null) return;
        
        groupTab = GetComponent<CanvasGroup>();
        if (groupTab == null)
        {
            groupTab = gameObject.AddComponent<CanvasGroup>();
        }
    }
#endif
    #endregion

    #region DEBUG
    #endregion
}
