using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ModulUI : MonoBehaviour
{
    public Image Background;
    public TextMeshProUGUI Name;
    private ActionModulInGame _baseModul;
    private bool _prevIsReady;
    
    public virtual void Init(ActionModulInGame baseModul)
    {
        _baseModul = baseModul;
//        _baseModul.OnApply += OnApply;
//        _baseModul.OnReady += OnReady;
        gameObject.SetActive(true);
//        OnApply(baseModul, baseModul.IsApply);
        Name.text = baseModul.ModulData.Name + " " + baseModul.ModulData.Level.ToString("0");

        var s = (_baseModul.IsReady());
        if (s)
        {
            Background.fillAmount = 0f;
        }
        if (!s)
        {
            var percent = _baseModul.PercentReady();
            Background.fillAmount = percent;
        }
    }
    

    void Update()
    {
        UpdateLoop();
    }

    private void UpdateLoop()
    {
        var s = (_baseModul.IsReady());
        if (s != _prevIsReady)
        {
            if (s)
            {
                Background.fillAmount = 0f;
            }
            else
            {
                Background.fillAmount = 1f;
            }
        }
        if (!s)
        {
            var percent = _baseModul.PercentReady();
            Background.fillAmount = percent;
        }
        _prevIsReady = s;
    }

    public virtual void Dispose()
    {
//        _baseModul.OnReady -= OnReady;
//        _baseModul.OnApply -= OnApply;
    }
}

