using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class LibraryModuls
{
    private static List<SimpleModulType> NormalModuls;
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
        SupportHashSet = WeaponUpgradeModuls();
        NonSupportList = NonSupportHashSet.ToList();
        SupportList = SupportHashSet.ToList();
        if (NonSupportList.Count + SupportList.Count != all.Length)
        {
            Debug.LogError($"Wrong delemiter of support {SupportList.Count} / not {NonSupportList.Count}  != all:{all.Length}");
        }


        NormalModuls = GetSimplesList().ToList();
        RareModuls = RareList().ToList();
        NotExistModuls = NotExistList().ToList();

        if (NormalModuls.Count + RareModuls.Count + NotExistModuls.Count != all.Length)
        {
            Debug.LogError($"Wrong delemiter of NormalModuls {NormalModuls.Count} + RareModuls {RareModuls.Count} + NotExistModuls {NotExistModuls.Count}  != all:{all.Length}");
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
        return NormalModuls;
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
            SimpleModulType.WeaponNoBulletDeath,
            SimpleModulType.WeaponPowerShot,
            SimpleModulType.WeaponFireNear,
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
        };
        return typesToRnd;
    }

    private static HashSet<SimpleModulType> GetSimplesList()
    {
        var typesToRnd = new HashSet<SimpleModulType>()
        {
            SimpleModulType.antiPhysical,
            SimpleModulType.antiEnergy,
            SimpleModulType.shieldLocker,
            SimpleModulType.damageMines,
            SimpleModulType.systemMines,
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
            SimpleModulType.blink,
   
            SimpleModulType.ShipSpeed,
            SimpleModulType.ShipTurnSpeed,
            SimpleModulType.shieldRegen,
            SimpleModulType.ResistDamages,
        };
        return typesToRnd;
    }


    private static HashSet<SimpleModulType> WeaponUpgradeModuls()
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
        };
        return typesToRnd;
    }

}
