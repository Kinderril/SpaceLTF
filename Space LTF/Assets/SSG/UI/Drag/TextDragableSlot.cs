using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;


public  class TextDragableSlot : DragableItemSlot
{
    public Text Info;
    public override void VisualUpdate()
    {
        if (CurrentItem == null)
        {
            Info.text = "";
        }
        else
        {
            Info.text = CurrentItem.GetInfo();
        }
    }
}

