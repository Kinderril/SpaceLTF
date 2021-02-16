using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public   class ColorChangeSettingsWindow : MonoBehaviour
{
    public ColorPicker OutlineRed;
    public ColorPicker OutlineGreen;
    public ColorPicker ShieldRed;
    public ColorPicker ShieldGreen;

    public void Init()
    {
        var startSettings = MainController.Instance.Settings;
        OutlineRed.Init(startSettings.OutlineRed,OnChangeOutlineRed,Namings.Tag("OutlineRed"));
        OutlineGreen.Init(startSettings.OutlineGreen, OnChangeOutlineGreen, Namings.Tag("OutlineGreen"));
        ShieldRed.Init(startSettings.ShieldRed, OnChangeShieldRed, Namings.Tag("ShieldRed"));
        ShieldGreen.Init(startSettings.ShieldGreen, OnChangeShieldGreen, Namings.Tag("ShieldGreen"));
    }

    public void DoDefault()
    {
        var startSettings = MainController.Instance.Settings;
        startSettings.DoDefault();
        Init();
    }

    private void OnChangeShieldGreen(Color obj)
    {
        MainController.Instance.Settings.SetGreenShield(obj);
    }

    private void OnChangeShieldRed(Color obj)
    {
        MainController.Instance.Settings.SetRedShield(obj);
    }

    private void OnChangeOutlineGreen(Color obj)
    {
        MainController.Instance.Settings.SetGreenOutline(obj);
    }

    private void OnChangeOutlineRed(Color obj)
    {
        MainController.Instance.Settings.SetRedOutline(obj);
    }
}

