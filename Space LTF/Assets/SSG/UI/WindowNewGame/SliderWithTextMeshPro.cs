using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SliderWithTextMeshPro : MonoBehaviour
{
    public Slider Slider;
    public TextMeshProUGUI Field;
    public TextMeshProUGUI NameField;
    private bool _inited;
    private Action _changeCallback;

    void Awake()
    {
        if (!_inited)
        {
            _inited = true;
            Slider.onValueChanged.AddListener(arg0 =>
            {
                UpdateFiled();
            });
        }

        subInit();
    }

    protected virtual void subInit()
    {
        
    }

    public void InitName(string name)
    {
        if (NameField != null)
        {
            NameField.text = name;
        }
    }

    public void InitCallback(Action changeCallback)
    {
        _changeCallback = changeCallback;
    }

    public void InitBorders(float min, float max, bool onlyInts)
    {
        Slider.minValue = min;
        Slider.maxValue = max;
        SetValue(min + (max - min) / 2f);
        Slider.wholeNumbers = onlyInts;
    }

    public float GetValue()
    {
        return Slider.value;
    }

    public int GetValueInt()
    {
        return (int)Slider.value;
    }

    public void SetValue(float val)
    {
        Slider.value = val;
        UpdateFiled();
    }

    protected virtual void UpdateFiled()
    {
        Field.text = Slider.value.ToString("0");
        if (_changeCallback != null)
        {
            _changeCallback();
        }
    }
}

