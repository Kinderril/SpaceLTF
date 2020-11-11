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

    private IItemInv _iitem = null;
    protected void Init(Action closeCallback, IItemInv iitem)
    {
        _iitem = iitem;
        gameObject.SetActive(true);
        _closeCallback = closeCallback;
    }
     public void OnRemoveItem()
     {
         WindowManager.Instance.ConfirmWindow.Init(OnOkRemove, null, Namings.Tag("wantRemove"));
     }

     private void OnOkRemove()
     {
         if (_iitem != null)
         {
             InventoryOperation.RemoveItemFromSelfInventory(_iitem);
             OnCloseClick();
         }
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
