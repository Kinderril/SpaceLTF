using System.Collections.Generic;

public static class ArmyCreatorLibrary
{

    private static Dictionary<ShipConfig, List<ArmyCreatorData>> _datas;

    public static void Init()
    {
        _datas = new Dictionary<ShipConfig, List<ArmyCreatorData>>();
        var merc1 = new ArmyCreatorData(new List<ShipConfig>() { ShipConfig.mercenary },
            new List<WeaponType>() { WeaponType.laser },
            new List<SimpleModulType>() { SimpleModulType.ShipTurnSpeed, SimpleModulType.antiPhysical, SimpleModulType.antiEnergy, SimpleModulType.WeaponPush },
            new List<SimpleModulType>() { SimpleModulType.ResistDamages, SimpleModulType.WeaponPowerShot },
            new List<SpellType>() { SpellType.machineGun, SpellType.throwAround });
        var merc2 = new ArmyCreatorData(new List<ShipConfig>() { ShipConfig.mercenary },
            new List<WeaponType>() { WeaponType.rocket },
            new List<SimpleModulType>() { SimpleModulType.ShipTurnSpeed, SimpleModulType.antiPhysical, SimpleModulType.antiEnergy, SimpleModulType.WeaponPush },
            new List<SimpleModulType>() { SimpleModulType.ResistDamages, SimpleModulType.WeaponPowerShot },
            new List<SpellType>() { SpellType.machineGun, SpellType.throwAround });

        var raidFireRockets = new ArmyCreatorData(new List<ShipConfig>() { ShipConfig.raiders },
            new List<WeaponType>() { WeaponType.rocket },
            new List<SimpleModulType>() { SimpleModulType.WeaponSector, SimpleModulType.WeaponDist, SimpleModulType.ShipTurnSpeed },
            new List<SimpleModulType>() { SimpleModulType.shieldLocker, SimpleModulType.WeaponShield },
            new List<SpellType>() { SpellType.lineShot, SpellType.shildDamage });
        var raidCassetBurner = new ArmyCreatorData(new List<ShipConfig>() { ShipConfig.raiders },
            new List<WeaponType>() { WeaponType.casset },
            new List<SimpleModulType>() { SimpleModulType.WeaponSector, SimpleModulType.WeaponEngine, SimpleModulType.ShipSpeed },
            new List<SimpleModulType>() { SimpleModulType.WeaponFire, SimpleModulType.fireMines, SimpleModulType.WeaponFireNear },
            new List<SpellType>() { SpellType.lineShot, SpellType.shildDamage });

        var fed = new ArmyCreatorData(new List<ShipConfig>() { ShipConfig.federation },
            new List<WeaponType>() { WeaponType.impulse },
            new List<SimpleModulType>() { SimpleModulType.WeaponSpeed, SimpleModulType.damageMines },
            new List<SimpleModulType>() { SimpleModulType.WeaponCrit, SimpleModulType.WeaponSpray, SimpleModulType.autoRepair },
            new List<SpellType>() { SpellType.artilleryPeriod, SpellType.engineLock });

        var fedManyLasers = new ArmyCreatorData(new List<ShipConfig>() { ShipConfig.federation },
            new List<WeaponType>() { WeaponType.laser },
            new List<SimpleModulType>() { SimpleModulType.WeaponSpeed, SimpleModulType.systemMines },
            new List<SimpleModulType>() { SimpleModulType.WeaponMultiTarget, SimpleModulType.WeaponSpray, SimpleModulType.WeaponShootPerTime },
            new List<SpellType>() { SpellType.artilleryPeriod, SpellType.engineLock });

        var kriosEMIShoots = new ArmyCreatorData(new List<ShipConfig>() { ShipConfig.krios },
            new List<WeaponType>() { WeaponType.eimRocket },
            new List<SimpleModulType>() { SimpleModulType.engineLocker, SimpleModulType.systemMines },
            new List<SimpleModulType>() { SimpleModulType.WeaponCrit, SimpleModulType.WeaponShootPerTime, SimpleModulType.ShieldDouble },
            new List<SpellType>() { SpellType.mineField, SpellType.rechargeShield });

        var kriosImpulse = new ArmyCreatorData(new List<ShipConfig>() { ShipConfig.krios },
            new List<WeaponType>() { WeaponType.impulse },
            new List<SimpleModulType>() { SimpleModulType.frontShield, SimpleModulType.ShipDecreaseSpeed },
            new List<SimpleModulType>() { SimpleModulType.WeaponCrit, SimpleModulType.blink, SimpleModulType.ShieldDouble },
            new List<SpellType>() { SpellType.mineField, SpellType.rechargeShield });

        var ocrons1 = new ArmyCreatorData(new List<ShipConfig>() { ShipConfig.ocrons },
            new List<WeaponType>() { WeaponType.eimRocket },
            new List<SimpleModulType>() { SimpleModulType.shieldLocker, SimpleModulType.ShipSpeed },
            new List<SimpleModulType>() { SimpleModulType.armor, SimpleModulType.WeaponLessDist },
            new List<SpellType>() { SpellType.distShot, SpellType.repairDrones });

        var ocrons2 = new ArmyCreatorData(new List<ShipConfig>() { ShipConfig.ocrons },
            new List<WeaponType>() { WeaponType.casset },
            new List<SimpleModulType>() { SimpleModulType.antiPhysical, SimpleModulType.antiEnergy, SimpleModulType.ShipSpeed },
            new List<SimpleModulType>() { SimpleModulType.armor, SimpleModulType.WeaponSelfDamage },
            new List<SpellType>() { SpellType.distShot, SpellType.repairDrones });

        var droids = new ArmyCreatorData(new List<ShipConfig>() { ShipConfig.droid },
            new List<WeaponType>() { WeaponType.casset, WeaponType.laser, WeaponType.eimRocket, WeaponType.rocket, WeaponType.casset, WeaponType.impulse },
            new List<SimpleModulType>() { SimpleModulType.ShipSpeed, SimpleModulType.ShipDecreaseSpeed, SimpleModulType.ShipTurnSpeed },
            new List<SimpleModulType>() { SimpleModulType.armor, SimpleModulType.WeaponCrit, SimpleModulType.WeaponSpray, },
            new List<SpellType>() { SpellType.distShot, SpellType.repairDrones });

        _datas.Add(ShipConfig.mercenary, new List<ArmyCreatorData>() { merc1, merc2 });
        _datas.Add(ShipConfig.raiders, new List<ArmyCreatorData>() { raidFireRockets, raidCassetBurner });
        _datas.Add(ShipConfig.federation, new List<ArmyCreatorData>() { fed, fedManyLasers });
        _datas.Add(ShipConfig.krios, new List<ArmyCreatorData>() { kriosEMIShoots, kriosImpulse });
        _datas.Add(ShipConfig.ocrons, new List<ArmyCreatorData>() { ocrons1, ocrons2 });
        _datas.Add(ShipConfig.droid, new List<ArmyCreatorData>() { droids });

    }

    public static ArmyCreatorData GetArmy(ShipConfig config)
    {
        var data = _datas[config].RandomElement();
        var copy = data.Copy();
        return copy;
    }
    public static ArmyCreatorData GetArmy(ShipConfig config1, ShipConfig config2)
    {
        var data1 = _datas[config1].RandomElement();
        var data2 = _datas[config2].RandomElement();
        ArmyCreatorData copy = data1.Merge(data2);
        return copy;
    }


}

