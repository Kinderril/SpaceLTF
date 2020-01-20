using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ArmyCreationMode
{
    equalize,

    random
//    allToOne,
}

public enum LibraryShipUpgradeType
{
    addWeapon,
    addModul,
    upgradeWeapon,
    upgradeModul,

//    upgradeSimple,
    levelUpPilot
}

public enum LibraryPilotUpgradeType
{
    health,
    shield,
    speed,
    turnSpeed
}

public static class Library
{
    public const float SECTOR_COEF_POWER = 0.3f;

    public const int MAX_WEAPON_LVL = 4;
    public const int MAX_SPELL_LVL = 4;
    public const int MAX_MOUDL_LEVEL = 3;

    public const float MIN_WORKING_SHIP = 6;
    public const float MAX_ARMY_POWER_MAP = 70;

    public const float REPAIR_DISCOUTNT = 0.5f;

    private const float LASER_SPEED = 9.7f;
    private const float LASER_DELAY = 4f;
    private const float LASER_ANG = 20f;

    private const float ROCKET_ANG = 44f;
    private const float ROCKET_DELAY = 10f;
    private const float ROCKET_SPEED = 6.8f;

    private const float IMPULSE_SPEED = 8.8f;
    private const float IMPULSE_DELAY = 5.5f;

    public const float MINE_SPEED = 5.2f;
    private const float MINE_DELAY = 7f;
    private const float MINE_ANG = 19f;

    private const float BEAM_ANG = 180f;
    private const float BEAM_DELAY = 3f;
    private const float BEAM_SPEED = 1f;

    private const float SPREAD_ANG = 20f;
    private const float SPREAD_DELAY = 6f;
    private const float SPREAD_SPEED = 4.9f;

    private const float ARTILLERY_ANG = 10f;

    private const float EMI_ANG = 100f;
    private const float EMI_DELAY = 6f;
    private const float EMI_SPEED = 12f;

    public const float BASE_WEAPON_VALUE = 1.2f;
    public const float BASE_SPELL_VALUE = 0.5f;
    public const float BASE_SIMPLE_MODUL_VALUE = 0.8f;
    public const float BASE_SIMPLE_MODUL_VALUE_UPGRADE = 0.6f;
    public const float MIN_POINTS_TO_CREATE_ARMY_WITH_BASESHIP = 20f;
    public const float BASE_SHIP_VALUE = 2f;
    public const float WEAPON_LEVEL_COEF = 0.8f;
    public const float PILOT_LEVEL_COEF = 0.35f;
    public const float PILOT_RANK_COEF = 0f; //0.7f;
    public const int MAX_PILOT_PARAMETER_LEVEL = 80;

    public const int COINS_TO_POWER_WEAPON_SHIP_SHIELD = 1;
    public const int COINS_TO_POWER_WEAPON_SHIP_SHIELD_DELAY = 40;
    public const int COINS_TO_CHARGE_SHIP_SHIELD = 1;
    public const int COINS_TO_WAVE_SHIP = 1;
    public const int COINS_TO_CHARGE_SHIP_SHIELD_DELAY = 20;
    public const int COINS_TO_WAVE_SHIP_DELAY = 30;
    public const float CHARGE_SHIP_SHIELD_HEAL_PERCENT = 0.35f;

    public const int PriorityTargetCostTime = 120;
    public const int PriorityTargetCostCount = 1;

    public const int BaitPriorityTargetCostTime = 120;
    public const int BaitPriorityTargetCostCount = 2;
    public const int CRITICAL_DAMAGES_TO_DEATH = 2;
    public const int SHIELD_COEF_EXTRA_CHARGE = 10;
    public const float BATTLE_REWARD_WIN_MONEY_COEF = 1.3f;
    public const float MONEY_PILOT_LEVEL_UP_COST_BASE = 5f;
    public const float MONEY_PILOT_LEVEL_UP_COST_COEF = 1.25f;
    public static int START_PLAYER_FREE_PARAMETERS = 2;
    private static int[] _lvlUps = new int[MAX_PILOT_PARAMETER_LEVEL];
    public static int RANK_ERIOD = 12;
    public static int BASE_CHARGES_COUNT = 5;
    public static int WEAPON_REQUIRE_LEVEL_COEF = 2;
    public static int MODUL_REQUIRE_LEVEL_COEF = 2;

    public static Dictionary<PilotRank, List<EPilotTricks>> PosibleTricks;

    public static void Init()
    {
        _lvlUps = new int[MAX_PILOT_PARAMETER_LEVEL];
        for (var i = 0; i < MAX_PILOT_PARAMETER_LEVEL; i++)
            _lvlUps[i] = (int) (i * MONEY_PILOT_LEVEL_UP_COST_COEF + MONEY_PILOT_LEVEL_UP_COST_BASE);
        LibraryModuls.Init();

        PosibleTricks = new Dictionary<PilotRank, List<EPilotTricks>>
        {
            {PilotRank.Private, new List<EPilotTricks> {EPilotTricks.turn}},
            {PilotRank.Lieutenant, new List<EPilotTricks> {EPilotTricks.turn, EPilotTricks.twist}},
            {PilotRank.Captain, new List<EPilotTricks> {EPilotTricks.turn, EPilotTricks.twist, EPilotTricks.loop}},
            {PilotRank.Major, new List<EPilotTricks> {EPilotTricks.turn, EPilotTricks.twist, EPilotTricks.loop}}
        };
    }

    public static int PilotLvlUpCost(int curLvl)
    {
        return _lvlUps[curLvl];
    }

    public static WeaponInv CreateWeapon(WeaponType weapon)
    {
        WeaponInventoryParameters parametes;
        switch (weapon)
        {
            case WeaponType.laser:
                parametes = new WeaponInventoryParameters(4, 4, LASER_ANG, LASER_DELAY, 0.5f, 1, LASER_SPEED, 8);
                return new LaserInventory(parametes, 1);
            case WeaponType.rocket:
                parametes = new WeaponInventoryParameters(2, 6, ROCKET_ANG, ROCKET_DELAY, 0.5f, 1, ROCKET_SPEED, 11,
                    36f);
                return new RocketInventory(parametes, 1);
            case WeaponType.impulse:
                parametes = new WeaponInventoryParameters(3, 1, LASER_ANG, IMPULSE_DELAY, 0.5f, 2, IMPULSE_SPEED, 6);
                return new ImpulseInventory(parametes, 1);
            case WeaponType.casset:
                parametes = new WeaponInventoryParameters(2, 4, MINE_ANG, MINE_DELAY, 0.5f, 1, MINE_SPEED, 9, 70f);
                return new BombInventoryWeapon(parametes, 1);
            case WeaponType.eimRocket:
                parametes = new WeaponInventoryParameters(2, 2, EMI_ANG, EMI_DELAY, 0.4f, 2, EMI_SPEED, 7);
                return new EMIRocketInventory(parametes, 1);
            case WeaponType.beam:
                parametes = new WeaponInventoryParameters(1, 4, BEAM_ANG, BEAM_DELAY, 0.4f, 1, BEAM_SPEED, 2.5f);
                return new BeamWeaponInventory(parametes, 1);
            //            case WeaponType.beam://NO USABLE
            //                return new WeaponInv(2,7, MINE_ANG, BEAM_DELAY, 0.5f, 1, BEAM_SPEED,4, weapon, 1);
            //            case WeaponType.artillery:
            //                parametes = new WeaponInventoryParameters(1, 4, MINE_ANG, MINE_DELAY, 0.5f, 1, MINE_SPEED, 9);
            //                return new WeaponInv(0,1, ARTILLERY_ANG, MINE_DELAY, 0.5f, 1, MINE_SPEED, 21, weapon, 1);
            //            case WeaponType.spread:
            //                parametes = new WeaponInventoryParameters(1, 4, MINE_ANG, MINE_DELAY, 0.5f, 1, MINE_SPEED, 9);
            //                return new WeaponInv(1,1, SPREAD_ANG, SPREAD_DELAY, 0.5f, 1, SPREAD_SPEED, 9, weapon, 1);
        }

        return null;
    }

    public static WeaponInv CreateWeapon(bool low)
    {
        var level = new WDictionary<int>(new Dictionary<int, float>
        {
            {1, low ? 15 : 10},
            {2, low ? 4 : 3},
            {3, low ? 0.5f : 1}
        });
        return CreateWeapon(level.Random());
    }

    public static WeaponInv CreateWeapon(int level)
    {
        var types = new WDictionary<WeaponType>(new Dictionary<WeaponType, float>
        {
            {WeaponType.laser, 9},
            {WeaponType.rocket, 9},
            {WeaponType.eimRocket, 7},
            {WeaponType.casset, 7},
            {WeaponType.impulse, 7},
            {WeaponType.beam, 7}
        });

        var w = CreateWeapon(types.Random());
        w.Level = level;
        return w;
    }

    public static ShipInventory CreateShip(ShipType shipType, ShipConfig config, Player player, PilotParameters pilot)
    {
        var reloadTime = CalcTrickReload(pilot.Stats.CurRank, shipType);
        switch (config)
        {
            case ShipConfig.droid:
                switch (shipType)
                {
                    case ShipType.Light:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 15, 5, 3.2f, 89, 1, 0, 0, 1, 0.0f,1f,reloadTime), player, pilot);
                    case ShipType.Middle:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 18, 7, 2.8f, 71, 1, 0, 0, 1, 0.0f,1f,reloadTime), player, pilot);
                    case ShipType.Heavy:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 22, 8, 2.1f, 69, 2, 0, 0, 1, 0.0f,1f,reloadTime), player, pilot);
                    case ShipType.Base:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 33, 10, 0.51f, 40, 0, 0, 6, 1, 0f,1f,reloadTime), player, pilot);
                }

                break;
            case ShipConfig.raiders:
                switch (shipType)
                {
                    case ShipType.Light:
                        var ship = new ShipInventory(
                            new StartShipParams(shipType, config, 21, 17, 4.3f, 78, 1, 3, 0, 1, 0.0f,1f,reloadTime), player, pilot);
                        ship.ShiledArmor = 1;
                        return ship;
                    case ShipType.Middle:
                        var ship1 = new ShipInventory(
                            new StartShipParams(shipType, config, 28, 19, 3.8f, 70, 1, 4, 0, 1, 0.05f,1f,reloadTime), player, pilot);
                        ship1.ShiledArmor = 1;
                        return ship1;
                    case ShipType.Heavy:
                        var ship2 = new ShipInventory(
                            new StartShipParams(shipType, config, 32, 21, 3.5f, 68, 2, 4, 0, 1, 0.1f,1f,reloadTime), player, pilot);
                        ship2.ShiledArmor = 1;
                        return ship2;
                    case ShipType.Base:
                        var ship3 = new ShipInventory(
                            new StartShipParams(shipType, config, 46, 28, 1.5f, 40, 0, 0, 6, 1, 0f,1f,reloadTime), player, pilot);
                        ship3.ShiledArmor = 1;
                        return ship3;
                }

                break;
            case ShipConfig.mercenary:
                switch (shipType)
                {
                    case ShipType.Light:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 22, 11, 3.8f, 72, 2, 2, 0, 1, 0.1f,1f,reloadTime), player, pilot);
                    case ShipType.Middle:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 26, 12, 3.2f, 60, 2, 3, 0, 1, 0.15f,1f,reloadTime), player, pilot);
                    case ShipType.Heavy:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 33, 17, 2.6f, 49, 3, 3, 0, 1, 0.2f,1f,reloadTime), player, pilot);
                    case ShipType.Base:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 41, 28, 0.81f, 40, 0, 0, 5, 1, 0f,1f,reloadTime), player, pilot);
                }

                break;
            case ShipConfig.federation:
                switch (shipType)
                {
                    case ShipType.Light:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 20, 16, 3.4f, 70, 2, 1, 0, 1, 0.2f,1f,reloadTime), player, pilot);
                    case ShipType.Middle:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 25, 21, 2.7f, 59, 3, 2, 0, 1, 0.25f,1f,reloadTime), player, pilot);
                    case ShipType.Heavy:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 33, 28, 1.9f, 48, 4, 2, 0, 1, 0.3f,1f,reloadTime), player, pilot);
                    case ShipType.Base:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 35, 35, 0.71f, 40, 0, 0, 6, 1, 0f,1f,reloadTime), player, pilot);
                }

                break;
            case ShipConfig.ocrons:
                switch (shipType)
                {
                    case ShipType.Light:
                        return new ShipInventory(new StartShipParams(shipType, config, 52, 0, 3.6f, 75, 2, 3, 0, 1, 0f,1f, reloadTime),
                            player, pilot);
                    case ShipType.Middle:
                        return new ShipInventory(new StartShipParams(shipType, config, 62, 0, 3.2f, 62, 2, 3, 0, 1, 0f,1f, reloadTime),
                            player, pilot);
                    case ShipType.Heavy:
                        var ship3 = new ShipInventory(
                            new StartShipParams(shipType, config, 74, 0, 3.0f, 50, 3, 4, 0, 1, 0f,1f,reloadTime), player, pilot);
                        ship3.ShiledArmor = 1;
                        return ship3;
                    case ShipType.Base:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 66, 10, 0.75f, 40, 0, 0, 4, 1, 0f,1f,reloadTime), player, pilot);
                }

                break;
            case ShipConfig.krios:
                switch (shipType)
                {
                    case ShipType.Light:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 14, 22, 3.6f, 77, 2, 2, 0, 1, 0.3f,1f,reloadTime), player, pilot);
                    case ShipType.Middle:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 18, 34, 3.0f, 65, 2, 2, 0, 1, 0.35f,1f,reloadTime), player, pilot);
                    case ShipType.Heavy:
                        var ship = new ShipInventory(
                            new StartShipParams(shipType, config, 20, 44, 2.2f, 53, 2, 3, 0, 1, 0.4f,1f,reloadTime), player, pilot);
                        ship.BodyArmor = 2;
                        return ship;
                    case ShipType.Base:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 16, 55, 0.6f, 40, 0, 0, 6, 1, 0f,1f,reloadTime), player, pilot);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(config), config, null);
        }

        return null;
    }

    public static BaseSpellModulInv CreateSpell(int level = 1)
    {
        var allSpells = ArmyCreatorData.AllSpellsStatic();
        var spell = CreateSpell(allSpells.RandomElement());
        for (int i = 0; i < level-1; i++)
        {
            spell.Upgrade();
        }
        return spell;
    }

    public static BaseSpellModulInv CreateSpell(SpellType spellType)
    {
        switch (spellType)
        {
            case SpellType.mineField:
                return new MineFieldSpell(2, 40);
//            case SpellType.invisibility:
//                return new InvisibleSpell(3, 20);
//            case SpellType.allToBase:
//                return new AllToBaseSpell(2, 90);
//            case SpellType.shildDamage:
            case SpellType.randomDamage:
                return new RandomDamageSpell(3, 20);
            case SpellType.lineShot:
                return new LineShotSpell(5, 14);
            case SpellType.throwAround:
                return new ThrowAroundSpell(2, 18);
            case SpellType.engineLock:
                return new EngineLockSpell(2, 22);
//            case SpellType.spaceWall:
//                return new ShieldWallSpell(2, 35);
            case SpellType.distShot:
                return new DistShotSpell(3, 17);
            case SpellType.shildDamage:
                return new ShieldOffSpell(3, 17);
            case SpellType.artilleryPeriod:
                return new ArtillerySpell(4, 10);
            case SpellType.repairDrones:
                return new RepairDronesSpell(4, 30);
            case SpellType.rechargeShield:
                return new RechargeShieldSpell(1, 30);
            case SpellType.roundWave:
                return new WaveStrikeOnShipSpell(2, 20);

            default:
                Debug.LogError("spellType not implemented " + spellType);
                break;
        }

        return null;
    }

    public static BaseModulInv CreatSimpleModul(SimpleModulType type, int level)
    {
        if (LibraryModuls.IsSupport(type))
            return CreateSupport(type, level);
        return new BaseModulInv(type, level);
    }

    public static BaseModulInv CreatSimpleModul(int level)
    {
        var list = LibraryModuls.GetExistsCacheList();
        var type = list.RandomElement();
        return CreatSimpleModul(type, level);
    }

    public static BaseModulInv CreatSimpleModul(int level, int maxLvl)
    {
        var list = LibraryModuls.GetExistsCacheList();
        var onlyLevel = list.Where(x => BaseModulInv.GetBaseReuire(x) <= maxLvl).ToList();
        SimpleModulType type;
        if (onlyLevel.Count > 0)
        {
            type = onlyLevel.RandomElement();
        }
        else
        {
            Debug.LogError($"can't create moduls with max level:{maxLvl}");
            type = list.RandomElement();
        }

        return CreatSimpleModul(type, level);
    }

    private static BaseSupportModul CreateSupport(SimpleModulType rnd, int level)
    {
        switch (rnd)
        {
            case SimpleModulType.WeaponSpeed:
                return new WeaponSpeedModul(level);
            case SimpleModulType.WeaponSpray:
                return new WeaponSprayModul(level);
            case SimpleModulType.WeaponDist:
                return new WeaponDistModul(level);
            case SimpleModulType.WeaponPush:
                return new WeaponPushModul(level);
            case SimpleModulType.WeaponFire:
                return new WeaponDamageTimeEffect(ShipDamageType.fire, level);
            case SimpleModulType.WeaponEngine:
                return new WeaponDamageTimeEffect(ShipDamageType.engine, level);
            case SimpleModulType.WeaponShield:
                return new WeaponDamageTimeEffect(ShipDamageType.shiled, level);
            // case SimpleModulType.WeaponWeapon:
            //     return new WeaponDamageTimeEffect(ShipDamageType.weapon, level);
            case SimpleModulType.WeaponCrit:
                return new WeaponCritModul(level);
            case SimpleModulType.WeaponAOE:
                return new WeaponAOEModul(level);
            case SimpleModulType.WeaponSector:
                return new WeaponSectorModul(level);
            case SimpleModulType.WeaponLessDist:
                return new WeaponLessDist(level);
            case SimpleModulType.WeaponChain:
                return new WeaponChainModul(level);
            case SimpleModulType.WeaponShieldIgnore:
                return new WeaponShieldIgnore(level);
            case SimpleModulType.WeaponSelfDamage:
                return new WeaponSelfDamageModul(level);
            case SimpleModulType.WeaponShieldPerHit:
                return new WeaponShieldPerHit(level);
            case SimpleModulType.WeaponShootPerTime:
                return new WeaponPerTimeBullet(level);
            case SimpleModulType.WeaponNoBulletDeath:
                return new WeaponNoDeathModul(level);
            case SimpleModulType.WeaponPowerShot:
                return new WeaponPowerShot(level);
            case SimpleModulType.WeaponFireNear:
                return new WeaponFireNearModul(level);
            case SimpleModulType.EMIUpgrade:
                return new AbstractWeaponUpgradeModul(WeaponType.eimRocket, rnd, level);
            case SimpleModulType.bombUpgrade:
                return new AbstractWeaponUpgradeModul(WeaponType.casset, rnd, level);
            case SimpleModulType.impulseUpgrade:
                return new AbstractWeaponUpgradeModul(WeaponType.impulse, rnd, level);
            case SimpleModulType.rocketUpgrade:
                return new AbstractWeaponUpgradeModul(WeaponType.rocket, rnd, level);
            case SimpleModulType.beamUpgrade:
                return new AbstractWeaponUpgradeModul(WeaponType.beam, rnd, level);
            case SimpleModulType.laserUpgrade:
                return new AbstractWeaponUpgradeModul(WeaponType.laser, rnd, level);
            case SimpleModulType.ShipSpeed:
                return new ShipSpeedModul(level);
            case SimpleModulType.ShipTurnSpeed:
                return new ShipTurnSpeedModul(level);
            case SimpleModulType.ShipDecreaseSpeed:
                return new ShipDecreaseSpeedModul(level);
            case SimpleModulType.ShieldDouble:
                return new ShipShieldDoubleModul(level);
            default:
                Debug.LogError($"this is not support {rnd}");
                throw new ArgumentOutOfRangeException();
        }
    }

    public static PilotParameters CreateDebugPilot()
    {
        var pilot = new PilotParameters();
        return pilot;
    }

    public static float CalcTrickReload(PilotRank rank, ShipType shipType)
    {
        var calculatedBoostTime = 60f;
        var pilotBoostCoef = 1f;
        switch (rank)
        {
            case PilotRank.Lieutenant:
                pilotBoostCoef = 0.8f;
                break;
            case PilotRank.Captain:
                pilotBoostCoef = 0.65f;
                break;
            case PilotRank.Major:
                pilotBoostCoef = 0.5f;
                break;
        }

        switch (shipType)
        {
            case ShipType.Light:
                calculatedBoostTime = 12f;
                break;
            case ShipType.Middle:
                calculatedBoostTime = 17f;
                break;
            case ShipType.Heavy:
                calculatedBoostTime = 22f;
                break;
        }

        return Mathf.Clamp(calculatedBoostTime * pilotBoostCoef, 5f, 60f);
    }

    public static float CalcPower(StartShipPilotData data)
    {
        var ship = CalcShipPower(data.Ship);
        var pilot = CalcPilotPower(data.Pilot);
        return ship + pilot;
    }

    public static float CalcShipPower(ShipInventory ship)
    {
        var sum = ship.WeaponsModuls.Where(weaponsModul => weaponsModul != null).Sum(weaponsModul =>
                      BASE_WEAPON_VALUE + (weaponsModul.Level - 1) * WEAPON_LEVEL_COEF) +
                  ship.Moduls.SimpleModuls.Where(simple => simple != null).Sum(simple => BASE_SIMPLE_MODUL_VALUE) +
                  ship.SpellsModuls.Where(spell => spell != null).Sum(spell => BASE_SPELL_VALUE);
//        float shipCoef = ShipPowerCoef(ship.ShipType);
//        var t = sum*shipCoef + BASE_SHIP_VALUE;
        var t = sum + BASE_SHIP_VALUE;
        return t;
    }

    public static float ShipPowerCoef(ShipType shipType)
    {
        var shipCoef = 1f;
        switch (shipType)
        {
            case ShipType.Light:
                shipCoef = 1.6f;
                break;
            case ShipType.Middle:
                shipCoef = 1.3f;
                break;
            case ShipType.Heavy:
                shipCoef = 1f;
                break;
            case ShipType.Base:
                shipCoef = 1f;
                break;
        }

        return shipCoef;
    }

    public static float CalcPilotPower(IPilotParameters pilot)
    {
        var sumLevel = pilot.HealthLevel + pilot.ShieldLevel + pilot.SpeedLevel + pilot.TurnSpeedLevel;
        var ranAsInt = (int) pilot.Stats.CurRank;
        return (sumLevel - 4) * PILOT_LEVEL_COEF + ranAsInt * PILOT_RANK_COEF;
    }

    public static int GetReapairCost(int hpPointToRepair, int shipLevel)
    {
        if (hpPointToRepair == 0) return 0;
        return (hpPointToRepair + shipLevel) / 5;
    }


    #region GLOBAL_MAP_DATA

    public const float MIN_GLOBAL_SECTOR_SIZE = 3;
    public const float MAX_GLOBAL_SECTOR_SIZE = 5;
    public const float MIN_GLOBAL_MAP_DEATHSTART = 1;
    public const float MAX_GLOBAL_MAP_DEATHSTART = 10;
    public const float MIN_GLOBAL_MAP_CORES = 2;
    public const float MAX_GLOBAL_MAP_CORES = 5;   
    public const float MIN_GLOBAL_MAP_ADDITIONAL_POWER = 0;
    public const float MAX_GLOBAL_MAP_ADDITIONAL_POWER = 10;

    public const int MAX_GLOBAL_MAP_VERYEASY_BASE_POWER = 6;
    public const int MIN_GLOBAL_MAP_EASY_BASE_POWER = 7;
    public const int MIN_GLOBAL_MAP_NORMAL_BASE_POWER = 8;
    public const int MIN_GLOBAL_MAP_HARD_BASE_POWER = 9;
    public const int MIN_GLOBAL_MAP_IMPOSIBLE_BASE_POWER = 10;


    public const int MIN_GLOBAL_MAP_SECTOR_COUNT = 3;
    public const int MAX_GLOBAL_MAP_SECTOR_COUNT = 8;
    public const float SELL_COEF = 0.5f;

    #endregion

    #region REPUTATION

    public static int REPUTATION_REPAIR_ADD = 12;
    public static int REPUTATION_SCIENS_LAB_ADD = 7;
    public static int REPUTATION_FIND_WAY_ADD = 8;
    public static float CHARGE_SPEED_COEF_PER_LEVEL = 0.12f;
    public static float REPURARTION_TO_DIPLOMATY_COEF = .05f;
    public static float MONEY_QUEST_COEF = 0.09f;
    public static int PEACE_REPUTATION = 50;
    public static int START_REPUTATION = -35;
    public static int BATTLE_REPUTATION_AFTER_FIGHT = 4;

    public const int REPUTATION_STEAL_REMOVE = 5;
    public const int REPUTATION_REPAIR_REMOVE = 4;
    public const int REPUTATION_ATTACK_PEACEFULL_REMOVE = 14;
    public const int REPUTATION_FRIGHTEN_SHIP_REMOVE = 9;
    public const int REPUTATION_HIRE_CRIMINAL_REMOVED = 10;
    public const float REPAIR_PERCENT_PERSTEP_PERLEVEL = 0.08f;

    #endregion
}