using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class DebugShipInShop : MonoBehaviour
{
    public ShipInventory _shipInv;
    public IPilotParameters PilotParameters;
    public Transform LayoutModuls;
    public Transform LayoutWeapons;
    public Transform LayoutSpells;
    public Transform LayoutParameters;
    public Text Prefab;
    public Button PrefabButton;
    private Action<DebugShipInShop> _onRemove;
    private Action<DebugShipInShop> _onSelect;
    public Image SelectImage;
    private Player _player;

    public void Init(Player player,ShipInventory ship = null, IPilotParameters pilot = null)
    {
        _player = player;
        Select(false);
        Prefab.gameObject.SetActive(false);
        if (pilot != null)
        {
            PilotParameters = pilot;
        }
        else
        {
            Debug.LogError("using debug pilot");
            PilotParameters = Library.CreateDebugPilot();
        }
        if (ship != null)
        {
            _shipInv = ship;
        }
        else
        {
            Debug.LogError("using debug ship");
            _shipInv = Library.CreateShip(ShipType.Middle,ShipConfig.mercenary, _player.SafeLinks, PilotParameters as PilotParameters);
        }
        UpdateImages();
    }

    public void SetCallbacks(Action<DebugShipInShop> OnRemove, Action<DebugShipInShop> onSelect)
    {
        _onRemove = OnRemove;
        _onSelect = onSelect;
    }

    public void Select(bool val)
    {
        SelectImage.enabled = val;
    }

    public void OnSelectClick()
    {
        _onSelect(this);
    }

    public void OnRemoveClick()
    {
        _onRemove(this);
    }

    public void SetBody(ShipType shiopType,ShipConfig config)
    {
        _shipInv = Library.CreateShip(shiopType, config, _player.SafeLinks,Library.CreateDebugPilot());
        UpdateImages();
    }

    public void TryAddSimpleModul(BaseModulInv m)
    {
        int slotId;
        if (_shipInv.GetFreeSimpleSlot(out slotId))
        {
            _shipInv.TryAddSimpleModul(m, slotId);
            UpdateImages();
        }
    }

    public void TryAddSimpleSpell(BaseSpellModulInv m)
    {
        int slotId;
        if (_shipInv.GetFreeSpellSlot(out slotId))
        {
            _shipInv.TryAddSpellModul(m, slotId);
            UpdateImages();
        }
    }

    public void TryAddWeapon(WeaponInv m)
    {
        int slotId;
        if (_shipInv.GetFreeWeaponSlot(out slotId))
        {
            _shipInv.TryAddWeaponModul(m, slotId);
            UpdateImages();
        }
    }

    public void UpdateImages()
    {
        LayoutModuls.ClearTransformImmediate();
        LayoutWeapons.ClearTransformImmediate();
        LayoutSpells.ClearTransformImmediate();
        LayoutParameters.ClearTransformImmediate();
        foreach (var sm in _shipInv.Moduls.GetNonNullActiveSlots())
        {
            var msg = (sm == null) ? "null" : sm.GetInfo();
            CreatAndText(msg, LayoutModuls);
        }
        foreach (var sm in _shipInv.WeaponsModuls.GetNonNullActiveSlots())
        {
            var msg = (sm == null)?"null": sm.GetInfo();
            CreatAndText(msg, LayoutWeapons);
        }
        foreach (var sm in _shipInv.SpellsModuls)
        {
            var msg = (sm == null) ? "null" : sm.GetInfo();
            CreatAndText(msg, LayoutSpells);
        }
        var calulatedParams = ShipParameters.CalcParams(_shipInv, PilotParameters, new List<EParameterShip>()
        {
            EParameterShip.bodyPoints, EParameterShip.shieldPoints, EParameterShip.speed, EParameterShip.turn,EParameterShip.bodyArmor
        });
        var maxSpeed = calulatedParams[EParameterShip.speed];// ShipParameters.ParamUpdate(shipSpeedBase, _pilot.SpeedLevel, ShipParameters.MaxSpeedCoef);
        var turnSpeed = calulatedParams[EParameterShip.turn];//ShipParameters.ParamUpdate(turnSpeedBase, _pilot.TurnSpeedLevel, ShipParameters.TurnSpeedCoef);
        var maxShiled = calulatedParams[EParameterShip.shieldPoints];//ShipParameters.ParamUpdate(maxShiledBase, _pilot.ShieldLevel, ShipParameters.MaxShieldCoef);
        var maxHealth = calulatedParams[EParameterShip.bodyPoints];// ShipParameters.ParamUpdate(maxHealthBase, _pilot.HealthLevel, ShipParameters.MaxHealthCoef);


        string f = "{0}:" + "{1}".ToString() + "(" + "{2}" + ")";
        CreatAndText("ShipType:" + _shipInv.ShipType.ToString(), LayoutParameters);
        CreatAndText(Namings.Format(f, "MaxHealth", maxHealth, PilotParameters.HealthLevel), LayoutParameters);
        CreatAndText(Namings.Format(f, "MaxShiled", maxShiled, PilotParameters.ShieldLevel), LayoutParameters);
        CreatAndText(Namings.Format(f, "TurnSpeed", turnSpeed, PilotParameters.TurnSpeedLevel), LayoutParameters);
        CreatAndText(Namings.Format(f, "MaxSpeed", maxSpeed, PilotParameters.SpeedLevel), LayoutParameters);

//        CreatAndText("MaxHealth:" + MaxHealth.ToString("0"), LayoutParameters);
//        CreatAndText("MaxShiled:" + MaxShiled.ToString("0"), LayoutParameters);
//        CreatAndText("TurnSpeed:" + TurnSpeed.ToString("0"), LayoutParameters);
//        CreatAndText("MaxSpeed:" + MaxSpeed.ToString("0"), LayoutParameters);

        CreatAndText("ShieldRegen:" + _shipInv.ShieldRegen.ToString("0"), LayoutParameters);
        CreatAndText("Level:" + PilotParameters.CurLevel.ToString("0"), LayoutParameters);
        CreatAndButton("Tactic:" + PilotParameters.Tactic.ToString(), LayoutParameters,OnClickTactic);

    }

    private void OnClickTactic(Text obj)
    {
//        PilotParameters.ChangeTactic();
        obj.text = "Tactic:" + PilotParameters.Tactic.ToString();
    }
    
    private void CreatAndText(string msg, Transform parent)
    {
        var p = DataBaseController.GetItem(Prefab);
        p.gameObject.SetActive(true);
        p.transform.SetParent(parent, false);
        p.text = msg;
    }

    private Text CreatAndButton(string msg, Transform parent,Action<Text> clickAction)
    {
        var p = DataBaseController.GetItem(PrefabButton);
        p.gameObject.SetActive(true);
        p.transform.SetParent(parent, false);
        var txt = p.GetComponentInChildren<Text>();
        txt.text = msg;
        p.onClick.AddListener(() =>
        {
            clickAction(txt);
        });
        return txt;
    }

}

