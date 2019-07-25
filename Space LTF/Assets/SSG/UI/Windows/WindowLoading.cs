using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class WindowLoading : BaseWindow
{
    public override void Init<T>(T obj)
    {
        Action callback = obj as Action;
        if (callback != null)
        {
            callback();
        }
        else
        {
            Debug.LogError("Wrong init window loading");
        }

        base.Init(obj);
    }
}

