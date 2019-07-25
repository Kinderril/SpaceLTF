using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public interface EffectConnecter
{
    void SetCallbackUpdate(Action<GameObject> objectMoveAction);

}

