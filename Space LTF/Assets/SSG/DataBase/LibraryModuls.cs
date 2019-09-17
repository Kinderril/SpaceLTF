using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class LibraryModuls
{
    private static List<SimpleModulType> CommonModuls;
    private static List<SimpleModulType> RareModuls;
    private static List<SimpleModulType> NotExistModuls;

    private static List<SimpleModulType> NonSupportList;
    private static List<SimpleModulType> SupportList;  
    
    private static HashSet<SimpleModulType> NonSupportHashSet;
    private static HashSet<SimpleModulType> SupportHashSet;

    public static void Init()
    {
        var all = (SimpleModulType[])Enum.GetValues(typeof(SimpleModulType));

        NonSupportHashSet = ShipActionModuls();
        SupportHashSet = SupportModuls();
        NonSupportList = NonSupportHashSet.ToList();
        SupportList = SupportHashSet.ToList();
        if (NonSupportList.Count + SupportList.Count != all.Length)
        {
            Debug.LogError($"Wrong delemiter of support {SupportList.Count} / not {NonSupportList.Count}  != all:{all.Length}");
        }


        CommonModuls = GetCommonList().ToList();
        RareModuls = RareList().ToList();
        NotExistModuls = NotExistList().ToList();

        if (CommonModuls.Count + RareModuls.Count + NotExistModuls.Count != all.Length)
        {
            Debug.LogError($"Wrong delemiter of CommonModuls {CommonModuls.Count} + RareModuls {RareModuls.Count} + NotExistModuls {NotExistModuls.Count}  != all:{all.Length}");
        }
    }

    public static bool IsSupport(SimpleModulType type)
    {
        return SupportHashSet.Contains(type);
    }
    public static bool IsRare(SimpleModulType type)
    {
        return RareModuls.Contains(type);
    }

    public static List<SimpleModulType> GetNormalList()
    {
        return CommonModuls;
    }   
//    public static List<SimpleModulType> GetUpgradesList()
//    {
//        return UpgradesModuls;
//    }  
    public static List<SimpleModulType> GetRareList()
    {
        return RareModuls;
    }


    private static HashSet<SimpleModulType> All()
    {
        var typesToRnd = new HashSet<SimpleModulType>()
        {
            SimpleModulType.autoRepair,
            SimpleModulType.autoShieldRepair,
            SimpleModulType.closeStrike,
            SimpleModulType.antiPhysical,
            SimpleModulType.antiEnergy,
            SimpleModulType.shieldLocker,
            SimpleModulType.engineLocker,
            SimpleModulType.damageMines,
            SimpleModulType.fireMines,
            SimpleModulType.frontShield,
            SimpleModulType.systemMines,
            SimpleModulType.blink,
            SimpleModulType.laserUpgrade,
            SimpleModulType.beamUpgrade,
            SimpleModulType.rocketUpgrade,
            SimpleModulType.EMIUpgrade,
            SimpleModulType.impulseUpgrade,
            SimpleModulType.bombUpgrade,
            SimpleModulType.ShipSpeed,
            SimpleModulType.ShipTurnSpeed,
            SimpleModulType.shieldRegen,
            SimpleModulType.ResistDamages,

            SimpleModulType.WeaponSpeed,
            SimpleModulType.WeaponSpray,
            SimpleModulType.WeaponDist,
            SimpleModulType.WeaponPush,
            SimpleModulType.WeaponFire,
            SimpleModulType.WeaponEngine,
            SimpleModulType.WeaponShield,
            SimpleModulType.WeaponWeapon,
            SimpleModulType.WeaponCrit,
            SimpleModulType.WeaponAOE,
            SimpleModulType.WeaponSector,
            SimpleModulType.WeaponLessDist,
            SimpleModulType.WeaponChain,
            SimpleModulType.WeaponShieldIgnore,
            SimpleModulType.WeaponSelfDamage,
            SimpleModulType.WeaponShieldPerHit,
            SimpleModulType.WeaponShootPerTime,
            SimpleModulType.WeaponNoBulletDeath,
            SimpleModulType.WeaponPowerShot,
            SimpleModulType.WeaponFireNear,
            SimpleModulType.ShipDecreaseSpeed,
            SimpleModulType.ShieldDouble,
        };
        return typesToRnd;
    }
    private static HashSet<SimpleModulType> NotExistList()
    {
        var typesToRnd = new HashSet<SimpleModulType>()
        {
            SimpleModulType.laserUpgrade,
            SimpleModulType.rocketUpgrade,
            SimpleModulType.EMIUpgrade,
            SimpleModulType.impulseUpgrade,
            SimpleModulType.bombUpgrade,
            SimpleModulType.beamUpgrade,
        };
        return typesToRnd;
    }

    private static HashSet<SimpleModulType> RareList()
    {
        var typesToRnd = new HashSet<SimpleModulType>()
        {
            SimpleModulType.engineLocker,
            SimpleModulType.systemMines,
            SimpleModulType.autoRepair,
            SimpleModulType.autoShieldRepair,
            SimpleModulType.blink,
            SimpleModulType.ResistDamages,
            SimpleModulType.WeaponSpray,
            SimpleModulType.WeaponFire,
            SimpleModulType.WeaponEngine,
            SimpleModulType.WeaponCrit,
            SimpleModulType.WeaponAOE,
            SimpleModulType.WeaponShieldIgnore,
            SimpleModulType.WeaponChain,
            SimpleModulType.WeaponNoBulletDeath,
            SimpleModulType.WeaponFireNear,
            SimpleModulType.ShieldDouble,
            SimpleModulType.WeaponShootPerTime,
        };
        return typesToRnd;
    }

    private static HashSet<SimpleModulType> GetCommonList()
    {
        var typesToRnd = new HashSet<SimpleModulType>()
        {
            SimpleModulType.antiPhysical,
            SimpleModulType.antiEnergy,
            SimpleModulType.shieldLocker,
            SimpleModulType.damageMines,
            SimpleModulType.ShipSpeed,
            SimpleModulType.ShipTurnSpeed,
            SimpleModulType.shieldRegen,
            SimpleModulType.closeStrike,
            SimpleModulType.WeaponSpeed,
            SimpleModulType.WeaponDist,
            SimpleModulType.WeaponShield,
            SimpleModulType.WeaponWeapon,
            SimpleModulType.WeaponSector,
            SimpleModulType.WeaponLessDist,
            SimpleModulType.WeaponSelfDamage,
            SimpleModulType.WeaponShieldPerHit,
            SimpleModulType.WeaponPowerShot,
            SimpleModulType.WeaponPush,
            SimpleModulType.ShipDecreaseSpeed,
            SimpleModulType.fireMines,


        };
        return typesToRnd;
    }


    private static HashSet<SimpleModulType> ShipActionModuls()
    {
        var typesToRnd = new HashSet<SimpleModulType>()
        {
            SimpleModulType.autoRepair,
            SimpleModulType.autoShieldRepair,
            SimpleModulType.closeStrike,
            SimpleModulType.antiPhysical,
            SimpleModulType.antiEnergy,
            SimpleModulType.shieldLocker,
            SimpleModulType.engineLocker,
            SimpleModulType.damageMines,
            SimpleModulType.systemMines,
            SimpleModulType.frontShield,
            SimpleModulType.fireMines,
            SimpleModulType.blink,
            SimpleModulType.ResistDamages,
            SimpleModulType.shieldRegen,

        };
        return typesToRnd;
    }


    private static HashSet<SimpleModulType> SupportModuls()
    {
        var typesToRnd = new HashSet<SimpleModulType>()
        {
            SimpleModulType.WeaponSpeed,
            SimpleModulType.WeaponSpray,
            SimpleModulType.WeaponDist,

            SimpleModulType.WeaponPush,
            SimpleModulType.WeaponFire,
            SimpleModulType.WeaponEngine,
            SimpleModulType.WeaponShield,
            SimpleModulType.WeaponWeapon,

            SimpleModulType.WeaponCrit,
            SimpleModulType.WeaponAOE,
            SimpleModulType.WeaponSector,
            SimpleModulType.WeaponLessDist,
            SimpleModulType.WeaponChain,

            SimpleModulType.WeaponShieldIgnore,
            SimpleModulType.WeaponSelfDamage,
            SimpleModulType.WeaponShootPerTime,
            SimpleModulType.WeaponShieldPerHit,
            SimpleModulType.WeaponNoBulletDeath,
            SimpleModulType.WeaponPowerShot,

            SimpleModulType.WeaponFireNear,

            SimpleModulType.laserUpgrade,
            SimpleModulType.rocketUpgrade,
            SimpleModulType.EMIUpgrade,
            SimpleModulType.impulseUpgrade,
            SimpleModulType.bombUpgrade,
            SimpleModulType.beamUpgrade,

            SimpleModulType.ShipSpeed,
            SimpleModulType.ShipTurnSpeed,
//            SimpleModulType.ResistDamages,
            SimpleModulType.ShipDecreaseSpeed,
            SimpleModulType.ShieldDouble,
        };
        return typesToRnd;
    }

}
