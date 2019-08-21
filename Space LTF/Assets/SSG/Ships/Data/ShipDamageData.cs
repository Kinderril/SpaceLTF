using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public enum ShipDamageType
{
    engine = 0,
//    turnEngine = 1,
    weapon = 2,
    shiled = 3,
//    moduls,
    fire = 4
}

public class ShipDamageData : ShipData
{
    private EngineStop _engineStop;
    private WeaponsController _weaponsController;
    private Dictionary<ShipDamageType,bool> damagedDetails = new Dictionary<ShipDamageType, bool>();
    public event Action<ShipBase,ShipDamageType, bool> OnDamageDone;
    public bool IsReflecOn = false;

    public ShipDamageData([NotNull] ShipBase owner) 
        : base(owner)
    {
        var values3 = Enum.GetValues(typeof(ShipDamageType));
        foreach (ShipDamageType v in values3)
        {
            damagedDetails.Add(v,false);
        }
    }

    public void Activate()
    {
        _engineStop = _owner.EngineStop;
        _weaponsController = _owner.WeaponsController;
    }

    public bool HaveSomeDamage()
    {
        foreach (var damagedDetail in damagedDetails)
        {
            if (damagedDetail.Value)
            {
                return true;
            }
        }
        return false;
    }

    public void ApplyEffect(ShipDamageType damageType, float time = -1, bool unresistable = false)
    {
        if (IsReflecOn)
        {
            return;
        }
//        if (!unresistable)
//        {
//            if (_owner.ShipParameters.ShieldParameters.ShiledIsActive && damageType != ShipDamageType.shiled)
//            {
//                return;
//            }
//        }

        damagedDetails[damageType] = true;
        var info = Namings.Damage(damageType, time);
        _owner.Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.GetDamageSpell(damageType));

        switch (damageType)
        {
            case ShipDamageType.engine:
                FlyNumberWithDependence.Create(_owner.transform, info, Color.red, FlyNumerDirection.right);
                _engineStop.Stop(time);
                break;
//            case ShipDamageType.turnEngine:
//                break;
            case ShipDamageType.weapon:
                FlyNumberWithDependence.Create(_owner.transform, info, Color.red, FlyNumerDirection.right);
                _weaponsController.CrashAllWeapons(true);
                break;
            case ShipDamageType.shiled:
                FlyNumberWithDependence.Create(_owner.transform, info, Color.red, FlyNumerDirection.right);
                _owner.ShipParameters.ShieldParameters.Crash();
                break;
            case ShipDamageType.fire:
                FlyNumberWithDependence.Create(_owner.transform, info, Color.red, FlyNumerDirection.right);
                _owner.PeriodDamage.Start((int)time);
                break;
        }
        if (OnDamageDone != null)
        {
            OnDamageDone(_owner,damageType, true);
        }
        if (time > 0)
        {
            
        }
    }

    public void RepairAll()
    {
        foreach (var damagedDetail in damagedDetails)
        {
            if (damagedDetail.Value)
            {
                Repair(damagedDetail.Key);
            }
        }
    }

    private void Repair(ShipDamageType key)
    {
        damagedDetails[key] = false;
        switch (key)
        {
            case ShipDamageType.engine:
                _engineStop.Start();
                break;
//            case ShipDamageType.turnEngine:
//                //TODO
//                break;
            case ShipDamageType.weapon:
                _weaponsController.CrashAllWeapons(false);
                break;
            case ShipDamageType.shiled:
                _owner.ShipParameters.ShieldParameters.Repair();
                break;
            case ShipDamageType.fire:
                _owner.PeriodDamage.Stop();
                break;
        }
    }
}
