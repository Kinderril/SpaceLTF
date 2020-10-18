using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ArmyCreatorUI : MonoBehaviour
{
    public Slider SliderPointsCount;
    public Slider SliderArmyCount;
    public Dropdown ArmyTypes;
    public Dropdown ArmyConfigs;
    public Dropdown ArmyWeaponTypes;
    public Text PointsField;
    public Text CountField;

    public Transform Layout;
    [HideInInspector]
    public List<DebugShipInShop> CurrentArmy = new List<DebugShipInShop>();
    private DebugShipInShop _prefabDebugShipInShop;
    private Action<DebugShipInShop> _onUnselect;
    private Action<DebugShipInShop, TeamIndex> _onSelect;
    List<ArmyCreationMode> _armyModesTypes = new List<ArmyCreationMode>();
    List<ShipConfig> _armyConfigs = new List<ShipConfig>();
    List<ArmyCreatorType> _armyCreatorTypes = new List<ArmyCreatorType>();
    private TeamIndex _teamIndex;
    public Player PlayerOwner;
    private ArmyCreatorUI OppositeArmy;

    public void Init(ArmyCreatorUI oppositeArmy,
        TeamIndex teamIndex, DebugShipInShop prefabDebugShipInShop,
        Action<DebugShipInShop> OnUnselect, Action<DebugShipInShop, TeamIndex> OnSelect)
    {
        OppositeArmy = oppositeArmy;
        PlayerOwner = new PlayerAI("Bot_" + Utils.GetId());
        _prefabDebugShipInShop = prefabDebugShipInShop;
        SliderPointsCount.minValue = 40;
        SliderPointsCount.maxValue = 80;
        SliderPointsCount.value = (SliderPointsCount.maxValue - SliderPointsCount.minValue) / 2;
        SliderArmyCount.minValue = 2;
        SliderArmyCount.maxValue = 5;
        SliderArmyCount.value = (SliderArmyCount.maxValue - SliderArmyCount.minValue) / 2;
        _teamIndex = teamIndex;
        _onUnselect = OnUnselect;
        _onSelect = OnSelect;
        CreateShip(ShipType.Base, ShipConfig.mercenary);
        _armyCreatorTypes.Clear();
        _armyModesTypes.Clear();
        List<string> m_DropOptions = new List<string>();
        foreach (ShipConfig amryType in (ShipConfig[])Enum.GetValues(typeof(ShipConfig)))
        {
            m_DropOptions.Add(amryType.ToString());
            _armyConfigs.Add(amryType);
        }
        SliderAddOption(ArmyConfigs, m_DropOptions);
        m_DropOptions.Clear();

        foreach (ArmyCreationMode amryType in (ArmyCreationMode[])Enum.GetValues(typeof(ArmyCreationMode)))
        {
            m_DropOptions.Add(amryType.ToString());
            _armyModesTypes.Add(amryType);
        }
        SliderAddOption(ArmyTypes, m_DropOptions);
        m_DropOptions.Clear();
        foreach (ArmyCreatorType amryCreatorType in (ArmyCreatorType[])Enum.GetValues(typeof(ArmyCreatorType)))
        {
            m_DropOptions.Add(amryCreatorType.ToString());
            _armyCreatorTypes.Add(amryCreatorType);
        }
        SliderAddOption(ArmyWeaponTypes, m_DropOptions);
    }

    public void OnBalanceClick()
    {
        Clear();
        var min = (int)OppositeArmy.GetValueOfArmy();
        var max = min;
        subCreateArmy(min, max);
    }


    public void OnLoadArmy()
    {
        Clear();
        //        PlayerOwner = Player.Load(Player.mainPlayer);
        foreach (var startShipPilotData in PlayerOwner.Army.Army)
        {
            var p = DataBaseController.GetItem(_prefabDebugShipInShop);
            AddShip(p, startShipPilotData.Ship.ShipType, startShipPilotData.Ship.ShipConfig, startShipPilotData.Ship, startShipPilotData.Pilot);
        }
    }

    public void OnCreateArmy()
    {
        Clear();
        var min = (int)SliderArmyCount.value;
        var max = (int)SliderArmyCount.value;
        subCreateArmy(min, max);
    }

    private float GetValueOfArmy()
    {
        float total = 0;
        foreach (var shipInShop in CurrentArmy)
        {
            float shipPower = Library.CalcShipPower(shipInShop._shipInv);
            float pilotPower = Library.CalcPilotPower(shipInShop.PilotParameters);
            total += shipPower;
            total += pilotPower;
        }
        return total;
    }

    private void subCreateArmy(int min, int max)
    {
        var h = _armyCreatorTypes[ArmyWeaponTypes.value];
        var d = _armyModesTypes[ArmyTypes.value];
        var config = _armyConfigs[ArmyConfigs.value];
        ArmyCreatorData data = ArmyCreatorLibrary.GetArmy(config);
        var army = ArmyCreator.CreateArmy(SliderPointsCount.value, d, min, max, data, true, PlayerOwner);
        foreach (var ship in army)
        {
            //            _prefabDebugShipInShop.SetConfig(config);
            var p = DataBaseController.GetItem(_prefabDebugShipInShop);
            AddShip(p, ship.Ship.ShipType, config, p._shipInv, ship.Pilot);
            foreach (var simple in ship.Ship.Moduls.GetNonNullActiveSlots())
            {
                p.TryAddSimpleModul(simple);
            }
            foreach (var simple in ship.Ship.WeaponsModuls.GetNonNullActiveSlots())
            {
                p.TryAddWeapon(simple);
            }
            foreach (var simple in ship.Ship.SpellsModuls.GetNonNullActiveSlots())
            {
                p.TryAddSimpleSpell(simple);
            }
            p.UpdateImages();
        }
    }

    public void OnPointsSliderUpdate()
    {
        PointsField.text = SliderPointsCount.value.ToString("0");
    }

    public void OnArmyCountSliderUpdate()
    {
        CountField.text = SliderArmyCount.value.ToString("0");
    }

    public void DeleteShip(DebugShipInShop ship)
    {
        if (CurrentArmy.Contains(ship))
        {
            CurrentArmy.Remove(ship);
        }
        GameObject.DestroyImmediate(ship);
    }

    public void OnCreatClick()
    {
        CreateShip(ShipType.Middle, ShipConfig.mercenary);
    }

    private void SliderAddOption(Dropdown dd, List<string> options)
    {
        dd.ClearOptions();
        dd.AddOptions(options);
    }

    private void CreateShip(ShipType st, ShipConfig confi)
    {
        var p = DataBaseController.GetItem(_prefabDebugShipInShop);
        AddShip(p, st, confi);
    }

    private void AddShip(DebugShipInShop p, ShipType st, ShipConfig config, ShipInventory ship = null, IPilotParameters pilot = null)
    {
        p.Init(PlayerOwner, ship, pilot);
        p.SetCallbacks(shop =>
        {
            CurrentArmy.Remove(p);
            _onUnselect(p);
            Destroy(p.gameObject);
        }, shop => { _onSelect(shop, _teamIndex); });
        p.transform.SetParent(Layout);
        CurrentArmy.Add(p);
        p.SetBody(st, config);
    }

    public void Clear()
    {
        foreach (var debugShipInShop in CurrentArmy)
        {
            GameObject.DestroyImmediate(debugShipInShop.gameObject);
        }
        CurrentArmy.Clear();
    }
}

