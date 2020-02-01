using System;
using UnityEngine;


public static class Namings
{
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
        }
        return $"none.{config.ToString()} ___  {config}";
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
            case SimpleModulType.closeStrike:
                Name = Tag("Side strike");
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
            case ActionType.attack:
                return Tag("Attack");
            case ActionType.attackHalfLoop:
                return Tag("AttackHalfLoop");
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
                return Tag("Attack");
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

    public static string Fleet = "Fleet";
    public static string RechargeButton = "Recharge Shield.\nCost[{0}/{1}]";
    public static string BuffButton = "Increase parameters of ship.\n[Cost:{0}/{1}]";
    public static string PowerWeaponButton = "Increase next shoot damage of ship.\n[Cost:{0}/{1}]";
    public static string CellScouted = "Coordinates Scouted [{0},{1}]";

    public static string QuestName(EQuestOnStart type)
    {
        switch (type)
        {
            case EQuestOnStart.killLight:
                return "Light ships kills";
            case EQuestOnStart.killMed:
                return "Medium ships kills";
            case EQuestOnStart.killHeavy:
                return "Heavy ships kills";
            case EQuestOnStart.killRaiders:
                return $"{Namings.ShipConfig(global::ShipConfig.raiders)} kills";
            case EQuestOnStart.killMerc:
                return $"{Namings.ShipConfig(global::ShipConfig.mercenary)} kills";
            case EQuestOnStart.killFed:
                return $"{Namings.ShipConfig(global::ShipConfig.federation)} kills";
            case EQuestOnStart.killKrios:
                return $"{Namings.ShipConfig(global::ShipConfig.krios)} kills";
            case EQuestOnStart.killOcrons:
                return $"{Namings.ShipConfig(global::ShipConfig.ocrons)} kills";
            case EQuestOnStart.killDroids:
                return $"{Namings.ShipConfig(global::ShipConfig.droid)} kills";

            case EQuestOnStart.mainShipKills:
                return "Kills by commander";
                break;
            case EQuestOnStart.laserDamage:
                return $"Damage by {Weapon(WeaponType.laser)}";
                break;
            case EQuestOnStart.rocketDamage:
                return $"Damage by {Weapon(WeaponType.rocket)}";
            case EQuestOnStart.impulseDamage:
                return $"Damage by {Weapon(WeaponType.impulse)}";
            case EQuestOnStart.emiDamage:
                return $"Damage by {Weapon(WeaponType.eimRocket)}";
            case EQuestOnStart.cassetDamage:
                return $"Damage by {Weapon(WeaponType.casset)}";

            case EQuestOnStart.upgradeWeapons:
                return "Weapons levels upgrades";
                break;
            case EQuestOnStart.sellModuls:
                return "Sell modules";
            case EQuestOnStart.winRaiders:
                return $"Wins against {Namings.ShipConfig(global::ShipConfig.raiders)}";
            case EQuestOnStart.winMerc:
                return $"Wins against {Namings.ShipConfig(global::ShipConfig.mercenary)}";
            case EQuestOnStart.winFed:
                return $"Wins against {Namings.ShipConfig(global::ShipConfig.federation)}";
            case EQuestOnStart.winKrios:
                return $"Wins against {Namings.ShipConfig(global::ShipConfig.krios)}";
            case EQuestOnStart.winOcrons:
                return $"Wins against {Namings.ShipConfig(global::ShipConfig.ocrons)}";
            case EQuestOnStart.winDroids:
                return $"Wins against {Namings.ShipConfig(global::ShipConfig.droid)}";
        }
        return $"ERROR{type.ToString()}";
    }
    public static object ShipDamage(ShipDamageType damageType)
    {
        switch (damageType)
        {
            case ShipDamageType.engine:
                return "Engine";
                break;
            // case ShipDamageType.weapon:
            //     return "Weapon";
            case ShipDamageType.shiled:
                return "Shield";
            case ShipDamageType.fire:
                return "Fire";
            default:
                throw new ArgumentOutOfRangeException(nameof(damageType), damageType, null);
        }

        return "not impl";
    }

    public static string PriorityTarget = "Priority Target";
    public static string BaitPriorityTarget = "Bait Target";

    public static string Damage(ShipDamageType damageType, float time)
    {
        switch (damageType)
        {
            case ShipDamageType.engine:
                return $"Engine crashed for {time:0} sec";
                break;
            // case ShipDamageType.weapon:
            //     return $"Weapons turn off for {time:0} sec";
            case ShipDamageType.shiled:
                return $"Shield turn off for {time:0} sec";
            case ShipDamageType.fire:
                return $"Fire start!. {time:0} sec";
            default:
                return "null";
        }

        //        return "null";
    }

    public static string PriorityTargetDesc = "All your ship will try to attack selected ship.";
    public static string BaitPriorityTargetDesc = "All enemies ships will try to attack selected ship";
    public static string SpellModulChargers = "Charges require: {0} with delay {1} sec.";

    public static string ArtillerySpell =
        "Starts fire at selected zone. Total bullets {0}. Damage of each bullet:{2}/{1}.";

    public static string EnerguLockSpell = "Destroy engines at all enemies ships in radius {1} for {0} sec.";

    public static string LineSHotSpell =
        "Triple shot by selected direction. Base damage: {0}/{1}. And starts fire for {2} sec. Total fire damage:{3}";

    public static string MinesSpell =
        "Set {0} mines for {1} sec to selected location. Each mine damage {2}/{3}";

    public static string RandomDamageSpell =
        "Random damage to all inner moduls of ship. Do not work through shield for {0} sec.";

    public static string ShieldOffSpell =
        "Disable shields of ships in radius for {0} sec. And damages shield for {1}.";

    public static string TrowAroundSpell =
        "Create a shockwave witch throw around all ships in radius with power {0}. And shield damage {1}.";


    public static string KillUIPilotMini = "Kills:{0}";
    public static string KillUIPilot = "Kills:{0}/{1}";
    public static string Remain = "Remain";
    public static string EngineDest = "Engine destroyed";
    // public static string WeaponDest= "Weapon destroyed";
    public static string ShieldDest = "Shield destroyed";
    public static string FireDest = "Fire on board";
    public static string Retreat = "Retreat";
    public static string DoWantRetire = "Do you want run away?";
    public static string DoWantRetry = "Do you want run away? {0} of your ships will be badly damaged.";
    // public static string BeenBefore = "You have been there before.";
    public static string NoSafeGame = "No save game";
    public static string MaxLevel = "Max";
    public static string And = "And";
    public static string SupportModul = "Support module. Affect on weapons.";
    public static string ActionModul = "Action module";
    public static string CataclysmProcess = "{0} cells destroyed";
    public static string RemainCataclysm = "Days remain:{0}";
    public static string VeryEasy = "Very easy";
    public static string Easy = "Easy";
    public static string Normal = "Normal";
    public static string Hard = "Hard";
    public static string Imposilbe = "Impossible";
    public static string Radius = "Radius";
    public static string Sector = "Sector";
    public static string Reload = "Reload";
    public static string ShootPerTime = "Bullet count";
    public static string cantUpgrade = "Can't upgrade";

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

    public static string UpgParams = "Dou you want upgrade {0}?\n Cost {1} credits.";
    public static string Ok = "Ok";
    public static string CoreSector = "Target sector";
    public static string NotCoreSector = "Simple sector";
    public static string OpenCoordinates = "Coordinates of some fleets open";
    public static string StatisticDifficulty = "Difficulty:{0}%.";
    public static string StatisticConfig = "Army:{0}.";
    public static string StatisticMapSize = "Map size:{0}.";
    public static string StatisticFinalArmyPower = "Final power:{0}.";
    public static string StatisticDate = "{0}";
    public static string Fight = "Fight";
    public static string StatisticNoResult = "No results";
    public static string StatisticNoLastResult = "Wait for game...";
    public static string StatisticLastResult = "Last result:";
    public static string Sacrifice = "Sacrifice ship {0} the {1} of {2}";
    public static string TutorCloseForever = "Do not show this tip again";

    public static string TooltipWeapons(WeaponsPair weaponsPair)
    {
        return $"On start your ships will have one of {Weapon(weaponsPair.Part1)} or {Weapon(weaponsPair.Part2)} complect.";
    }

    public static string Tooltip(ShipConfig config)
    {
        switch (config)
        {
            case global::ShipConfig.raiders:
                return "Pros: Good speed, many slots for modules." +
                    "\nCons: Few weapon slots. No shield regeneration.";
            case global::ShipConfig.federation:
                return "Pros: Many weapon slots. Shield regeneration." +
                       "\nCons: Low speed.";
            case global::ShipConfig.mercenary:
                return "Pros: Balanced." +
                       "\nCons: Balanced.";
            case global::ShipConfig.ocrons:
                return "Pros: Perfect body health. Many slots for modules." +
                       "\nCons: No shied. No shield regeneration";
            case global::ShipConfig.krios:
                return "Pros: Good shield. Perfect shield regeneration" +
                       "\nCons: Low body health.";
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
                return "Scout can provide you information about enemies army.\nAnd sometimes can try steal something.";
            // case PlayerParameterType.diplomaty:
            //     return "Usable in map events.\nSometimes you can simple use it instead of fight.";
            case PlayerParameterType.repair:
                return "Between battles you will need to repair your fleet.\nThis one will help you.";
            case PlayerParameterType.chargesCount:
                return "Increase main ship charges count.";
            case PlayerParameterType.chargesSpeed:
                return "Increase main ship charges regeneration speed.";
        }

        return "null";
    }

    public static string EndGameDays = "Days:{0}";
    public static string BattleWinsStat = "Wins:{0}";
    public static string OpenWeaponsEndGame = "New weapons for start open {0} and {1}";
    public static string OpenConfigEndGame = "New fleet type available to play: {0}";
    public static string FrontShieldActivate = "Front shield activated";
    public static string Crit = "Crit {0}!";
    public static string WinEnd = "All fine now you are free and everything will be fine. Maybe...";
    public static string LoseEnd = "Your fleet destroyed.";

    public static string RepairDroneSpell =
        "Launch repair drone. When ship comes near drone starts healing it. Heal {1}% of hull points.";



    public static string BattleEvent(BattlefildEventType? eventType)
    {
        switch (eventType)
        {
            case BattlefildEventType.asteroids:
                return "Asteroid vortex";
            case BattlefildEventType.shieldsOff:
                return "EMP surge";
            case BattlefildEventType.engineOff:
                return "Engines off";
            case BattlefildEventType.turrets:
                break;
            case null:
                break;
        }

        return $"Error {eventType.ToString()}";
    }

    public static string Reflected = "Effect reflected";
    public static string CriticalDamage = $"Critical damages. If damages more than {Library.CRITICAL_DAMAGES_TO_DEATH} ship destroyed";
    public static string Event = "Event";

    public static string UnstableCore0 = "Some of your ships can get damage cause your core is unstable.\n\n Find repair services to stabilize your core.\n\n If core stability level become less than -10 your fleet will be destroyed.";

    public static string MapExit = "Do you want to exit to main menu?\nProgress will be saved and you can resume later.";
    public static string CantByLevel = "Can't use this item cause ship level is too low";
    public static string ReqireLevelFeild = "Require pilot level:{0}";

    public static string CanCauseNoLevel =
        "Can't upgrade weapon cause pilot don't have enough level.\nPilot level:{0}. Target Level{1}";

    public static string RoundWaveStrike = "Damage all ships near by [{2}/{3}].\n[Cost:{0}/{1}]";


    public static string DialogTag(string tag)
    {
        return Tag($"dialog_{tag}");
    }
    public static string Tag(string tag)
    {
        if (EngLocalization._locals.TryGetValue(tag, out var info))
        {
            return info;
        }
        return $"ERROR:{tag}";
    }

    public static string WaveStrikeSpell = "Strikes a bullets to each close enemy. Damage:{0}/{1}";
    public static string RechargeSheildSpell = "Recharge shield on {0}%";
    public static string ArmorField = "Armor:{0}/{1}";
}