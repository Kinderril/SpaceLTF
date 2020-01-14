using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SliderWithTextMeshProWithOff   : SliderWithTextMeshPro
{
    public GameObject OffObject;
    public bool offByMin;
    protected override void UpdateFiled()
    {
        var isMin = offByMin ? (Slider.value <= Slider.minValue) : (Slider.value >= Slider.maxValue);
        OffObject.SetActive(isMin);
        base.UpdateFiled();
    }

    protected override void subInit()
    {
        OffObject.SetActive(false);
    }

    public void SetOff(bool val)
    {
        OffObject.SetActive(val);
    }
}

