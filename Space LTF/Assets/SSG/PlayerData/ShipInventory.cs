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
    public int SimpleModulsCount { get; private set; }
    public PilotParameters PilotParameters => _pilot;
    public int SpellModulsCount { get; private set; }
    public int CriticalDamages { get; private set; }
    public string Name { get; private set; }
    public float BoostChargeTime { get; private set; }
    public PilotTactic Tactic => PilotParameters.Tactic;

    public int Id { get; private set; }
    public ShipModulsInventory Moduls;
    public WeaponInv[] WeaponsModuls;
    public BaseSpellModulInv[] SpellsModuls;

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
        WeaponsModuls = new WeaponInv[WeaponModulsCount];
        SimpleModulsCount = pParams.SimpleModulsCount;
        Moduls = new ShipModulsInventory(SimpleModulsCount, this);
        SpellModulsCount = pParams.SpellModulsCount;
        SpellsModuls = new BaseSpellModulInv[SpellModulsCount];
        Name = pParams.Name;
        BoostChargeTime = pParams.BoostChargeTime;
        //        Debug.Log("ShipInventory create:" + pParams.Name);
        Id = Utils.GetId();
        //        BodyVisualType = pParams.BodyVisualType;
    }

    public List<IItemInv> GetAllItems()
    {
        var list = new List<IItemInv>();
        list.AddRange(WeaponsModuls);
        list.AddRange(SpellsModuls);
        list.AddRange(Moduls.SimpleModuls);
        return list;
    }

    public bool GetFreeSimpleSlot(out int index)
    {
        return Moduls.GetFreeSimpleSlot(out index);
    }

    public float ValuableItem(IItemInv item)
    {
        return 1f;
    }
    public bool GetFreeSpellSlot(out int index)
    {
        if (SpellsModuls == null)
        {
            index = -1;
            return false;
        }
        for (int i = 0; i < SpellsModuls.Length; i++)
        {
            var m = SpellsModuls[i];
            if (m == null)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }

    public bool GetFreeWeaponSlot(out int index)
    {
        for (int i = 0; i < WeaponsModuls.Length; i++)
        {
            var m = WeaponsModuls[i];
            if (m == null)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
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

    public bool TryAddWeaponModul(WeaponInv weaponModul, int fieldIndex)
    {
        if (weaponModul == null)
        {
            return false;
        }
        if (WeaponsModuls.Length <= fieldIndex)
        {
            Debug.LogError("Too big index weapon");
            return false;
        }
        var field = WeaponsModuls[fieldIndex];
        if (field == null)
        {
            WeaponsModuls[fieldIndex] = weaponModul;
            weaponModul.CurrentInventory = this;
            //            field = WeaponsModuls[fieldIndex];
            TransferItem(weaponModul, true);
            return true;
        }
        Debug.LogError("Slot not free");
        return false;
    }

    public bool RemoveWeaponModul(WeaponInv weaponModul)
    {
        for (int i = 0; i < WeaponsModuls.Length; i++)
        {
            var m = WeaponsModuls[i];
            if (m == weaponModul)
            {
                WeaponsModuls[i] = null;
                TransferItem(m, false);
                return true;
            }
        }
        return false;
    }

    public bool RemoveSimpleModul(BaseModulInv simpleModul)
    {
        return Moduls.RemoveSimpleModul(simpleModul);
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
        //        Debug.Log("Transfer item complete:" + item);
        if (OnItemAdded != null)
        {
            OnItemAdded(item, val);
        }
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


