using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class StartArmyChooserUI : UIElementWithTooltip
{
    public TextMeshProUGUI Field;
    public ShipConfig Config { get; private set; }
    private Action<StartArmyChooserUI> oncallback;
    public Toggle Toggle;

    public void Init(ShipConfig config,Action<StartArmyChooserUI> oncallback,bool interactable)
    {
        Field.text = Namings.ShipConfig(config);
        this.oncallback = oncallback;
        Config = config;
        Toggle.interactable = interactable;
    }

    public void OnClick()
    {
        oncallback(this);
    }
    protected override string TextToTooltip()
    {
        var p = Namings.TooltipConfigProsCons(Config);
        var p2 = Namings.TooltipConfigProsConsCalc(Config);
        return $"{p}\n{p2}";
    }
}

