using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIWithSubDataInner    : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _isOpen = false;
    private UIWithSubData _data;

    public void Init(UIWithSubData data)
    {
        _data = data;
        gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isOpen = true;
//        Debug.Log("Sub enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isOpen = false;
//        Debug.Log("Sub exit");
        _data.SubPoinetExit();
    }

    public bool MainPointerExit()
    {
        return !_isOpen;

    }
}

