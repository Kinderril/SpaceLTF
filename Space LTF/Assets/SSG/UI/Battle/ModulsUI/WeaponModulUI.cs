using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponModulUI : MonoBehaviour
{
    public Slider ReloadSlider;
    public Image LoadImage;
    private WeaponInGame weapon;
    private float lastFrameLoad;
    public TextMeshProUGUI Name;

    public void Init(WeaponInGame baseModul)
    {
        weapon = baseModul as WeaponInGame;

        gameObject.SetActive(true);
        Name.text = baseModul.Name + " " + baseModul.Level.ToString("0");
    }

    void Update()
    {
        var d = weapon.PercentLoad();
        LoadImage.color = d <= 0.001f ? Color.green : Color.red;
        if (d != lastFrameLoad)
        {
            ReloadSlider.value = 1f-d;
        }
    }

    public virtual void Dispose()
    {

    }
}

