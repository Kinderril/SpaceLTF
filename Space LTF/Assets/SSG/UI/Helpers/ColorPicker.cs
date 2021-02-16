using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ColorPicker  : MonoBehaviour
{
    public TextMeshProUGUI NameFiled;
    public Slider SliderRed;
    public Slider SliderGreen;
    public Slider SliderBlue;
    public Image TestImage;
    private Color _color;
    private Action<Color> _callbackChange;
    public bool AlphaUse = false;

    public void Init(Color startColor, Action<Color> callbackChange,string name)
    {
        _callbackChange = callbackChange;
        _color = startColor;
        SliderRed.value = startColor.r;
        SliderGreen.value = startColor.g;
        SliderBlue.value = startColor.b;
        UpdateTestImage();
        NameFiled.text = name;
    }

    public void UpdateTestImage()
    {
        if (!AlphaUse)
            _color.a = 1f;
        TestImage.color = _color;
    }

    public void OnChangeColor()
    {
        _color = new Color(SliderRed.value,SliderGreen.value,SliderBlue.value);
        _callbackChange(_color);
        UpdateTestImage();
    }
}

