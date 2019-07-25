using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public   abstract class AbstractBaseInfoUI :MonoBehaviour
{
     private Action _closeCallback;
//    public GanvasGrou

     protected void Init(Action closeCallback)
    {
        gameObject.SetActive(true);
        _closeCallback = closeCallback;
     }

    public void OnCloseClick()
    {
        Dispose();
        gameObject.SetActive(false);
        _closeCallback();
    }


    public virtual void Dispose()
    {
    }

}
