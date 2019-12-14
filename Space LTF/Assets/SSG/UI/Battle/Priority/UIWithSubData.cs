using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIWithSubData   : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UIWithSubDataInner ObjectToShow;
    public bool IsSubOpenNow = false;

    void Awake()
    {
        ObjectToShow.Init(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsSubOpenNow = true;
        ObjectToShow.gameObject.SetActive(true);
//        Debug.Log("Main enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var shallCloes = ObjectToShow.MainPointerExit();
//        Debug.Log("Main exit");
        IsSubOpenNow = false;
        if (shallCloes)
        {
            ObjectToShow.gameObject.SetActive(false);
        }
    }

    public void SubPoinetExit()
    {
        if (!IsSubOpenNow)
        {
            ObjectToShow.gameObject.SetActive(false);
        }
    }
}
