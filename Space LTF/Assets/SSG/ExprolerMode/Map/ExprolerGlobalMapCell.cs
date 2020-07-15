using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public enum ExprolerCellMapType
{
    normal,
    milatary,
    longType,
}

[Serializable]
public class ExprolerGlobalMapCell : MonoBehaviour
{
    public GameObject BlockObject;
    public Image BackObject;
    public Image UnblockObject;
    public Image GlowObject;
    public Image MilitaryObject;

    public GameObject Size4;
    public GameObject Size5;
    public GameObject Size6;

    public Animator Anim;
    public CanvasGroup AnimCanvas;
    
    public TextMeshProUGUI LevelField;
    public int Id;
    public int Power = 10;

    public ExprolerCellMapType MapType = ExprolerCellMapType.normal;
    //    public int Size = 6;
    public ShipConfig Config = ShipConfig.droid;
    public UIElementWithTooltipCache Tooltip;
    public List<int> Neighhoods = new List<int>();
    public string ids = "";
    private Action<ExprolerGlobalMapCell> _clickCell;

    public bool IsOpen;

    void Awake()
    {
        Neighhoods.Clear();
        var split = ids.Split('_');
        foreach (var s in split)
        {
            if (s.Length > 0)
            {
                if (Int32.TryParse(s, out var nID))
                {
                    Neighhoods.Add(nID);
                }
            }

        }

        switch (MapType)
        {
            case ExprolerCellMapType.normal:
                MilitaryObject.gameObject.SetActive(false);
                break;
            case ExprolerCellMapType.milatary:
            case ExprolerCellMapType.longType:
                MilitaryObject.gameObject.SetActive(true);
                break;
//                MilitaryObject.gameObject.SetActive(false);
//                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private string TooltipCache()
    {
        if (IsOpen)
        {
            return $"{Namings.ShipConfig(Config)} Power:{Power}";// {Namings.Tag("Size")}:{Size}";
        }
        else
        {

            return $"{Namings.Tag("Blocked")} ";// {Namings.Tag("Size")}:{Size}";
        }
    }

    public void Init()
    {
        LevelField.text = Power.ToString();
        UnblockObject.gameObject.SetActive(false);
        BlockObject.SetActive(true);
        Size4.SetActive(false);
        Size5.SetActive(false);
        Size6.SetActive(false);
        GlowObject.gameObject.SetActive(false);
        Tooltip.Cache = TooltipCache();

        var c = Library.GetColorByConfig(Config);
        var copy = new Color(c.r, c.g, c.b,0.5f);
        MilitaryObject.color = copy;
        UnblockObject.color = c;
        BackObject.color = c;
        GlowObject.color =c;
    }

    public void Unblock()
    {
        IsOpen = true;
//        UnblockObject.SetActive(true);
        BlockObject.SetActive(false);
        GlowObject.gameObject.SetActive(true);
        Tooltip.Cache = TooltipCache();

    }

    public void Complete(HashSet<int> keys)
    {
        foreach (var key in keys)
        {
            switch (key)
            {
                case 4:
                    Size4.SetActive(true);
                    break;
                case 5:
                    Size5.SetActive(true);
                    break;
                case 6:
                    Size6.SetActive(true);
                    break;
            }
        }
        UnblockObject.gameObject.SetActive(true);
    }

    public void SetClickCallback(Action<ExprolerGlobalMapCell> clickCell)
    {
//        if (IsOpen)
            _clickCell = clickCell;
    }

    public void OnClickCell()
    {
        _clickCell(this);
    }

    public void Block()
    {
        IsOpen = false;
        BlockObject.SetActive(!IsOpen);
        GlowObject.gameObject.SetActive(IsOpen);
        Tooltip.Cache = TooltipCache();

    }

    public void StopAnim()
    {
        Anim.enabled = false;
        AnimCanvas.alpha = 1f;
    }

    public void StartAnim()
    {
        Anim.enabled = true;
    }
}
