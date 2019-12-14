using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SpellZoneVisualCircle : MonoBehaviour
{
    public GameObject MainVisual;

    public void SetSize(float val)
    {
        MainVisual.transform.localScale = Vector3.one*val/2f;
    }
}

