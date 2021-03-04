using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SpellZoneVisualCircle : MonoBehaviour
{
    public GameObject MainVisual;

    public void SetRad(float val)
    {
#if UNITY_EDITOR
        if (val == 0)
        {
            Debug.LogError("circle size is null");
        }
#endif
        MainVisual.transform.localScale = Vector3.one*val;
    }
}

