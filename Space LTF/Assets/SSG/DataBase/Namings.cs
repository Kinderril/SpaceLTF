using System;
using System.Collections.Generic;
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
            case SimpleModulType.WeaponMultiTarget:
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
                Name = "Decrease damage on ships hull";
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
            case SimpleModulType.WeaponSpeed:
                Name = "Increase speed of all bullets.";
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
            // case SpellType.randomDamage:
            //     return "Inner Damage";
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
        switch (type)
        {
            case PlayerParameterType.scout:
                return "Scouts";
            // case PlayerParameterType.diplomaty:
            //     return "Diplomacy";
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

    // public static string StartInfo = "You are in the remote sector of the galaxy on the very edge of inhabited space. " +
    //                                  "\nOn the way, you collided a strange object that caused serious damage to the ship's systems. " +
    //                                  "\nAfter checking and repair, you found out that this mysterious object is firmly stuck in the " +
    //                                  "\nship's hull and unstable, and its extraction is impossible in this sector.\nYou have to head to the" +
    //                                  "intergalactic gates immediately.According to the latest information, they are discharged." +
    //                                  "To activate them, you need energy cells.Find them and go to the gates " +
    //                                  "(by coincidence, they are far from you, on the other side of the galaxy)!";



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
                return Namings.Tag("Attack");
                break;
            case ActionType.attackHalfLoop:
                return Namings.Tag("AttackHalfLoop");
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
                return Namings.Tag("Evade");
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

    public static string WANT_UPGRADE_WEAPON = "Do you want to upgrade {0}?";
    public static string WeaponMaxLevel = "Weapon have max level";
    public static string Processing = "Processing...";
    public static string Win = "Win!";
    public static string Lose = "Lose";
    public static string RunAwayComplete = "Run away complete";
    public static string Type = "Type";
    public static string Regen = "Regeneration";
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
        "launch {0} drones. When ship comes near drone starts healing it. Heal {1}% of maximum heath points.";



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

    private static Dictionary<string, string> _locals = new Dictionary<string, string>()
    {
        {"StartInfo", "You are in the remote sector of the galaxy on the very edge of inhabited space.\n" +
                  "As a result of special operations, you had to steal the command ship and the energy\n" +
                  " element on it. Now you have to find the rest of the elements to open the galaxy gates," +
                  "\n and deliver the data to your command"},
        {"Yes", "Yes"},
        {"No", "No"},
        {"Ok", "Ok"},

        {"Shop", "Shop"},
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
        {"ReputationElement", "Enemies:{0}"},
        {"Trickturn", "Quick turn"},
        {"Trickloop", "Loop"},
        {"Tricktwist", "Twist"},
        {"Settings", "Settings"},
        {"Private", "Private"},
        {"Lieutenant", "Lieutenant"},
        {"Achievements", "Achievements"},
        {"Captain", "Captain"},
        {"Major", "Major"},
        {"OpenNew", "Unlock"},
        {"FinalSector", "Galaxy gates"},
        {"Attack", "Attack"},
        {"AttackHalfLoop", "Trick attack"},
        {"Evade", "Evade"},
        {"Level", "Level"},
        {"NotEnoughtBattries", "Battries are not ready!"},
        {"reloadBoost", "Trick reload time:{0}"},
        {"PointsToOpen", "Points to unlock start options:"},
        {"SpellMaxLevel", "Max level reached: {0}"},


        {"descWinner", "--"},
        {"descCaptain", "--"},
        {"descMayor", "--"},
        {"descBig guns", "--"},
        {"descImprover", "--"},
        {"descTeam five", "--"},
        {"descFriend", "--"},
        {"descBoss", "--"},
        {"descMaster", "--"},
        {"descFighter", "--"},
        {"descDestroyer", "--"},
        {"descAnnihilator", "--"},
        {"descShooter", "--"},
        {"descSniper", "--"},
        {"descGunner", "--"},
        {"descBanker", "--"},
        {"descCroesus", "--"},
        {"descUnusual", "--"},
        {"descNo crash", "--"},
        {"descFull pack", "--"},
        {"descFull ammo", "--"},

        {"nameWinner", "Winner"},
        {"nameCaptain", "Captain"},
        {"nameMayor", "Mayor"},
        {"nameBig guns", "Big guns"},
        {"nameImprover", "Improver"},
        {"nameTeam five", "Team five"},
        {"nameFriend", "Friend"},
        {"nameBoss", "Boss"},
        {"nameMaster", "Master"},
        {"nameFighter", "Fighter"},
        {"nameDestroyer", "Destroyer"},
        {"nameAnnihilator", "Annihilator"},
        {"nameShooter", "Shooter"},
        {"nameSniper", "Sniper"},
        {"nameGunner", "Gunner"},
        {"nameBanker", "Banker"},
        {"nameCroesus", "Croesus"},
        {"nameUnusual", "Unusual"},
        {"nameNo crash", "No crash"},
        {"nameFull pack", "Full pack"},
        {"nameFull ammo", "Full ammo"},

        {"MercGlobal", "Mercenary hideout"},
        {"NotEnoughtMoney", "Not enough money"},
        {"leave", "Leave"},
        {"HireMerc", "Hire {1} the {0}. Level:{2}. Cost:{3} credits."},
        {"Take", "Take"},
        {"BrokenItem", "Your item broken {0}"},
        {"StabilizaInfo", "Stability level:{0}"},
        {"MainElements", "Energy blocks"},

        {"Health", "Health"},
        {"Shield", "Shield"},
        {"Speed", "Speed"},
        {"TurnSpeed", "Turn"},
        {"Unknown", "Unknown"},

        {"Laser", "Laser"},
        {"Rocket", "Rocket"},
        {"Impulse", "Impulse"},
        {"Swarm", "Swarm"},
        {"EMI", "EMI"},
        {"Beam", "Beam"},
        {"MovingArmy", "Special forces of {1}\nStatus:{2}. Power:{3}"},
        {"MovingArmyBorn", "Special forces of {0} appear. Coordinates:{1}"},
        {"AdditionalPower", "Enemies upgrades"},

        {"Comparable", "Comparable"},
        {"Risky", "Risky"},
        {"Easily", "Easily"},
        {"confirmDismiss", "You really want to dismiss ship {0}?"},
        {"MovingArmyName", "SpecOps {0}"},

        {"Demo", "This is demo version. We are not ready to show more. Maximum visited sectors:{0}. Maximum turns:{1}"},

        {"RecievePoints", "You will get points to unlock after winning. Amount depends on difficulty."},
        {"AA_FXAA", "Anti aliasing"},
        {"Sound", "Sound"},
        {"battlefield", "Battlefield"},
        {"Trader","Trader"},
        {"BrokenNavigation", "Broken navigation"},
        {"modifBaseStart", "This is modification base. You can try to modificate something."},
        {"modifCellName", "Modification station"},

        {"scout_shipType", "No ship type info"},
        {"scout_weapons", "No weapons info"},
        {"scout_level", "No level info"},
        {"scout_moduls", "No modules info"},
        {"scout_spells", "No big guns info"},
        {"targteFar", "Target is too far"},

        {"ChainModulDesc", "Shoot to all targets in radius. Decrease damage on {0}%"},
        {"FireNearModulDesc", "Can fire nearby enemies for {0} sec when shoot."},
        {"SprayModulDesc", "Shoot with {0} bullets instead on one. Increase reload time by {1}%."},
        {"WeaponSpeedModulDesc", "Increase bullet speed by {0}% per level"},
        {"DescWeaponDistModul", "Increase aim radius by {0}% per level"},

        {"DistShotSpell", "Single bullet. Base damage {0}. Additional damage dependence on distance."},
        {"DistShotSpellSpecial", "Single bullet. Base damage {0}. Additional damage dependence on distance. If target have no shield destroy engine for {1} sec."},

        {"rep_adv_friend", "You are friends. Maybe they can help you."},
        {"rep_adv_neutral", "Fleets will fight, but you can trade with them."},
        {"rep_adv_negative", "You still can trade with them, but prices will be bad"},
        {"rep_adv_enemy", "No trade. Only fight! Special force will try to find and kill you."},
        {"rep_friend", "Friends"},
        {"rep_neutral", "Neutral"},
        {"rep_negative", "Negative"},
        {"rep_enemy", "Enemy"},
        {"DemoStart", "This is not final version on game, something can change later."},
        {"DemoStart2", "You will have only 25 turns. And some mechanics will be blocked."},

        {"dialog_MovingArmyStart", "You have been occupied by special force. You have no chanse to run."},
        {"dialog_MovingArmyFight", "Fight"},
        {"dialog_MovingArmyWin", "Special forces destroyed."},
        {"dialog_MovingArmyGerReward", "Get all valuable items."},
        {"dialog_shopEnemy", "You see shop. Nobody wants to trade with you." },
        {"dialog_shopTrade", "You see shop. Maybe you want buy something?" },
        {"dialog_repairEnemy", "This is repair station. They won't help you." },
        {"dialog_repairStart", "This is repair station. We can repair our fleet here." },
        {"dialog_fixCrit", "Fix all critical damages" },
        {"dialog_repairCritFixed", "Critical damages fixed" },
        {"dialog_repairAll", "Yes repair all.{0} credits" },
        {"dialog_repairNotEnought", "I don't have enough credits" },

        //DILOGS ASTEROIDS
        {"dialog_asteroid_nothingComplete", "Nothing happens. You successfully complete way" },
        {"dialog_asteroid_nothingCompleteRepair","Nothing happens, but some ships need to be repaired. You successfully complete way" },
        {"dialog_asteroid_start", "Big asteroid field. You need somehow get to other side." },
        {"dialog_asteroid_fireAll","Fire from all weapons" },
        {"dialog_asteroid_slow","Slow go through field." },
        {"dialog_asteroid_brokenShip","You see {0} broken ship." },
        {"dialog_asteroid_brokenShipRepair", "Try to repair it [Repair:{0}]."},
        {"dialog_asteroid_brokenShipdecompile","Try to decompile it for money." },
        {"dialog_repairResultFull", "This ship can't be fully repaired but now you can use some parts. {0}"},
        {"dialog_repairResultCant", "This ship can't be fully repaired but you have no place for items."},
        {"dialog_asteroid_weaponsFire","Explosive weapons easily destroy enough asteroid. Now you have a way." },
        {"dialog_asteroid_weaponsFail", "Your weapons are have not much power to create a way throug. And some of your ships get damage"},
        {"dialog_asteroid_haveWay", "Finally you have a way! But some of your ships get damage"},
              
        //DILOGS BATTLEFIELD
        {"dialog_battlefield_start","You see some {0} ships stands against each other." },
        {"dialog_battlefield_provocate", "Hide and provocate them"},
        {"dialog_battlefield_reconcile","Try to reconcile all sides" },
        {"dialog_battlefield_thanks","Thanks!" },
        {"dialog_battlefield_diplomacyWin", "Your diplomacy skills are perfect. They will not fight and send you a gift for helping. Reputation add {Library.REPUTATION_RELEASE_PEACEFULL_ADD}."},
        {"dialog_battlefield_run","Run!" },
        {"dialog_battlefield_shoot","Shoot near!" },
        {"dialog_battlefield_diplomacyFail","Your diplomacy skills is awful. But now they want to kill you instead of each other." },
        {"dialog_battlefield_shit", "Shit."},
        {"dialog_battlefield_theyAttack","This is not your day. They attacking you!" },
        {"dialog_battlefield_ufff", "Ufff... Great."},
        {"dialog_battlefield_stopAttack","They stop trying attack anybody and run away. Maybe this is not bad. At least you save some lives" },
        {"dialog_battlefield_youRun", "You run away"},
        {"dialog_battlefield_artillery","Catch moment and fire with artillery" },
        {"dialog_battlefield_wait","Wait" },
        {"dialog_battlefield_theyBattle","They starts battle." },
        {"dialog_battlefield_killEachOther", "Battle ends. They kill each other. And you find some credits {0}."},
        {"dialog_battlefield_isee", "I see"},
        {"dialog_battlefield_battleHaveWinner", "Battle ends. Win side take trophies and go away. You can do nothing"},
        {"dialog_battlefield_good","Good" },
        {"dialog_battlefield_goodSHot", "Good shoot. You destroy all of them. After battle you find some items"},
        {"dialog_battlefield_fight","Fight" },
        {"dialog_battlefield_killYou","Now they want to destroy you." },

        //DIALOG_BROKEN_NAVIGATION
        {"dialog_navigation_start","You have a distress signal" },
        {"dialog_navigation_closer","Come closer" },
        {"dialog_navigation_trap", "It's a trap!"},
        {"dialog_navigation_fight", "Fight"},
        {"dialog_navigation_askHelp","You see a {0} ship with broken navigation system. He asking for help" },
        {"dialog_navigation_shelter","Send scout to deliver him to closest shelter. [Scouts: {0}]" },
        {"dialog_navigation_repair","Try repair navigation system. [Repair {0}]" },
        {"dialog_navigation_gift", "Your gift: {0}"},
        {"dialog_navigation_noFree", "Not free space for gift"},
        {"dialog_navigation_scouted", "{0} cells on global map scouted."},
        {"dialog_navigation_shelterOk","You successfully find way to shelter. {0}." },
        {"dialog_navigation_repairOk","You successfully repair ship. Reputation add." },
        {"dialog_navigation_tryHire", "Try hire"},
        {"dialog_navigation_takeMoney","Take money" },
        {"dialog_navigation_hired","Ship hired {0}." },
                            
        //DIALOG_CHANGE_ITEM
        {"dialog_changeitem_start","This ship wants to trade with you. {0}" },
        {"dialog_changeitem_tradeData","\nThey want to change your {0} to {1}" },
        {"dialog_changeitem_ok","Ok. Lets change." },
        {"dialog_changeitem_no","No, thanks." },
        {"dialog_changeitem_nothing","You have nothing to trade" },

        {"dialog_armySectorEvent","Sector event:{0}\n" },
        {"dialog_armyFrendly", "This fleet looks friendly.\n {0}"},
        {"dialog_armyBuyOut","Try to buy out. [Cost :{0}]" },
        {"dialog_armyStronger", "You see enemies. They look stronger than you. Shall we fight? \n {0}"},
        {"dialog_armyShallFight","You see enemies. Shall we fight? \n {0}" },
        {"dialog_armyAskHelp",  "Ask for help. [Reputation:{0}]"},
        {"dialog_armyRun","Runaway" },
        {"dialog_armyRunComplete","Running away complete." },
        {"dialog_armyRunFail", "Fail! Now you must fight."},
        {"dialog_armyBuyoutComplete","Buyout complete. You lose {0} credits." },

        {"dialog_coreAttack","Attack." },
        {"dialog_coreTargetProtect","Target is under protection." },
        {"dialog_coreBuy","Buy for {0} credits." },
        {"dialog_coreUseDiplomacy","Use diplomacy." },
        {"dialog_coreSendScouts","Send scouts to steal." },
        {"dialog_coreFleetHave","Some other fleet already have your target. \n {scoutsField}" },
        {"dialog_coreFleetHaveFriend","Friendly fleet already find your target. You can try to use diplomacy. Or simply buy it.\n {scoutsField}" },
        {"dialog_coreWasPurchase","Element was purchased. {0}/{1}" },
        {"dialog_coreElementYour","Element is yours. {0}/{1}" },
        {"dialog_coreFight","Very bad. Fight." },
        {"dialog_coreFailSteal","Fail. While you were trying to steal an item, reinforcements came to them, and now you can't runaway." },
        {"dialog_corePartYours", "This part is yours now."},
        {"dialog_coreDiplomacyFail", "They don't want to give you this part"},

        {"dialog_hirePay", "Pay"},
        {"dialog_hireAttack", "You won't pay. Instead of it, you will just want to kill them all."},
        {"dialog_hireEnter","Space base inside big asteroid. Have impressive protection. \nEnter not free. Pay {0} to enter" },
        {"dialog_hireSomebody","You can try to hire somebody here." },
        {"dialog_hireMax","You have maximum size of fleet." },
        {"dialog_hireComplete","Hire complete" },

        {"Prisoner","Prisoner"},
        {"dialog_prisonerStart","Criminal try to escapes from the {0} police. But your fleet can catch him"},
        {"dialog_prisonerBuy","Buy stolen item. [Cost:{0}]"},
        {"dialog_prisonerCatch","Catch and return to police."},
        {"dialog_prisonerHire","Hire him"},
        {"dialog_prisonerHide","Hide him from police. [Reputation:{0}]"},
        {"dialog_prisonerHired","Criminal hired. Reputation removed {0}"},
        {"dialog_prisonerYourItem","Your item: {0}"},
        {"dialog_prisonerNoSpace","Not free space for item"},
        {"dialog_prisonerComplete","Complete. {0}"},
        {"dialog_prisonerHideOk","You successfully hide criminals ship. Credits add {0}"},
        {"dialog_prisonerFailFight","Fail! Now you will fight with police."},
        {"dialog_prisonerCatchOk","You successfully catch him. Reputation added"},
        {"dialog_prisonerBuyFail","You don't have enough money. Criminal just fly away"},

        {"ScienceLaboratory","Science laboratory"},
        {"dialog_scLabStart","You are close to {0} science laboratory. But this one is under siege."},
        {"dialog_scLabAttackNow","Attack immediately"},
        {"dialog_scLabContact","Contact with fleet commander"},
        {"dialog_scLabSedScouts","Send scouts to laboratory"},
        {"dialog_scLabFrighten","Try to frighten them."},
        {"dialog_scLabBetterContact","No, better I contact with them."},
        {"dialog_scLabNotRegular","This isn't regular army. We can try to frighten them."},
        {"dialog_scLabGoStation","Go to station."},
        {"dialog_scLabTheyRun","They running away!."},
        {"dialog_scLabFight","Fight"},
        {"dialog_scLabTotalFail","Fail! They just launch rockets to laboratory and now wants to kill you."},
        {"dialog_scLabGiveCredits","Maybe if just give you some credits and you will leave [Credits:{0}]"},
        {"dialog_scLabSimpleThiefs","They look like simple thiefs."},
        {"dialog_scLabTheLeave","They leaving."},
        {"dialog_scLabBadJoke","Bad joke! [Not enough credits]"},
        {"dialog_scLabAllKilled","All scientists were killed."},
        {"dialog_scLabImproveMain","Improve main ship."},
        {"dialog_scLabImproveBattle","Improve battle ships."},
        {"dialog_scLabScientsChoose","Scientists are alive. And they can improve your army"},
        {"dialog_scLabShipUpgraded","{0} upgrade at ship {1} Upgraded."},
        {"dialog_scLabAllMax","All your ships have max level."},
        {"dialog_scParamUpgraded","{0} Upgraded"},
        {"dialog_scLabNotingUpgrade","Nothing to improve."},

        {"dialog_secretStart","A lot of space garbage and other useless thing here. Do you want to investigate?"},
        {"dialog_secretSendScouts","Send scouts"},
        {"dialog_secretHideWait","Hide and wait"},
        {"dialog_secretSendFake","Send fake signal"},
        {"dialog_secretAttack","Attack!"},
        {"dialog_secretConnectWith","Connect with commander."},
        {"dialog_secretScoutResult","Scouts find a huge fleet, waiting for something. [Scouts:{0}]"},
        {"dialog_secretTryPrevent","Try to prevent conflict"},
        {"dialog_secretRun","Run"},
        {"dialog_secretSeeHugeArmy","Suddenly you see a huge army. And they are ready! [Scouts:{0}]"},
        {"dialog_secretHaveCredits","And now you have [credits{0} and {1}]"},
        {"dialog_secretYouRun","You run away. {0} [Scouts:{1}]"},
        {"dialog_secretFight","Fight"},
        {"dialog_secretRunFail","Running away failed! [Scouts:{0}]"},
        {"dialog_secretApplyDeal","Apply deal"},
        {"dialog_secretLeave","Leave"},
        {"dialog_secretTheyMessage","They send you a message with trade options."},
        {"dialog_secretTheyGoKill","Now they going to kill. Diplomacy is not your good side."},
        {"dialog_secretPartGo","Part of fleet going to check signal."},
        {"dialog_secretDoProvocation","Do provocation."},
        {"dialog_secretWait","Wait."},
        {"dialog_secretSecondCome","{0}s army coming."},
        {"dialog_secretSearchPlace","Search place."},
        {"dialog_secretTheyEnd","They complete trade, and ready to jump."},
        {"dialog_secretTake","Take. [Credits:{0}]"},
        {"dialog_secretForgotCredits","Looks like they forgot some credits."},
        {"dialog_secretTakeAll","Take all and run."},
        {"dialog_secretAppearSecond","While trading, appears another fleet."},
        {"dialog_secretFindGoods","After massive battle you find some goods. [Credits:{0}]"},
        {"dialog_secretProvoceFail","Your provocation failed. Now fight."},

        {"dialog_finalStartReady","This is your main goal. You have {0}/{1} parts to open gates.\n You are ready to go in."},
        {"dialog_finalStartNotReady","This is your main goal. You have {0}/{1} parts to open gates.\nNow you should acquire all others.\n Only one way to do it send one of your ships to block energy on gates. \n But you don't have enought ship \n Your are dead."},
        {"dialog_finalProcess","This is your main goal. You have {0}/{1} parts to open gates.\n Now you should acquire all others.\n Only one way to do it send one of your ships to block energy on gates. "},
        {"dialog_finalStartFight","But somebody don't want let you go and attacks you"},
        {"dialog_finalEnd","Now way is free and you can go whatever you want."},


    };

    public static string DialogTag(string tag)
    {
        return Tag($"dialog_{tag}");
    }
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
    public static string ArmorField = "Armor:{0}/{1}";
}