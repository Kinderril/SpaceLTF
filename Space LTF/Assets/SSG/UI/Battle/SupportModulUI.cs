﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SupportModulUI : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public UIElementWithTooltipCache Tooltip;
    
    public virtual void Init(BaseSupportModul baseModul)
    {
        gameObject.SetActive(true);
        Tooltip.Cache = baseModul.DescSupport();
        Name.text = baseModul.Name + " " + baseModul.Level.ToString("0");
    }
}

