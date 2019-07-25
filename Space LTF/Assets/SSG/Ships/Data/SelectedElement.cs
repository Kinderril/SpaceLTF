using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public  class SelectedElement : MonoBehaviour
{
    public Transform ScalTransform;

    public void Init(ShipType shipType)
    {
        float scale = 1f;
        switch (shipType)
        {
            case ShipType.Light:
                scale = 1.1f;
                break;
            case ShipType.Middle:
                scale = 1.3f;
                break;
            case ShipType.Heavy:
                scale = 1.6f;
                break;
            case ShipType.Base:
                scale = 2f;
                break;
        }
        ScalTransform.localScale = Vector3.one * scale;
    }

    public void SetActive(bool val)
    {
        gameObject.SetActive(val);
    }
}

