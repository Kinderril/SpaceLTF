using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public static class Namings
{
    public static string ShipConfig(ShipConfig config)
    {
        switch (config)
        {
            case global::ShipConfig.raiders:
                return "Raiders";
            case global::ShipConfig.federation:
                return "Federation";
            case global::ShipConfig.mercenary:
                return "Mercenary";
            case global::ShipConfig.ocrons:
                return "Ocrons";
            case global::ShipConfig.krios:
                return "Krios";  
            case global::ShipConfig.droid:
                return "Droids";
        }
        return "none";
    }
    public static string ShipType(ShipType t)
    {
        switch (t)
        {
            case global::ShipType.Light:
                return "Light";
                break;
            case global::ShipType.Middle:
                return "Middle";
                break;
            case global::ShipType.Heavy:
                return "Heavy";
                break;
            case global::ShipType.Base:
                return "Base";
                break;
        }
        return "none";
    }

    public static string Weapon(WeaponType config)
    {
        switch (config)
        {
            case WeaponType.laser:
                return "Laser";
            case WeaponType.rocket:
                return "Rocket";
            case WeaponType.impulse:
                return "Impulse";
            case WeaponType.casset:
                return "Swarm";
            case WeaponType.eimRocket:
                return "EMI";     
            case WeaponType.beam:
                return "Beam";
        }
        return $"none.{config.ToString()} ___  {config}";
    }


    public static string SimpleModulName(SimpleModulType config)
    {
        string Name = "none";
        switch (config)
        {
            case SimpleModulType.shieldLocker:
                Name = "Shield Locker";
                break;
            case SimpleModulType.autoShieldRepair:
                Name = "Auto shiled";
                break;
            case SimpleModulType.shieldRegen:
                Name = "Shield regen";
                break;
            case SimpleModulType.autoRepair:
                Name = "Repair drone";
                break;
            case SimpleModulType.antiPhysical:
                Name = "Rocked catcher";
                break;
            case SimpleModulType.antiEnergy:
                Name = "Deflector";
                break;
            case SimpleModulType.closeStrike:
                Name = "Close strike";
                break;
            case SimpleModulType.engineLocker:
                Name = "Engine locker";
                break;
            case SimpleModulType.systemMines:
                Name = "EMI mine";
                break;
            case SimpleModulType.damageMines:
                Name = "Power mine";
                break;
            case SimpleModulType.blink:
                Name = "Teleportation";
                break;
            case SimpleModulType.laserUpgrade:
                Name = "Laser power";
                break;
            case SimpleModulType.rocketUpgrade:
                Name = "Rockets power";
                break;
            case SimpleModulType.bombUpgrade:
                Name = "Bomb power";
                break;
            case SimpleModulType.EMIUpgrade:
                Name = "EMI power";
                break;
            case SimpleModulType.impulseUpgrade:
                Name = "Impulse power";
                break;
            case SimpleModulType.ShipSpeed:
                Name = "Speed";
                break;
            case SimpleModulType.ShipTurnSpeed:
                Name = "Turn Speed";
                break;
            case SimpleModulType.WeaponSpeed:
                Name = "Bullet speed";
                break;   
            case SimpleModulType.WeaponSpray:
                Name = "Spray fire";
                break;   
            case SimpleModulType.WeaponDist:
                Name = "Longer dist";
                break;   
            case SimpleModulType.WeaponPush:
                Name = "Pusher";
                break;   
            case SimpleModulType.WeaponFire:
                Name = "Burner";
                break;   
            case SimpleModulType.WeaponEngine:
                Name = "Stopper";
                break;   
            case SimpleModulType.WeaponShield:
                Name = "Locker";
                break;   
            case SimpleModulType.WeaponWeapon:
                Name = "Blind";
                break;   
            case SimpleModulType.WeaponCrit:
                Name = "Critical";
                break;    
            case SimpleModulType.WeaponAOE:
                Name = "AOE";
                break;    
            case SimpleModulType.WeaponSector:
                Name = "Wider";
                break;    
            case SimpleModulType.WeaponLessDist:
                Name = "Close strike";
                break;    
            case SimpleModulType.WeaponChain:
                Name = "Chain strike";
                break;  
            case SimpleModulType.WeaponNoBulletDeath:
                Name = " Penetrating shot";
                break;
            case SimpleModulType.WeaponSelfDamage:
                Name = "Powerful recoil";
                break;
            case SimpleModulType.WeaponShieldPerHit:
                Name = "Shield stealer";
                break;   
            case SimpleModulType.WeaponPowerShot:
                Name = "Power shot";
                break;    
            case SimpleModulType.WeaponFireNear:
                Name = "Fire wave";
                break;  
            case SimpleModulType.ResistDamages:
                Name = "Protector";
                break; 
            case SimpleModulType.beamUpgrade:
                Name = "Beam upgrade";
                break;
            case SimpleModulType.ShipDecreaseSpeed:
                Name = "Heavy weapons";
                break;
            case SimpleModulType.ShieldDouble:
                Name = "Maximum shield";
                break;
            default:
                Debug.LogError($"NO NAME {config.ToString()}");
                break;

        }
        return Name;
    }

    public static string DescSimpleModul(SimpleModulType config)
    {
        string Name = "none";
        switch (config)
        {
            case SimpleModulType.shieldLocker:
                Name = "Lock shild of target enemy.";
                break;
            case SimpleModulType.autoShieldRepair:
                Name = "Repair shield when it's down.";
                break;
            case SimpleModulType.shieldRegen:
                Name = "Faster shield restoring.";
                break;
            case SimpleModulType.autoRepair:
                Name = "Add HP when ship have low hp.";
                break;
            case SimpleModulType.antiPhysical:
                Name = "Destroy physical bullets in close radius.";
                break;
            case SimpleModulType.antiEnergy:
                Name = "Destroy energy bullets in close radius";
                break;
            case SimpleModulType.closeStrike:
                Name = "When enemy close, launch special bullet to hit them.";
                break;
            case SimpleModulType.engineLocker:
                Name = "Destroy engine of target enemy.";
                break;
            case SimpleModulType.systemMines:
                Name = "Put system mines trying to hit enemies";
                break;
            case SimpleModulType.damageMines:
                Name = "Put damage mines trying to hit enemies";
                break;
            case SimpleModulType.blink:
                Name = "Ship can teleport to better locations.";
                break;
            case SimpleModulType.laserUpgrade:
                Name = "Increase power of all laser weapons for +1/+1 per level.";
                break;
            case SimpleModulType.rocketUpgrade:
                Name = "Increase power of all rocket weapons for +1/+1 per level.";
                break;
            case SimpleModulType.EMIUpgrade:
                Name = "Increase power of all EMI weapons for +1/+1 per level.";
                break;
            case SimpleModulType.impulseUpgrade:
                Name = "Increase power of all impulse weapons for +1/+1 per level.";
                break;
            case SimpleModulType.bombUpgrade:
                Name = "Increase power of all bomb weapons for +1/+1 per level.";
                break;
//            case SimpleModulType.ShipSpeed:
//                Name = String.Format("Increase ship max speed for {0}% per level.", ShipTurnModul.PER_LEVEL);
//                break;
//            case SimpleModulType.ShipTurnSpeed:
//                Name = String.Format("Increase ship turn speed for {0}% per level.", ShipSpeedModul.PER_LEVEL);
//                break;

            default:
                Debug.LogError($"NO DescSimpleModul {config.ToString()}");
                break;
        }
        return Name;
    }
    
    public static string Health = "Health";
    public static string Shield = "Shield";
    public static string Speed = "Speed";
    public static string TurnSpeed = "Turn";
    public static string Unknown = "Unknown";
//    public static string Destroyed = "Destroyed";

//    public static string SpellDesc(SpellType spellType,int level)
//    {
//        switch (spellType)
//        {
//            case SpellType.shildDamage:
//                return $"Disable shields of ships in radius for {ShieldOffSpell.PERIOD.ToString("0")} sec.";
//            case SpellType.engineLock:
//                return String.Format("Destroy engines for {0} sec. Work only if shield of target disabled.",EngineLockSpell.LOCK_PERIOD.ToString("0"));
//            case SpellType.lineShot:
//                return "Triple shot by selected direction";
//            case SpellType.throwAround:
//                return "Create a shockwave witch throw around all ships in radius";
////            case SpellType.allToBase:
////                return "Return all ship to base immidiatly";
////            case SpellType.invisibility:
////                return "Invisibility";
//            case SpellType.artilleryPeriod:
//                return "Start attack zone with artillery";
//            case SpellType.distShot:
//                return "Single bullet. Damage dependence on distance. And starts fire on target";
//            case SpellType.mineField:
//                return
//                    $"Settings {MineFieldSpell.MINES_COUNT} mines for {MineFieldSpell.MINES_PERIOD.ToString("0")} sec to selected location";
//            case SpellType.randomDamage:
//                return "Random damage to all inner moduls of ship. Do not work through shield";
////            case SpellType.spaceWall:
////                return String.Format("Creates a wall for {0} sec, whitch destroy all bullet coming to it",ShieldWallSpell.WALL_PERIOD);
//            default:
//                throw new ArgumentOutOfRangeException(nameof(spellType), spellType, null);
//        }
//
//        return "TODO DESC";
//    }

    public static string SpellName(SpellType spellType)
    {
        switch (spellType)
        {
            case SpellType.shildDamage:
                return "Shield Locker";
            case SpellType.engineLock:
                return "Engine locker";
            case SpellType.lineShot:
                return "Line shot";
            case SpellType.throwAround:
                return "Power explotion"; 
//            case SpellType.allToBase:
//                return "Phase move";
//            case SpellType.invisibility:
//                return "Invisibility";
            case SpellType.mineField:
                return "Mine Field";
            case SpellType.randomDamage:
                return "Inner Damage";
//            case SpellType.spaceWall:
//                return "Power wall";
            case SpellType.distShot:
                return "Power shot";
            case SpellType.artilleryPeriod:
                return "Artillery";
            default:
                throw new ArgumentOutOfRangeException(nameof(spellType), spellType, null);
        }

        return "TODO DESC";
    }

    public static string ParameterName(LibraryPilotUpgradeType value)
    {
        switch (value)
        {
            case LibraryPilotUpgradeType.health:
                return Health;
            case LibraryPilotUpgradeType.shield:
                return Shield;
            case LibraryPilotUpgradeType.speed:
                return Speed;
            case LibraryPilotUpgradeType.turnSpeed:
                return TurnSpeed;
        }
        return "todo";
    }

    public static string ParameterName(PlayerParameterType type)
    {
        switch (type)
        {
            case PlayerParameterType.scout:
                return "Scouts";
            case PlayerParameterType.diplomaty:
                return "Diplomaty";
            case PlayerParameterType.repair:
                return "Repair servies";
            case PlayerParameterType.chargesCount:
                return "Charges Count";
            case PlayerParameterType.chargesSpeed:
                return "Charges Speed";
            default:
                return "todo";
        }
        return "todo";
    }

    public static string Destroyed = "Destroyed";
    public static string Completed = "Completed";

    public static string StartNewGameFieldSize = "Sector size";
    public static string StartNewGameStartDeathTime = "Cataclysm time";
    public static string StartNewGameCoresCount = "Keys count";
    public static string StartNewGameBasePower = "Enemies power";
    public static string StartNewGameSectorsCount = "Sectors count";

    public static string StartInfo = "All is very simple. This galaxy crashing becouse of... nobody knows why." +
                                     "\nYou should run away as fast as you can, but you are not only one who want to do it." +
                                     "\nFind gates and activate it, before galaxy becomes dead. " +
                                     "\nTo activate gate you need to collect keys as many keys as you can, and bring them to gates." +
                                     "\nGood luck!";

    public static string DamageInfoUI = "Damage: {0}/{1}";
    public static string KillsInfoUI = "Kills: {0}";
    public static string ReputationChanges = "Reputation change {0} => {1}.";
    public static string Reputation = "Reputation: {0}";

    public static string ActionName(ActionType arg2ActionType)
    {
        switch (arg2ActionType)
        {
            case ActionType.attack:
                return "Attack";
                break;
            case ActionType.moveToBase:
                break;
            case ActionType.returnToBattle:
                return "Return";
                break;
            case ActionType.closeStrikeAction:
                break;
            case ActionType.evade:
                return "Evade";
                break;
            case ActionType.afterAttack:
                return "Evade";
                break;
            case ActionType.waitHeal:
                break;
            case ActionType.defence:
                break;
            case ActionType.mineField:
                break;
            case ActionType.attackSide:
                return "Attack";
                break;
            case ActionType.repairAction:
                return "Repair";
                break;
            case ActionType.waitEnemy:
                return "Wait";
                break;
            case ActionType.goToCurrentPointAction:
                break;
            case ActionType.goToHide:
                return "Hiding";
                break;
            case ActionType.waitEnemySec:
                return "Wait";
                break;
            case ActionType.waitEdnGame:
                return "Wait";
                break;
        }
        return "";
    }

    public static string TacticName(PilotTcatic pilotTactic)
    {
        switch (pilotTactic)
        {
            case PilotTcatic.defenceBase:
                return "Defence";
            case PilotTcatic.attack:
                return "Balance";
            case PilotTcatic.attackBase:
                return "Only base.";
        }
        return "";
    }

    public static string Fleet = "Fleet";
    public static string RechargeButton = "Recharge Shiled.\nCost[{0}/{1}]"; 
    public static string BuffButton = "Increase parameters of ship.\n[Cost:{0}/{1}]";
    public static string CellScouted = "Coordinates Scouted [{0},{1}]";

    public static object ShipDamage(ShipDamageType damageType)
    {
        switch (damageType)
        {
            case ShipDamageType.engine:
                return "Engine";
                break;
            case ShipDamageType.weapon:
                return "Weapon";
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
    public static string FakePriorityTarget = "Bait Target";

    public static string Damage(ShipDamageType damageType, float time)
    {
        switch (damageType)
        {
            case ShipDamageType.engine:
                return $"Engine crashed for {time:0} sec";
                break;
            case ShipDamageType.weapon:
                return $"Weapons turn off for {time:0} sec";
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
    public static string FakePriorityTargetDesc = "All enemies ships will try to attack selected ship";
}