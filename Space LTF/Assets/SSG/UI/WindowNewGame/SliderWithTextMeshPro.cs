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
        Field.text = ((int)Slider.value).ToString("0");
        if (_changeCallback != null)
        {
            _changeCallback();
        }
    }
}

