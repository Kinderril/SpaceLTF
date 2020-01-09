using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
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
                return "Medium";
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
                Name = "Auto shield";
                break;
            case SimpleModulType.shieldRegen:
                Name = "Shield regeneration";
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
            case SimpleModulType.fireMines:
                Name = "Fire mine";
                break;
            case SimpleModulType.frontShield:
                Name = "Front shield";
                break; 
            case SimpleModulType.armor:
                Name = "Body armor";
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
            // case SimpleModulType.WeaponWeapon:
            //     Name = "Blind";
            //     break;   
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
            case SimpleModulType.WeaponShootPerTime:
                Name = "More bullets";
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
            case SimpleModulType.WeaponShieldIgnore:
                Name = "Ignore shield";
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
                Name = "Lock shield of target enemy.";
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
                Name = "Put system mines, crashing enemies engines";
                break;
            case SimpleModulType.damageMines:
                Name = "Put damage mines to hit enemies";
                break;
            case SimpleModulType.frontShield:
                Name = "If bullet bullet hit at front reflects it.";
                break; 
            case SimpleModulType.armor:
                Name = "Decrease damage on ships body";
                break; 
            case SimpleModulType.fireMines:
                Name = $"Put damage mines, witch can fire enemies for {MineFireModul.PERIOD_DAMAGE} sec.";
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
            case SpellType.repairDrones:
                return "Repair drones"; 
            case SpellType.rechargeShield:
                return "Recharge shield"; 
            case SpellType.roundWave:
                return "Round damage";
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
                return "Diplomacy";
            case PlayerParameterType.repair:
                return "Repair services";
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
    public static string StartNewGameStartDeathTime = "Stability level";
    public static string StartNewGameCoresCount = "Keys count";
    public static string StartNewGameBasePower = "Enemies power";
    public static string StartNewGameSectorsCount = "Sectors count";

    public static string StartInfo = "You are in the remote sector of the galaxy on the very edge of inhabited space. " +
                                     "\nOn the way, you collided a strange object that caused serious damage to the ship's systems. " +
                                     "\nAfter checking and repair, you found out that this mysterious object is firmly stuck in the " +
                                     "\nship's hull and unstable, and its extraction is impossible in this sector.\nYou have to head to the" +
                                     "intergalactic gates immidiatelly.According to the latest information, they are discharged." +
                                     "To activate them, you need energy cells.Find them and go to the gates " +
                                     "(by coincidence, they are far from you, on the other side of the galaxy)!";

    public static string DamageInfoUI = "Damage: {0}/{1}";
    public static string KillsInfoUI = "Kills: {0}";
    public static string ReputationChanges = "{2} reputation change {0} => {1}.";
    public static string Reputation = "{0} reputation: {1}";

    public static string ActionName(ActionType arg2ActionType)
    {
        switch (arg2ActionType)
        {
            case ActionType.readyToAttack:
                return "Preparing";
                break;   
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
                return "Evade enemy";
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
        "Starts fire at selected zone. Total bullets {0}. Damage of each bullet:{1}/{1}.";

    public static string DistShotSpell =
        "Single bullet. Base damage {0}. Additional damage dependence on distance.";

    public static string EnerguLockSpell = "Destroy engines for {0} sec.";

    public static string LineSHotSpell =
        "Triple shot by selected direction. Base damage: {0}/{1}. And starts fire for {2} sec.";

    public static string MinesSpell =
        "Set {0} mines for {1} sec to selected location. Each mine damage {2}/{3}";

    public static string RandomDamageSpell =
        "Random damage to all inner moduls of ship. Do not work through shield for {0} sec.";

    public static string ShieldOffSpell =
        "Disable shields of ships in radius for {0} sec. And damages shield for {1}.";

    public static string TrowAroundSpell =
        "Create a shockwave witch throw around all ships in radius with power {0}. And body damage {1}.";

    public static string WANT_UPGRADE_WEAPON = "Do you want to upgrade {0}?";
    public static string WeaponMaxLevel = "Weapon have max level";
    public static string SpellMaxLevel = "Spell have max level";
    public static string Processing = "Processing...";
    public static string Level = "Level";
    public static string Win = "Win!";
    public static string Lose = "Lose";
    public static string RunAwayComplete = "Run away complete";
    public static string Type = "Type";
    public static string Regen = "Regeneration";
    public static string SheildActive = "Shield is active";
    public static string SheildRestore = "Shield restoring";
    public static string SheildDisable = "Shield disabled";

    public static string BattleRestor = "Battery restoring";
    public static string BattleDisable = "Battery disable";
    public static string DamageBody = "Damage body";
    public static string DamageShield = "Damage shield";
    public static string ChargesCount = "Charges count {0}";
    public static string ChargesDelay = "Charges delay {0}";
    public static string Scouted = "Scouted";
    public static string Gate = "Gate";
    public static string Home = "Home";
    public static string Complete = "Complete {0}";
    public static string Target = "Target {0}";
    public static string Delay = "Delay";
    public static string KillUIPilot = "Kills:{0}/{1}";
    public static string Remain = "Remain";
    public static string EngineDest = "Engine destroyed";
    // public static string WeaponDest= "Weapon destroyed";
    public static string ShieldDest = "Shield destroyed";
    public static string FireDest = "Fire on board";
    public static string Retreat = "Retreat";
    public static string DoWantRetire = "Do you want run away?";
    public static string DoWantRetry = "Do you want run away? {0} of your ships will be badly damaged.";
    public static string BeenBefore = "You have been there before.";
    public static string NoSafeGame = "No save game";
    public static string MaxLevel = "Max";
    public static string And = "And";
    public static string SupportModul = "Support module";
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
    public static string Populated = "Populated by";
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
            case PlayerParameterType.diplomaty:
                return "Usable in map events.\nSometimes you can simple use it instead of fight.";
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
        "launch {0} drones. When ship comes near drone starts healing it. Heal {1}% of maximum heath points.";

    public static string MercGlobal= "Mercenary hideout";
    public static string NotEnoughtMoney = "Not enough money";
    public static string leave  = "Leave";
    public static string HireMerc = "Hire {1} the {0}. Level:{2}. Cost:{3} credits.";
    public static string Take = "Take";
    public static string BrokenItem = "Your item broken {0}";
    public static string StabilizaInfo = "Stability level:{0}";
    public static string MainElements = "Energy blocks";

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

    private static Dictionary<string,string> _locals = new Dictionary<string, string>()
    {
        {"Level", "Level"},
        {"ESideAttackStraight", "Attack as usual"},
        {"ESideAttackFlangs", "Attack from flags"},
        {"BaseDefenceYes", "Defend the base"},
        {"BaseDefenceNo", "No"},
        {"ECommanderPriority1MinShield","Choose to attack ships with minimum shield" },
        {"ECommanderPriority1MinHealth","Choose to attack ships with minimum body"  },
        {"ECommanderPriority1MaxShield","Choose to attack ships with max shield"  },
        {"ECommanderPriority1MaxHealth","Choose to attack ships with max body"  },
        {"ECommanderPriority1Any", "Any ships to attack"},
        {"ECommanderPriority1Light","Light ships will be a priority" },
        {"ECommanderPriority1Mid","Medium ships will be a priority" },
        {"ECommanderPriority1Heavy", "Heavy ships will be a priority"},
        {"QuestReward", "Choose one reward"},
        {"Reputation", "Reputation"},
        {"Inventory", "Inventory"},
    };

    public static string Tag(string tag)
    {
        if (_locals.TryGetValue(tag, out var info))
        {
            return info;
        }
        return $"ERROR:{tag}";
    }

    public static string WaveStrikeSpell = "Strikes a bullets to each close enemy. Damage:{0}/{1}";
    public static string RechargeSheildSpell = "Recharge shield on {0}%";
    public static string ArmorField = "Armor:{0}/{0}";
}