using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ShipInventory : IStartShipParams, IInventory
{
    public float MaxHealth { get; private set; }
    public float MaxShiled { get; private set; }
    public float MaxSpeed { get; private set; }
    public float TurnSpeed { get; private set; }
    public float ShieldRegen { get; private set; }
    public float BodyArmor { get; set; }
    public float ShiledArmor { get; set; }
    public float HealthPercent { get; private set; }


    [field: NonSerialized]
    public event ItemTransferedTo OnItemAdded;
    [field: NonSerialized]
    public event Action<ShipInventory> OnShipRepaired;
    [field: NonSerialized]
    public event Action<ShipInventory> OnShipCriticalChange;

    //    public int DamageTimes = 0;
    public ShipBattleData LastBattleData;
    public ShipType ShipType { get; private set; }
    public ShipConfig ShipConfig { get; private set; }
    public int WeaponModulsCount { get; private set; }
    public int SimpleModulsCount => Moduls.SimpleModulsCount;
    public PilotParameters PilotParameters => _pilot;
    public int SpellModulsCount { get; private set; }
    public int CriticalDamages { get; private set; }
    public string Name { get; private set; }
    public float BoostChargeTime { get; private set; }
    public PilotTactic Tactic => PilotParameters.Tactic;

    public int Id { get; private set; }
    public ShipModulsInventory Moduls;
    public ShipWeaponsInventory WeaponsModuls;
    public BaseSpellModulInv[] SpellsModuls;
    public ParameterItem CocpitSlot { get; private set; }
    public ParameterItem EngineSlot { get; private set; }
    public ParameterItem WingSlot { get; private set; }

    private readonly Player _player;
    private PilotParameters _pilot;


    public ShipInventory(IStartShipParams pParams, Player player, PilotParameters pilot)
    {
        _pilot = pilot;
        CriticalDamages = 0;
        HealthPercent = 1f;
        _player = player;
        MaxHealth = pParams.MaxHealth;
        MaxShiled = pParams.MaxShiled;
        MaxSpeed = pParams.MaxSpeed;
        TurnSpeed = pParams.TurnSpeed;
        ShieldRegen = pParams.ShieldRegen;
        ShipType = pParams.ShipType;
        ShipConfig = pParams.ShipConfig;
        BodyArmor = pParams.BodyArmor;
        ShiledArmor = pParams.ShiledArmor;
        WeaponModulsCount = pParams.WeaponModulsCount;
        WeaponsModuls = new ShipWeaponsInventory(WeaponModulsCount,this);
//        SimpleModulsCount = pParams.SimpleModulsCount;
        Moduls = new ShipModulsInventory(pParams.SimpleModulsCount, this);
        SpellModulsCount = pParams.SpellModulsCount;
        SpellsModuls = new BaseSpellModulInv[SpellModulsCount];
        Name = pParams.Name;
        BoostChargeTime = pParams.BoostChargeTime;
        //        Debug.Log("ShipInventory create:" + pParams.Name);
        Id = Utils.GetId();
        CreateBaseItems(ShipType);
        //        BodyVisualType = pParams.BodyVisualType;
    }

    private void CreateBaseItems(ShipType shipType)
    {
        switch (shipType)
        {
            case ShipType.Light:
                var cocpit = Library.CreateParameterItem(EParameterItemSubType.light, EParameterItemRarity.normal, ItemType.cocpit);
                TryAddItem(cocpit);
                var engineSlot = Library.CreateParameterItem(EParameterItemSubType.light, EParameterItemRarity.normal, ItemType.engine);
                TryAddItem(engineSlot);
                var wingSlot = Library.CreateParameterItem(EParameterItemSubType.light, EParameterItemRarity.normal, ItemType.wings);
                TryAddItem(wingSlot);
                break;
            case ShipType.Middle:
                var cocpit1 = Library.CreateParameterItem(EParameterItemSubType.middle, EParameterItemRarity.normal, ItemType.cocpit);
                TryAddItem(cocpit1);
                var engineSlot1 = Library.CreateParameterItem(EParameterItemSubType.middle, EParameterItemRarity.normal, ItemType.engine);
                TryAddItem(engineSlot1);
                var wingSlot1 = Library.CreateParameterItem(EParameterItemSubType.middle, EParameterItemRarity.normal, ItemType.wings);
                TryAddItem(wingSlot1);
                break;
            case ShipType.Heavy:
                var cocpit2 = Library.CreateParameterItem(EParameterItemSubType.heavy, EParameterItemRarity.normal, ItemType.cocpit);
                TryAddItem(cocpit2);
                var engineSlot2 = Library.CreateParameterItem(EParameterItemSubType.heavy, EParameterItemRarity.normal, ItemType.engine);
                TryAddItem(engineSlot2);
                var wingSlot2 = Library.CreateParameterItem(EParameterItemSubType.heavy, EParameterItemRarity.normal, ItemType.wings);
                TryAddItem(wingSlot2);
                break;
        }
    }

    public List<IItemInv> GetAllItems()
    {
        var list = new List<IItemInv>();
        list.AddRange(WeaponsModuls.GetNonNullActiveSlots());
        list.AddRange(SpellsModuls);
        list.AddRange(Moduls.GetNonNullActiveSlots());
        return list;
    }

    public bool GetFreeSlot(out int index, ItemType type)
    {
        index = -1;
        switch (type)
        {
            case ItemType.weapon:
                return GetFreeWeaponSlot(out index);
            case ItemType.modul:
                return Moduls.GetFreeSimpleSlot(out index);
            case ItemType.spell:
                return GetFreeSpellSlot(out index);
            case ItemType.cocpit:
                return CocpitSlot == null;
            case ItemType.engine:
                return EngineSlot == null;
            case ItemType.wings:
                return WingSlot == null;
        }

        return false;
    }

    public bool GetFreeSimpleSlot(out int index)
    {
        return Moduls.GetFreeSimpleSlot(out index);
    }

    public float ValuableItem(IItemInv item)
    {
        return 1f;
    }

    private bool GetFreeItemSlot<T>(out int index,T[] list)
    {
        if (list == null)
        {
            index = -1;
            return false;
        }
        for (int i = 0; i < list.Length; i++)
        {
            var m = list[i];
            if (m == null)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }

    public bool GetFreeSpellSlot(out int index)
    {
        return GetFreeItemSlot(out index, SpellsModuls);
    }

    public bool GetFreeWeaponSlot(out int index)
    {
        return WeaponsModuls.GetFreeSimpleSlot(out index);
    }

    public bool TryAddSpellModul(BaseSpellModulInv spellModul, int fieldIndex)
    {
        if (spellModul == null)
        {
            return false;
        }
        if (SpellsModuls.Length <= fieldIndex)
        {
            Debug.LogError("Too big spell index slot");
            return false;
        }
        var field = SpellsModuls[fieldIndex];
        if (field == null)
        {
            SpellsModuls[fieldIndex] = spellModul;
            spellModul.CurrentInventory = this;
            TransferItem(spellModul, true);
            return true;
        }
        Debug.LogError("Slot not spell free");
        return false;
    }

    public bool TryAddSimpleModul(BaseModulInv simpleModul, int fieldIndex)
    {
        return Moduls.TryAddSimpleModul(simpleModul, fieldIndex);
    }



    public bool RemoveSimpleModul(BaseModulInv simpleModul)
    {
        return Moduls.RemoveSimpleModul(simpleModul);
    }

    public bool TryAddWeaponModul(WeaponInv weaponModul, int fieldIndex)
    {
        return WeaponsModuls.TryAddWeaponModul(weaponModul, fieldIndex);
    }

    public bool RemoveWeaponModul(WeaponInv weaponModul)
    {
        return WeaponsModuls.RemoveWeaponModul(weaponModul);
    }

    public bool RemoveSpellModul(BaseSpellModulInv spell)
    {
        for (int i = 0; i < SpellsModuls.Length; i++)
        {
            var m = SpellsModuls[i];
            if (m == spell)
            {
                SpellsModuls[i] = null;
                TransferItem(m, false);
                return true;
            }
        }
        return false;
    }

    public void TransferItem(IItemInv item, bool val)
    {
        OnItemAdded?.Invoke(item, val);
    }

    public Player Owner
    {
        get { return _player; }
    }

    public int SlotsCount => SpellModulsCount + SimpleModulsCount + WeaponModulsCount;

    public bool IsShop()
    {
        return false;
    }

    public bool CanMoveToByLevel(IItemInv item, int posibleLevel)
    {
        if (posibleLevel <= 0)
        {
            return item.RequireLevel() <= _pilot.CurLevel;
        }
        else
        {
            return item.RequireLevel(posibleLevel) <= _pilot.CurLevel;
        }
    }

    public bool TryAddItem(ParameterItem itemParam)
    {

        if (itemParam == null)
        {
            return false;
        }

        switch (itemParam.ItemType)
        {
            case ItemType.cocpit:
                if (CocpitSlot == null)
                {
                    CocpitSlot = itemParam;
                    itemParam.CurrentInventory = this;
                    TransferItem(itemParam, true);
                    CheckerRefreshSlots(itemParam);
                    return true;
                }
                break;
            case ItemType.engine:
                if (EngineSlot == null)
                {
                    EngineSlot = itemParam;
                    itemParam.CurrentInventory = this;
                    TransferItem(itemParam, true);
                    CheckerRefreshSlots(itemParam);
                    return true;
                }
                break;
            case ItemType.wings:
                if (WingSlot == null)
                {
                    WingSlot = itemParam;
                    itemParam.CurrentInventory = this;
                    TransferItem(itemParam, true);
                    CheckerRefreshSlots(itemParam);
                    return true;
                }
                break;
        }
        return false;
    }

    private void CheckerRefreshSlots(ParameterItem itemParam)
    {
        float val;
        if (itemParam.ParametersAffection.TryGetValue(EParameterShip.modulsSlots, out val))
        {
            var intSlots = (int)(val + 0.1f);
            for (int i = 0; i < intSlots; i++)
            {
                Moduls.AddSlots(DragItemType.modul);
            }
        }  
        if (itemParam.ParametersAffection.TryGetValue(EParameterShip.weaponSlots, out val))
        {
            var intSlots = (int)(val + 0.1f);
            for (int i = 0; i < intSlots; i++)
            {
                WeaponsModuls.AddSlots(DragItemType.weapon);
            }
        }
    }

    public bool RemoveItem(ParameterItem itemParam)
    {

        if (itemParam == null)
        {
            return false;
        }

        switch (itemParam.ItemType)
        {
            case ItemType.cocpit:
                CocpitSlot= null;
                TransferItem(itemParam, false);
                CheckRemoveSlots(itemParam);
                return true;
            case ItemType.engine:
                EngineSlot = null;
                TransferItem(itemParam, false);
                CheckRemoveSlots(itemParam);
                return true;
            case ItemType.wings:
                WingSlot = null;
                TransferItem(itemParam, false);
                CheckRemoveSlots(itemParam);
                return true;
        }
        return false;
    }

    private void CheckRemoveSlots(ParameterItem itemParam)
    {
        float val;
        if (itemParam.ParametersAffection.TryGetValue(EParameterShip.modulsSlots, out val))
        {
            var intSlots = (int)(val + 0.1f);
            for (int i = 0; i < intSlots; i++)
            {
                if (!Moduls.RemoveSlot(DragItemType.modul))
                {
                    Debug.LogError($"Can't remove modules:{intSlots}");
                }
            }
        }    
        if (itemParam.ParametersAffection.TryGetValue(EParameterShip.weaponSlots, out val))
        {
            var intSlots = (int)(val + 0.1f);
            for (int i = 0; i < intSlots; i++)
            {
                if (!WeaponsModuls.RemoveSlot(DragItemType.weapon))
                {
                    Debug.LogError($"Can't remove modules:{intSlots}");
                }
            }
        }
    }

    public bool CanRemoveModulSlots(int slotsInt)
    {
        return Moduls.CanRemoveSlots(slotsInt);
    }    
    public bool CanRemoveWeaponSlots(int slotsInt)
    {
        return WeaponsModuls.CanRemoveSlots(slotsInt);
    }

    public int HealthPointToRepair()
    {
        var delta = (int)(MaxHealth * (1f - HealthPercent));
        return delta;
    }

    //public void RepairFor()
    //{
    //    Owner.MoneyData.RemoveMoney(perShip);
    //}

    public bool SetRepairPercent(float p)
    {
        //        Debug.Log("SetRepairPercent:" + p);
        p = Mathf.Clamp(p, 0.01f, 1f);

        var delta = Mathf.Abs(p - HealthPercent);
        HealthPercent = p;
        if (delta > 0)
        {
            if (OnShipRepaired != null)
            {
                OnShipRepaired(this);
            }
            return true;
        }

        return false;
    }

    public bool IsDead()
    {
        return CriticalDamages >= Library.CRITICAL_DAMAGES_TO_DEATH;
    }

    public void AddCriticalyDamage()
    {
        CriticalDamages++;
        if (IsDead())
        {
            Debug.Log("Ship fully destroyed cause critical damages");
            MainController.Instance.MainPlayer.Army.RemoveShip(this);
        }
        else
        {
            if (OnShipCriticalChange != null)
            {
                OnShipCriticalChange(this);
            }
        }


    }

    public void RestoreAllCriticalDamages()
    {
        CriticalDamages = 0;
        if (OnShipCriticalChange != null)
        {
            OnShipCriticalChange(this);
        }
    }
}


