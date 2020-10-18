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
    [field: NonSerialized]
    public event Action<ShipInventory,bool> OnDeadChange;

    //    public int DamageTimes = 0;
    public ShipBattleData LastBattleData;
    public ShipType ShipType { get; private set; }
    public ShipConfig ShipConfig { get; private set; }
    public int WeaponModulsCount => WeaponsModuls.WeaponsCount;
    public int SimpleModulsCount => Moduls.SimpleModulsCount;
    public PilotParameters PilotParameters => _pilot;
    public int SpellModulsCount { get; private set; }
    public int CriticalDamages { get; private set; }
    private bool _isDead = false;

    public bool IsDead
    {
        get => _isDead;
        private set
        {
            _isDead = value;
            OnDeadChange?.Invoke(this,_isDead);
        }
    }

    public string Name { get; private set; }
    public float BoostChargeTime { get; private set; }
    public PilotTactic Tactic => PilotParameters.Tactic;

    public int Id { get; private set; }
    public ShipModulsInventory Moduls;
    public ShipWeaponsInventory WeaponsModuls;
    public ShipSpellsInventory SpellsModuls;
    public OnlyModulsInventory[] SpellConnectedModules = null;
    public ParameterItem CocpitSlot { get; private set; }
    public ParameterItem EngineSlot { get; private set; }
    public ParameterItem WingSlot { get; private set; }

    private readonly PlayerSafe _player;
    private PilotParameters _pilot;

    public bool Marked { get; set; } = false;


    public ShipInventory(IStartShipParams pParams, PlayerSafe player, PilotParameters pilot)
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
        WeaponsModuls = new ShipWeaponsInventory(pParams.WeaponModulsCount, this);
//        SimpleModulsCount = pParams.SimpleModulsCount;
        Moduls = new ShipModulsInventory(pParams.SimpleModulsCount, this);
        SpellModulsCount = pParams.SpellModulsCount;
        SpellConnectedModules = new OnlyModulsInventory[SpellModulsCount];
        for (int i = 0; i < SpellModulsCount; i++)
        {
            SpellConnectedModules[i] = new OnlyModulsInventory(player);
        }
        SpellsModuls = new ShipSpellsInventory(SpellModulsCount,this);
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
                var cocpit = Library.CreateParameterItem(EParameterItemSubType.Light, EParameterItemRarity.normal, ItemType.cocpit);
                TryAddItem(cocpit);
                var engineSlot = Library.CreateParameterItem(EParameterItemSubType.Light, EParameterItemRarity.normal, ItemType.engine);
                TryAddItem(engineSlot);
                var wingSlot = Library.CreateParameterItem(EParameterItemSubType.Light, EParameterItemRarity.normal, ItemType.wings);
                TryAddItem(wingSlot);
                break;
            case ShipType.Middle:
                var cocpit1 = Library.CreateParameterItem(EParameterItemSubType.Middle, EParameterItemRarity.normal, ItemType.cocpit);
                TryAddItem(cocpit1);
                var engineSlot1 = Library.CreateParameterItem(EParameterItemSubType.Middle, EParameterItemRarity.normal, ItemType.engine);
                TryAddItem(engineSlot1);
                var wingSlot1 = Library.CreateParameterItem(EParameterItemSubType.Middle, EParameterItemRarity.normal, ItemType.wings);
                TryAddItem(wingSlot1);
                break;
            case ShipType.Heavy:
                var cocpit2 = Library.CreateParameterItem(EParameterItemSubType.Heavy, EParameterItemRarity.normal, ItemType.cocpit);
                TryAddItem(cocpit2);
                var engineSlot2 = Library.CreateParameterItem(EParameterItemSubType.Heavy, EParameterItemRarity.normal, ItemType.engine);
                TryAddItem(engineSlot2);
                var wingSlot2 = Library.CreateParameterItem(EParameterItemSubType.Heavy, EParameterItemRarity.normal, ItemType.wings);
                TryAddItem(wingSlot2);
                break;
        }
    }

    public OnlyModulsInventory GetModulsInventory(int index)
    {

        if (SpellConnectedModules != null && index < SpellConnectedModules.Length)
        {
            return SpellConnectedModules[index];
        }

        return null;
    }

    public List<IItemInv> GetAllItems()
    {
        var list = new List<IItemInv>();
        list.AddRange(WeaponsModuls.GetNonNullActiveSlots());
        list.AddRange(SpellsModuls.GetNonNullActiveSlots());
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
        return SpellsModuls.GetFreeSimpleSlot(out index);
    }

    public bool GetFreeWeaponSlot(out int index)
    {
        return WeaponsModuls.GetFreeSimpleSlot(out index);
    }

    public bool TryAddSpellModul(BaseSpellModulInv spellModul, int fieldIndex)
    {
        return SpellsModuls.TryAddSpellModul(spellModul, fieldIndex);
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
        return SpellsModuls.RemoveSpellModul(spell);
    }

    public void TransferItem(IItemInv item, bool val)
    {
        OnItemAdded?.Invoke(item, val);
    }

    public PlayerSafe Owner => _player;

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


    public int GetItemIndex(IItemInv item)
    {
        switch (item.ItemType)
        {
            case ItemType.weapon:
                return WeaponsModuls.GetItemIndex(item);
            case ItemType.modul:
                return Moduls.GetItemIndex(item);
            case ItemType.spell:
                return SpellsModuls.GetItemIndex(item);
            default:
            case ItemType.cocpit:
            case ItemType.engine:
            case ItemType.wings:
                return 0;
        }
    }
    public bool IsSlotFree(int preferableIndex, ItemType type)
    {
        switch (type)
        {
            case ItemType.weapon:
                return WeaponsModuls.IsSlotFree(preferableIndex);
            case ItemType.modul:
                return Moduls.IsSlotFree(preferableIndex);
            case ItemType.spell:
                return SpellsModuls.IsSlotFree(preferableIndex);
            default:
            case ItemType.cocpit:
            case ItemType.engine:
            case ItemType.wings:
                return false;
        }
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

    public bool CheckIsDead()
    {
        return CriticalDamages >= Library.CRITICAL_DAMAGES_TO_DEATH;
    }

    public void AddCriticalyDamage()
    {
        CriticalDamages++;
        if (CheckIsDead())
        {
            Debug.Log("Ship fully destroyed cause critical damages");
            IsDead = true;
//            MainController.Instance.MainPlayer.Army.RemoveShip(this);
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
        IsDead = false;
    }
}


