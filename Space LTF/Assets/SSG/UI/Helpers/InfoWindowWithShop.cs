using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class InfoWindowWithShop : InfoWindow
{
    public override void Init(Action onOK,string msg)
    {
        transform.SetAsLastSibling();
        base.Init(onOK,msg);
    }

    public void OnClickBank()
    {
        gameObject.SetActive(false);
        WindowManager.Instance.CurrentWindow.OnToPay();
    }
}

