using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public abstract class UIElementWithTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip toolTip = DataBaseController.Instance.Pool.GetToolTip();
        toolTip.Init(TextToTooltip(), gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DataBaseController.Instance.Pool.CloseTooltip();
    }

    protected abstract string TextToTooltip();
}
