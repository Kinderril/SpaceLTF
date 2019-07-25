using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class DebugStartBattleWindow : BaseWindow
{
    [HideInInspector]
//    public List<DebugShipInShop> redList = new List<DebugShipInShop>();
    public DebugShipInShop SelectedShip { get; set; }
    private TeamIndex _selectedShipIndex;
    public DebugShipInShop PrefabDebugShipInShop;
    public DebugShopCatcher DebugShopCatcher;
    public ArmyCreatorUI GreenArmy;
    public ArmyCreatorUI RedArmy;
    
    
    public override void Init()
    {

        DebugShopCatcher.Init(this);
        if (RedArmy == null)
        {
            RedArmy = DataBaseController.GetItem(GreenArmy);
            RedArmy.transform.SetParent(GreenArmy.transform.parent,false);
        }
        GreenArmy.Clear();
        GreenArmy.Init(RedArmy,TeamIndex.green, PrefabDebugShipInShop, UnSelectIfNeed, OnSelect);
        RedArmy.Clear();
        RedArmy.Init(GreenArmy,TeamIndex.red, PrefabDebugShipInShop, UnSelectIfNeed, OnSelect);
        base.Init();
    }
    
    public void OnDeleteSelectedShip()
    {
        if (SelectedShip != null)
        {
            switch (_selectedShipIndex)
            {
                case TeamIndex.red:
                    RedArmy.DeleteShip(SelectedShip);
                    break;
                case TeamIndex.green:
                    GreenArmy.DeleteShip(SelectedShip);
                    break;
            }
        }
    }

    private void UnSelectIfNeed(DebugShipInShop t)
    {
        if (t == SelectedShip && SelectedShip != null)
        {
            SelectedShip = null;
        }
    }

    private void OnSelect(DebugShipInShop obj, TeamIndex index)
    {
        _selectedShipIndex = index;
        if (SelectedShip != null)
        {
            SelectedShip.Select(false);
        }
        SelectedShip = obj;
        SelectedShip.Select(true);
    }

    public void OnStartBattleClick()
    {
        List<StartShipPilotData> greenSide = new List<StartShipPilotData>();
        bool haveBase = false;
        foreach (var debugShipInShop in GreenArmy.CurrentArmy)
        {
            StartShipPilotData d = new StartShipPilotData(debugShipInShop.PilotParameters, debugShipInShop._shipInv);
            if (d.Ship.ShipType == ShipType.Base)
            {
                haveBase = true;
            }
            greenSide.Add(d);
        }
        if (!haveBase)
        {
            Debug.LogError("green side have no base");
            return;
        }


        haveBase = false;
        List<StartShipPilotData> redSide = new List<StartShipPilotData>();
        foreach (var debugShipInShop in RedArmy.CurrentArmy)
        {
            StartShipPilotData d = new StartShipPilotData(debugShipInShop.PilotParameters, debugShipInShop._shipInv);
            if (d.Ship.ShipType == ShipType.Base)
            {
                haveBase = true;
            }
            redSide.Add(d);
        }
        if (!haveBase)
        {
            Debug.LogError("red side have no base");
            return;
        }
        var greenPLayer = new Player("Me");
        greenPLayer.Army = greenSide;
        var redPlayer = new Player("Bot_" + Utils.GetId());
        redPlayer.Army = redSide;
        MainController.Instance.PreBattle(greenPLayer,redPlayer);
    }

    public void SetBodyType(ShipType shipType,ShipConfig config)
    {
        if (SelectedShip != null)
        {
            SelectedShip.SetBody(shipType, config);
        }
    }

    public void SetModul(BaseModulInv baseModul)
    {
        if (SelectedShip != null)
        {
            SelectedShip.TryAddSimpleModul(baseModul);
        }
    }

    public void SetWeapon(WeaponInv weapon)
    {
        if (SelectedShip != null)
        {
            SelectedShip.TryAddWeapon(weapon);
        }
    }

    public void SetSpell(BaseSpellModulInv spell)
    {
        if (SelectedShip != null)
        {
            SelectedShip.TryAddSimpleSpell(spell);
        }
    }


}

