using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public abstract class UIElementWithTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip toolTip = DataBaseController.Instance.Pool.GetToolTip();
        toolTip.Init(TextToTooltip(), gameObject);
        OnPointEnterSub();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DataBaseController.Instance.Pool.CloseTooltip();
        OnPointExitSub();
    }

    protected virtual void OnPointExitSub()
    {

    } 
    protected virtual void OnPointEnterSub()
    {

    }

    protected abstract string TextToTooltip();
}
