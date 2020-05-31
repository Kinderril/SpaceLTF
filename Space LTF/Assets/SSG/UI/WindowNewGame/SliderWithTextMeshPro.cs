using System;
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
    private AudioSource source;
    private AudioClip clip;

    void Awake()
    {
        if (!_inited)
        {
            _inited = true;
            Slider.onValueChanged.AddListener(arg0 =>
            {
                source.PlayOneShot(clip);
                UpdateFiled();
            });
        }

        subInit();
    }

    protected virtual void subInit()
    {
        source = WindowManager.Instance.UiAudioSource;
        clip = DataBaseController.Instance.AudioDataBase.SliderChange;
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

    public void InitBorders(float min, float max, bool onlyInts,bool setMidVal = true)
    {
        Slider.minValue = min;
        Slider.maxValue = max;
        Slider.wholeNumbers = onlyInts;
        if (setMidVal)
        {
            var val = min + (max - min) / 2f;
            if (onlyInts)
            {
                SetValue((int)val);
            }
            else
            {
                SetValue(val);
            }
        }
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

        Field.text = Slider.wholeNumbers?
            ((int)Slider.value).ToString("0"):
            (Slider.value).ToString("0.0");
        if (_changeCallback != null)
        {
            _changeCallback();
        }
    }
}

