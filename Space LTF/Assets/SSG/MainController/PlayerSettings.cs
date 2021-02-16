using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class PlayerSettings : MonoBehaviour
{
    public Material OutlieMaterial;
    public Color ShieldGreen = Utils.CreateColor(39f, 191f, 165f, 146f / 255f);
    public Color ShieldRed = Utils.CreateColor(191, 86, 39f, 146f / 255f);      
    public Color OutlineGreen = Utils.CreateColor(39f, 191f, 165f, 146f / 255f);
    public Color OutlineRed = Utils.CreateColor(191, 86, 39f, 146f / 255f);
    private const string haveSmt = "haveSmt";
    private const string ShieldGreenKey = "ShieldGreen";
    private const string ShieldRedKey = "ShieldRed";
    private const string OutlineGreenKey = "OutlineGreen";
    private const string OutlineRedKey = "OutlineRed";

    void Awake()
    {
        var haveLoad = PlayerPrefs.GetInt(haveSmt, -1) > 0;
        if (haveLoad)
            LoadColors();
        else
        {
            DoDefault();
        }
    }


    private void LoadColors()
    {
        OutlineGreen = LoadColor(OutlineGreenKey);
        OutlineRed = LoadColor(OutlineRedKey);
        ShieldGreen = LoadColor(ShieldGreenKey);
        ShieldRed = LoadColor(ShieldRedKey);
    }
    private void SaveColor()
    {
        SaveColor(OutlineGreenKey, OutlineGreen);
        SaveColor(OutlineRedKey, OutlineRed);
        SaveColor(ShieldGreenKey, ShieldGreen);
        SaveColor(ShieldRedKey, ShieldRed);
        PlayerPrefs.SetInt(haveSmt,1);

    }

    public void UpdateOutline()
    {
        OutlieMaterial.SetColor("_ColorRed",OutlineRed);
        OutlieMaterial.SetColor("_ColorGreen", OutlineGreen);
        SaveColor();
    }


    public void SetRedShield(Color color)
    {
        ShieldRed = color;
        SaveColor();
    }   
    public void SetGreenShield(Color color)
    {
        ShieldGreen = color;
        SaveColor();
    }     
    public void SetRedOutline(Color color)
    {
        OutlineRed = color;
        UpdateOutline();
    }   
    public void SetGreenOutline(Color color)
    {
        OutlineGreen = color;
        UpdateOutline();
    }

    public void DoDefault()
    {
        ShieldGreen = Utils.CreateColor(39f, 191f, 165f, 146f / 255f);
        ShieldRed = Utils.CreateColor(191, 86, 39f, 146f / 255f);

        OutlineGreen = Utils.CreateColor(50, 152, 1, 1f);
        OutlineRed = Utils.CreateColor(255f, 157, 1, 1f); 

        SaveColor();

    }
    private void SaveColor(string name, Color color)
    {
        PlayerPrefs.SetFloat(NameR(name), color.r);
        PlayerPrefs.SetFloat(NameG(name), color.g);
        PlayerPrefs.SetFloat(NameB(name), color.b);
        PlayerPrefs.SetFloat(NameA(name), color.a);
    }

    private string NameR(string name)
    {
        return $"{name}r";
    }    
    private string NameB(string name)
    {
        return $"{name}b";
    }    
    private string NameG(string name)
    {
        return $"{name}g";
    }    
    private string NameA(string name)
    {
        return $"{name}a";
    }


    private Color LoadColor(string name)
    {

        var r = PlayerPrefs.GetFloat(NameR(name));
        var g = PlayerPrefs.GetFloat(NameG(name));
        var b = PlayerPrefs.GetFloat(NameB(name));
        var a = PlayerPrefs.GetFloat(NameA(name));
        return new Color(r, g, b, a);
    }

}

