using System;
using System.Collections.Generic;
using UnityEngine;

public enum ELocTag
{
    English,
    Russian ,
    Spain ,
}

public static class Namings
{
    private const string LANG_KEY = "LANG_KEY";

    public static string Format(string core, params object[] args)
    {
        try
        {
            var result = String.Format(core, args);
            return result;
        }
        catch (Exception e)
        {
            Debug.LogError($"Wrong string format core:{core}");

        }

        return core;
    }

    private static Dictionary<string, string> _curLocalization;
    public static ELocTag LocTag;

    public static void Init()
    {
        var key = PlayerPrefs.GetInt(LANG_KEY, 1);
        switch (key)
        {
            case 2:
                Rus();
                break;   
            case 3:
                Esp();
                break;
            default:
                English();
                break;
        }

        CheckLocals();
    }

    private static void CheckLocals()
    {
        var eng = EngLocalization._locals;
        var rus = RusLocalization._locals;
        var esp = EspLocalization._locals;

#if UNITY_EDITOR
        if (eng.Count != rus.Count || eng.Count != esp.Count)
        {
            foreach (var v in eng)
            {
                if (!rus.ContainsKey(v.Key))
                {
                    Debug.LogError($"Rus don't have key {v.Key}");
                }
            }
            foreach (var v in rus)
            {
                if (!eng.ContainsKey(v.Key))
                {
                    Debug.LogError($"Eng don't have key {v.Key}");
                }
            }
        }
#endif


    }

    public static string TagByType(ELocTag t,string tag)
    {
        switch (t)
        {
            case ELocTag.English:
                if (EngLocalization._locals.TryGetValue(tag, out var tag1))
                {
                    return tag1;
                }
                break;
            case ELocTag.Russian:
                if (RusLocalization._locals.TryGetValue(tag, out var tag2))
                {
                    return tag2;
                }
                break;  
            case ELocTag.Spain:
                if (EspLocalization._locals.TryGetValue(tag, out var tag3))
                {
                    return tag3;
                }
                break;
        }
        return $"Error:{tag}";

    }   

    public static void English()
    {
        PlayerPrefs.SetInt(LANG_KEY, 1);
        LocTag = ELocTag.English;
        _curLocalization = EngLocalization._locals;
    }
    public static void Rus()
    {
        PlayerPrefs.SetInt(LANG_KEY, 2);
        LocTag = ELocTag.Russian;
        _curLocalization = RusLocalization._locals;
    }   
    public static void Esp()
    {
        PlayerPrefs.SetInt(LANG_KEY, 3);
        LocTag = ELocTag.Spain;
        _curLocalization = EspLocalization._locals;
    }

    public static string DialogTag(string tag)
    {
        return Tag($"dialog_{tag}");
    }
    public static string Tag(string tag)
    {
        if (_curLocalization.TryGetValue(tag, out var info))
        {
            return info;
        }
#if UNITY_EDITOR
        Debug.LogError($"localization error  tag:{tag}");
#endif
        return $"ERROR:{tag}";
    }


    public static string ShipConfig(ShipConfig config)
    {
        switch (config)
        {
            case global::ShipConfig.raiders:
                return Tag("Raiders");
            case global::ShipConfig.federation:
                return Tag("Federation");
            case global::ShipConfig.mercenary:
                return Tag("Mercenary");
            case global::ShipConfig.ocrons:
                return Tag("Ocrons");
            case global::ShipConfig.krios:
                return Tag("Krios");
            case global::ShipConfig.droid:
                return Tag("Droids");
        }
        return "none";
    }
    public static string ShipType(ShipType t)
    {
        switch (t)
        {
            case global::ShipType.Light:
                return Tag("Light");
                break;
            case global::ShipType.Middle:
                return Tag("Medium");
                break;
            case global::ShipType.Heavy:
                return Tag("Heavy");
                break;
            case global::ShipType.Base:
                return Tag("Base");
            case global::ShipType.Turret:
                return Tag("Turret");
                break;
        }
        return "none";
    }
    public static string Weapon(WeaponType config)
    {
        switch (config)
        {
            case WeaponType.laser:
                return Tag("Laser");
            case WeaponType.rocket:
                return Tag("Rocket");
            case WeaponType.impulse:
                return Tag("Impulse");
            case WeaponType.casset:
                return Tag("Swarm");
            case WeaponType.eimRocket:
                return Tag("EMI");
            case WeaponType.beam:
                return Tag("Beam");
            case WeaponType.healBodySupport:
                return Tag("healBodySupport");
            case WeaponType.healShieldSupport:
                return Tag("healShieldSupport");
        }
        var error = $"none.{config.ToString()} ___  {config}";
        Debug.LogError(error);
        return error;
    }
    public static string SimpleModulName(SimpleModulType config)
    {
        string Name = "none";
        switch (config)
        {
            case SimpleModulType.shieldLocker:
                Name = Tag("Shield Locker");
                break;
            case SimpleModulType.autoShieldRepair:
                Name = Tag("Auto shield");
                break;
            case SimpleModulType.shieldRegen:
                Name = Tag("Shield regeneration");
                break;
            case SimpleModulType.autoRepair:
                Name = Tag("Repair drone");
                break;
            case SimpleModulType.antiPhysical:
                Name = Tag("Rocked catcher");
                break;
            case SimpleModulType.antiEnergy:
                Name = Tag("Deflector");
                break;
//            case SimpleModulType.closeStrike:
//                Name = Tag("Side strike");
                break;
            case SimpleModulType.engineLocker:
                Name = Tag("Engine locker");
                break;
            case SimpleModulType.systemMines:
                Name = Tag("EMI mine");
                break;
            case SimpleModulType.damageMines:
                Name = Tag("Power mine");
                break;
            case SimpleModulType.fireMines:
                Name = Tag("Fire mine");
                break;
            case SimpleModulType.frontShield:
                Name = Tag("Front shield");
                break;
            case SimpleModulType.armor:
                Name = Tag("Body armor");
                break;
            case SimpleModulType.blink:
                Name = Tag("Teleportation");
                break;
            case SimpleModulType.laserUpgrade:
                Name = Tag("Laser power");
                break;
            case SimpleModulType.rocketUpgrade:
                Name = Tag("Rockets power");
                break;
            case SimpleModulType.bombUpgrade:
                Name = Tag("Bomb power");
                break;
            case SimpleModulType.EMIUpgrade:
                Name = Tag("EMI power");
                break;
            case SimpleModulType.impulseUpgrade:
                Name = Tag("Impulse power");
                break;
            case SimpleModulType.ShipSpeed:
                Name = Tag("Speed");
                break;
            case SimpleModulType.ShipTurnSpeed:
                Name = Tag("Turn Speed");
                break;
            case SimpleModulType.WeaponSpeed:
                Name = Tag("Bullet speed");
                break;
            case SimpleModulType.WeaponSpray:
                Name = Tag("Spray fire");
                break;
            case SimpleModulType.WeaponDist:
                Name = Tag("Longer dist");
                break;
            case SimpleModulType.WeaponPush:
                Name = Tag("Pusher");
                break;
            case SimpleModulType.WeaponFire:
                Name = Tag("Burner");
                break;
            case SimpleModulType.WeaponEngine:
                Name = Tag("Stopper");
                break;
            case SimpleModulType.WeaponShield:
                Name = Tag("Locker");
                break;
            case SimpleModulType.WeaponCrit:
                Name = Tag("Critical");
                break;
            case SimpleModulType.WeaponAOE:
                Name = Tag("AOE");
                break;
            case SimpleModulType.WeaponSector:
                Name = Tag("Wider");
                break;
            case SimpleModulType.WeaponLessDist:
                Name = Tag("Close strike");
                break;
            case SimpleModulType.WeaponMultiTarget:
                Name = Tag("Chain strike");
                break;
            case SimpleModulType.WeaponNoBulletDeath:
                Name = Tag("Penetrating shot");
                break;
            case SimpleModulType.WeaponSelfDamage:
                Name = Tag("Powerful recoil");
                break;
            case SimpleModulType.WeaponShieldPerHit:
                Name = Tag("Shield stealer");
                break;
            case SimpleModulType.WeaponShootPerTime:
                Name = Tag("More bullets");
                break;
            case SimpleModulType.WeaponPowerShot:
                Name = Tag("Power shot");
                break;
            case SimpleModulType.WeaponFireNear:
                Name = Tag("Fire wave");
                break;
            case SimpleModulType.ResistDamages:
                Name = Tag("Protector");
                break;
            case SimpleModulType.beamUpgrade:
                Name = Tag("Beam upgrade");
                break;
            case SimpleModulType.ShipDecreaseSpeed:
                Name = Tag("Heavy weapons");
                break;
            case SimpleModulType.ShieldDouble:
                Name = Tag("Maximum shield");
                break;
            case SimpleModulType.WeaponShieldIgnore:
                Name = Tag("Ignore shield");
                break;
            default:
                Debug.LogError($"NO NAME {config.ToString()}");
                break;

        }
        return Name;
    }
    public static string DescSimpleModul(SimpleModulType config)
    {
        return Tag(config.ToString());
    }
    public static string SpellName(SpellType spellType)
    {
        return Tag(spellType.ToString());
    }
    public static string ParameterName(LibraryPilotUpgradeType value)
    {
        switch (value)
        {
            case LibraryPilotUpgradeType.health:
                return Tag("Health");
            case LibraryPilotUpgradeType.shield:
                return Tag("Shield");
            case LibraryPilotUpgradeType.speed:
                return Tag("Speed");
            case LibraryPilotUpgradeType.turnSpeed:
                return Tag("TurnSpeed");
        }
        return "todo";
    }
    public static string ParameterName(PlayerParameterType type)
    {
        return Tag(type.ToString());
    }
    public static string ActionName(ActionType arg2ActionType)
    {
        switch (arg2ActionType)
        {
            case ActionType.readyToAttack:
                return Tag("Preparing");
            case ActionType.shootFromPlace:
                return Tag("Attack");
            case ActionType.attack:
                return Tag("Attack");
            // case ActionType.attackHalfLoop:
            //     return Tag("AttackHalfLoop");
            case ActionType.moveToBase:
                break;
            case ActionType.returnToBattle:
                return Tag("Return");
            case ActionType.closeStrikeAction:
                break;
            case ActionType.evade:
                return Tag("Evade enemy");
            case ActionType.afterAttack:
                return Tag("Evade");
            case ActionType.waitHeal:
                break;
            case ActionType.defence:
                break;
            case ActionType.mineField:
                break;
            case ActionType.attackSide:
                return Tag("FlangAttack");
            case ActionType.repairAction:
                return Tag("RepairAction");
            case ActionType.waitEnemy:
                return Tag("Wait");
            case ActionType.goToCurrentPointAction:
                break;
            case ActionType.goToHide:
                return "Hiding";
            case ActionType.waitEnemySec:
                return Tag("Wait");
            case ActionType.waitEdnGame:
                return Tag("Wait");
        }
        return "";
    }
    public static string Damage(ShipDamageType damageType, float time)
    {
        switch (damageType)
        {
            case ShipDamageType.engine:
                return Namings.Format(Tag("engineCrash"), time);
            case ShipDamageType.shiled:
                return Namings.Format(Tag("shieldCrash"), time);
            case ShipDamageType.fire:
                return Namings.Format(Tag("fireCrash"), time);
            default:
                return "null";
        }
    }
    public static string QuestName(EQuestOnStart type)
    {
        switch (type)
        {
            case EQuestOnStart.killLight:
                return Tag("killLight");
            case EQuestOnStart.killMed:
                return Tag("killMed");
            case EQuestOnStart.killHeavy:
                return Tag("killHeavy");
            case EQuestOnStart.mainShipKills:
                return Tag("mainShipKills");
            case EQuestOnStart.upgradeWeapons:
                return Tag("upgradeWeapons");
            case EQuestOnStart.sellModuls:
                return Tag("sellModuls");
            case EQuestOnStart.laserDamage:
                return Namings.Format(Tag("damageByQuest"), Weapon(WeaponType.laser));
            case EQuestOnStart.rocketDamage:
                return Namings.Format(Tag("damageByQuest"), Weapon(WeaponType.rocket));
            case EQuestOnStart.impulseDamage:
                return Namings.Format(Tag("damageByQuest"), Weapon(WeaponType.impulse));
            case EQuestOnStart.emiDamage:
                return Namings.Format(Tag("damageByQuest"), Weapon(WeaponType.eimRocket));
            case EQuestOnStart.cassetDamage:
                return Namings.Format(Tag("damageByQuest"), Weapon(WeaponType.casset));
            case EQuestOnStart.winRaiders:
                return Namings.Format(Tag("winAgainst"), Namings.ShipConfig(global::ShipConfig.raiders));
            case EQuestOnStart.winMerc:
                return Namings.Format(Tag("winAgainst"), Namings.ShipConfig(global::ShipConfig.mercenary));
            case EQuestOnStart.winFed:
                return Namings.Format(Tag("winAgainst"), Namings.ShipConfig(global::ShipConfig.federation));
            case EQuestOnStart.winKrios:
                return Namings.Format(Tag("winAgainst"), Namings.ShipConfig(global::ShipConfig.krios));
            case EQuestOnStart.winOcrons:
                return Namings.Format(Tag("winAgainst"), Namings.ShipConfig(global::ShipConfig.ocrons));
            case EQuestOnStart.winDroids:
                return Namings.Format(Tag("winAgainst"), Namings.ShipConfig(global::ShipConfig.droid));
            case EQuestOnStart.killRaiders:
                return Namings.Format(Tag("questKills"), Namings.ShipConfig(global::ShipConfig.raiders));
            case EQuestOnStart.killMerc:
                return Namings.Format(Tag("questKills"), Namings.ShipConfig(global::ShipConfig.mercenary));
            case EQuestOnStart.killFed:
                return Namings.Format(Tag("questKills"), Namings.ShipConfig(global::ShipConfig.federation));
            case EQuestOnStart.killKrios:
                return Namings.Format(Tag("questKills"), Namings.ShipConfig(global::ShipConfig.krios));
            case EQuestOnStart.killOcrons:
                return Namings.Format(Tag("questKills"), Namings.ShipConfig(global::ShipConfig.ocrons));
            case EQuestOnStart.killDroids:
                return Namings.Format(Tag("questKills"), Namings.ShipConfig(global::ShipConfig.droid));
        }
        return $"ERROR{type.ToString()}";
    }
    public static string TooltipWeapons(WeaponsPair weaponsPair)
    {
        return Namings.Format(Tag("WeaponsStart"), Weapon(weaponsPair.Part1),
            Weapon(weaponsPair.Part2));
    }

    public static string TooltipConfigProsCons(ShipConfig config)
    {
        switch (config)
        {
            case global::ShipConfig.raiders:
                return Namings.Format(Tag("RaidersProCons"));
            case global::ShipConfig.federation:
                return Format(Tag("FederationProCons"));
            case global::ShipConfig.mercenary:
                return Format(Tag("MercenaryProCons"));
            case global::ShipConfig.ocrons:
                return Format(Tag("OcronsProCons"));
            case global::ShipConfig.krios:
                return Format(Tag("KriosProCons"));
            case global::ShipConfig.droid:
                return "";
        }

        return "null";

    }

    public static string TooltipConfigProsConsCalc(ShipConfig config)
    {
        switch (config)
        {
            case global::ShipConfig.raiders:
                return Namings.Format(Tag("RaidersProConsCalc"),Utils.FloatToChance(Library.RAIDER_SPEED_COEF -1f),Utils.FloatToChance(Library.RAIDER_TURNSPEED_COEF -1f));
            case global::ShipConfig.federation:
                return Format(Tag("FederationProConsCalc"));
            case global::ShipConfig.mercenary:
                return Format(Tag("MercenaryProConsCalc"), Utils.FloatToChance(Library.MERC_SPEED_COEF - 1f), Utils.FloatToChance(Library.MERC_TURNSPEED_COEF - 1f));
            case global::ShipConfig.ocrons:
                return Format(Tag("OcronsProConsCalc"),Utils.FloatToChance(Library.OCRONS_HP_COEF -1f));
            case global::ShipConfig.krios:
                return Format(Tag("KriosProConsCalc"), Utils.FloatToChance(Library.KRIOS_SHIELD_COEF - 1f), Utils.FloatToChance(Library.KRIOS_HP_COEF - 1f));
            case global::ShipConfig.droid:
                return "";
        }

        return "null";

    }

    public static string TooltipParam(PlayerParameterType type)
    {
        switch (type)
        {
            case PlayerParameterType.scout:
                return Tag("Paramscout");
            case PlayerParameterType.repair:
                return Tag("Paramrepair");
            case PlayerParameterType.chargesCount:
                return Tag("ParamchargesCount");
            case PlayerParameterType.chargesSpeed:
                return Tag("ParamchargesSpeed");
            case PlayerParameterType.engineParameter:
                return Tag("ParamEngine");
        }

        return "null";
    }

    public static string BattleEvent(BattlefildEventType? eventType)
    {
        switch (eventType)
        {
            case BattlefildEventType.asteroids:
                return Tag("asteroidsEvent");
            case BattlefildEventType.shieldsOff:
                return Tag("shieldsOffEvent");
            // case BattlefildEventType.engineOff:
            //     return Tag("engineOffEvent");
            case BattlefildEventType.fireVortex:
                return Tag("fireVortexEvent");
            case BattlefildEventType.Vortex:
                return Tag("VortexEvent");   
            case BattlefildEventType.BlackHole:
                return Tag("BlackHoleEvent");     
            case BattlefildEventType.IceZone:
                return Tag("IceZoneEvent");
            case null:
                break;
        }

        return $"Error {eventType.ToString()}";
    }





    public static string KeyKode(KeyCode descKey)
    {
        switch (descKey)
        {
            case KeyCode.Q:
                return "Select main ship";
            case KeyCode.Alpha1:
                return "Select spell 1";
            case KeyCode.Alpha2:
                return "Select spell 2";
            case KeyCode.Alpha3:
                return "Select spell 3";
            case KeyCode.Alpha4:
                return "Select spell 4";
            case KeyCode.Alpha5:
                return "Select spell 5";
            case KeyCode.Alpha6:
                return "Select spell 6";
            case KeyCode.Space:
                return "Pause/Unpause";
            case KeyCode.Tab:
                return "Choose ship";
            case KeyCode.W:
                return "Move camera Up";
            case KeyCode.S:
                return "Move camera down";
            case KeyCode.A:
                return "Move camera left";
            case KeyCode.D:
                return "Move camera right";

        }

        return "null";
    }


    public static string ParameterModulName(ItemType modulItemType)
    {
        switch (modulItemType)
        {
            case ItemType.weapon:
                break;
            case ItemType.modul:
                break;
            case ItemType.spell:
                break;
            case ItemType.cocpit:
                return Namings.Tag("cocpit");
                break;
            case ItemType.engine:
                return Namings.Tag("engine");
                break;
            case ItemType.wings:
                return Namings.Tag("wings");
                break;
        }

        return $"error:{modulItemType.ToString()}";
    }
}