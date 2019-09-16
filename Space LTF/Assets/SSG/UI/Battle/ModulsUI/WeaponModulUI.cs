using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponModulUI : UIElementWithTooltip
{
    public Slider ReloadSlider;
//    public Image LoadImage;
    private WeaponInGame weapon;
    private float lastFrameLoad;
    public TextMeshProUGUI Name;
    private string tooltip;

    public void Init(WeaponInGame baseModul)
    {
        weapon = baseModul as WeaponInGame;
        tooltip ="Damage:" + weapon.CurrentDamage.BodyDamage.ToString("0") + "/" + weapon.CurrentDamage.ShieldDamage.ToString("0");
        gameObject.SetActive(true);
        Name.text = baseModul.Name + " " + baseModul.Level.ToString("0");
    }

    void Update()
    {
        var d = weapon.PercentLoad();
//        LoadImage.color = d <= 0.001f ? Color.green : Color.red;
        if (d != lastFrameLoad)
        {
            ReloadSlider.value = 1f-d;
        }
    }

    public virtual void Dispose()
    {

    }

    protected override string TextToTooltip()
    {
        return tooltip;
    }
}

