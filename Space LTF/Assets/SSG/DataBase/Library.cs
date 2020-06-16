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
    public const float SECTOR_COEF_POWER = 0.47f;
    public const float COEF_CORE_ARMY = 1.3f;
    public static float SECTOR_POWER_START_COEF = 0.048f;
    public static float SECTOR_POWER_LOG1 = 14f;
    public static float SECTOR_POWER_LOG2 = 0.162f;
    public static float SECTOR_POWER_LOG3 = 34f;

    public const int MAX_WEAPON_LVL = 5;
    public const int MAX_SPELL_LVL = 4;
    public const int SPECIAL_SPELL_LVL = 4;
    public const int MAX_MOUDL_LEVEL = 3;

    public static int PARAMETER_ITEM_COST_NORMAL = 8;
    public static int PARAMETER_ITEM_COST_IMPROVED = 25;
    public static int PARAMETER_ITEM_COST_PERFECT = 50;

    #region CONFIGS COEF

    public const float RAIDER_SPEED_COEF = 1.05f;
    public const float RAIDER_TURNSPEED_COEF = 1.105f; 
    
    public const float MERC_SPEED_COEF = 1.05f;
    public const float MERC_TURNSPEED_COEF = 1.15f;    
    
    public const float OCRONS_SHIELD_COEF = 0f;
    public const float OCRONS_HP_COEF = 1.6f;    
    
    public const float KRIOS_SHIELD_COEF = 1.4f;
    public const float KRIOS_HP_COEF = 0.5f;


    #endregion

    public const float MIN_WORKING_SHIP = 6;
    public const float MAX_ARMY_POWER_MAP = 70;

    public const float REPAIR_DISCOUTNT = 1.2f;
    public const float COST_REPAIR_CRIT = 1.1f;

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

    private const float SUPPORT_ANG = 180f;
    private const float SUPPORT_SPEED = 4.0f;
    private const float SUPPORT_RELOAD = 9.0f;
    private const float SUPPORT_TURN_SPEED = 104.0f;
    // private const float SUPPORT_ANG = 44f;

    private const float BEAM_ANG = 180f;
    private const float BEAM_DELAY = 3f;
    private const float BEAM_SPEED = 1f;

    private const float EMI_ANG = 50f;
    private const float EMI_DELAY = 7f;
    private const float EMI_SPEED = 15f;

    public const float MOVING_ARMY_POWER_COEF = 1.5f;

    public const float BASE_WEAPON_VALUE = 1.2f;
    public const float BASE_SPELL_VALUE = 1.1f;
    public const float BASE_SPELL_VALUE_LEVEL = 0.35f;
    public const float BASE_SIMPLE_MODUL_VALUE = 0.8f;
    public const float BASE_SIMPLE_MODUL_VALUE_UPGRADE = 0.6f;
    public const float MIN_POINTS_TO_CREATE_ARMY_WITH_BASESHIP = 20f;
    public const float BASE_SHIP_VALUE = 2f;
    public const float BASE_TURRET_VALUE = 1f;
    public const float WEAPON_LEVEL_COEF = 0.8f;
    public const float PILOT_LEVEL_COEF = 0.15f;
    public const float PILOT_RANK_COEF = 0f; //0.7f;
    public const int MAX_PILOT_PARAMETER_LEVEL = 80;
    public static float RELOAD_COEF_DIF_WEAPONS = 2f;

    public const int COINS_TO_POWER_WEAPON_SHIP_SHIELD = 1;
    public const int COINS_TO_POWER_WEAPON_SHIP_SHIELD_DELAY = 40;
    public const int COINS_TO_CHARGE_SHIP_SHIELD = 1;
    public const int COINS_TO_WAVE_SHIP = 1;
    public const int COINS_TO_CHARGE_SHIP_SHIELD_DELAY = 20;
    public const int COINS_TO_WAVE_SHIP_DELAY = 30;
    public const float CHARGE_SHIP_SHIELD_HEAL_PERCENT = 0.36f;

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
    // public static int RANK_ERIOD = 12;
    public static int BASE_CHARGES_COUNT = 5;
    public static int WEAPON_REQUIRE_LEVEL_COEF = 2;
    public static int MODUL_REQUIRE_LEVEL_COEF = 2;

    // public static Dictionary<PilotRank, List<EPilotTricks>> PosibleTricks;

    public static void Init()
    {
        ArmyCreatorLibrary.Init();
        _lvlUps = new int[MAX_PILOT_PARAMETER_LEVEL];
        _lvlUps[0] = 8;
        _lvlUps[1] = 8;
        _lvlUps[2] = 10;
        _lvlUps[3] = 13;
        _lvlUps[4] = 14;
        _lvlUps[5] = 18;
        _lvlUps[6] = 22;
        _lvlUps[7] = 28;
        _lvlUps[8] = 32;
        _lvlUps[9] = 36;
        _lvlUps[10] = 39;
        _lvlUps[11] = 43;
        _lvlUps[12] = 46;
        _lvlUps[13] = 48;
        _lvlUps[14] = 50;
        _lvlUps[15] = 54;
        for (var i = 15; i < MAX_PILOT_PARAMETER_LEVEL; i++)
        {
            _lvlUps[i] = (int)((i - 10) * 5 + 50);
        }
        LibraryModuls.Init();

        // PosibleTricks = new Dictionary<PilotRank, List<EPilotTricks>>
        // {
        //     {PilotRank.Private, new List<EPilotTricks> {EPilotTricks.turn}},
        //     {PilotRank.Lieutenant, new List<EPilotTricks> {EPilotTricks.turn, EPilotTricks.twist}},
        //     {PilotRank.Captain, new List<EPilotTricks> {EPilotTricks.turn, EPilotTricks.twist, EPilotTricks.loop}},
        //     {PilotRank.Major, new List<EPilotTricks> {EPilotTricks.turn, EPilotTricks.twist, EPilotTricks.loop}}
        // };
    }

    public static Color GetColorByConfig(ShipConfig config)
    {
        switch (config)
        {
            case ShipConfig.raiders:
                return Utils.CreateColor(90,255,0);
            case ShipConfig.federation:
                return Utils.CreateColor(255,0,192);
            case ShipConfig.mercenary:
                return Utils.CreateColor(0,252,252);
            case ShipConfig.ocrons:
                return Utils.CreateColor(255,110,0);
            case ShipConfig.krios:
                return Utils.CreateColor(0,50,250);
            case ShipConfig.droid:
                return Utils.CreateColor(210,0,0);
        }
        return Color.white;
    }

    public static int PilotLvlUpCost(int curLvl)
    {
        return _lvlUps[curLvl];
    }

    public static WeaponInv CreateWeaponByType(WeaponType weapon)
    {
        WeaponInventoryParameters parametes;
        switch (weapon)
        {
            //DAMAGE
            case WeaponType.laser:
                parametes = new WeaponInventoryParameters(8, 8, 2, 2, LASER_ANG, LASER_DELAY, 0.5f, 1, LASER_SPEED, 8, 0, TargetType.Enemy);
                return new LaserInventory(parametes, 1);
            case WeaponType.rocket:
                parametes = new WeaponInventoryParameters(4, 12, 1, 3, ROCKET_ANG, ROCKET_DELAY, 0.5f, 1, ROCKET_SPEED, 11,
                    36f, TargetType.Enemy);
                return new RocketInventory(parametes, 1);
            case WeaponType.impulse:
                parametes = new WeaponInventoryParameters(5, 2, 2, 1, LASER_ANG, IMPULSE_DELAY, 0.5f, 2, IMPULSE_SPEED, 6, 0f, TargetType.Enemy);
                return new ImpulseInventory(parametes, 1);
            case WeaponType.casset:
                parametes = new WeaponInventoryParameters(4, 8, 1, 2, MINE_ANG, MINE_DELAY, 0.5f, 1, MINE_SPEED, 9, 70f, TargetType.Enemy);
                return new BombInventoryWeapon(parametes, 1);
            case WeaponType.eimRocket:
                parametes = new WeaponInventoryParameters(4, 4, 2, 1, EMI_ANG, EMI_DELAY, 0.4f, 2, EMI_SPEED, 11, 0f, TargetType.Enemy);
                return new EMIRocketInventory(parametes, 1);
            case WeaponType.beam:
                parametes = new WeaponInventoryParameters(2, 8, 1, 3, BEAM_ANG, BEAM_DELAY, 0.4f, 1, BEAM_SPEED, 2.5f, 0f, TargetType.Enemy);
                return new BeamWeaponInventory(parametes, 1);

            //SUPPORT
            case WeaponType.healBodySupport:
                parametes = new WeaponInventoryParameters(0, 4, 0, 1, SUPPORT_ANG, SUPPORT_RELOAD, 0.4f, 1, SUPPORT_SPEED, 4.5f, SUPPORT_TURN_SPEED, TargetType.Ally);
                return new HealSuppotWeaponInventory(parametes, 1);
            case WeaponType.healShieldSupport:
                parametes = new WeaponInventoryParameters(4, 0, 1, 0, SUPPORT_ANG, SUPPORT_RELOAD, 0.4f, 1, SUPPORT_SPEED, 4.5f, SUPPORT_TURN_SPEED, TargetType.Ally);
                return new ShieldSuppotWeaponInventory(parametes, 1);




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

    public static WeaponInv CreateDamageWeapon(bool low)
    {
        var level = new WDictionary<int>(new Dictionary<int, float>
        {
            {1, low ? 15 : 10},
            {2, low ? 4 : 3},
            {3, low ? 0.5f : 1}
        });
        return CreateDamageWeapon(level.Random());
    }

    public static WeaponInv CreatWeapon(int level)
    {
        var types = new WDictionary<WeaponType>(new Dictionary<WeaponType, float>
        {
            {WeaponType.laser, 9},
            {WeaponType.rocket, 9},
            {WeaponType.eimRocket, 7},
            {WeaponType.casset, 7},
            {WeaponType.impulse, 7},
            {WeaponType.beam, 7}   ,
            {WeaponType.healBodySupport, 5},
            {WeaponType.healShieldSupport, 5},
        });

        var w = CreateWeaponByType(types.Random());
        w.Level = level;
        return w;
    }

    public static WeaponInv CreateDamageWeapon(int level)
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

        var w = CreateWeaponByType(types.Random());
        w.Level = level;
        return w;
    }
    public static WeaponInv CreateSupportWeapon(int level)
    {
        var types = new WDictionary<WeaponType>(new Dictionary<WeaponType, float>
        {
            {WeaponType.healBodySupport, 9},
            {WeaponType.healShieldSupport, 9},
        });

        var w = CreateWeaponByType(types.Random());
        w.Level = level;
        return w;
    }

    public static ShipInventory CreateShip(ShipType shipType, ShipConfig config, PlayerSafe player, PilotParameters pilot)
    {
        float hull = 34;
        float shield = 0;
        if (shipType == ShipType.Turret)
        {
            switch (config)
            {
                case ShipConfig.raiders:
                case ShipConfig.mercenary:
                case ShipConfig.droid:

                    break;
                case ShipConfig.federation:
                    hull = 26;
                    shield = 12;
                    break;
                case ShipConfig.ocrons:
                    hull = 44;
                    shield = 0;
                    break;
                case ShipConfig.krios:
                    hull = 14;
                    shield = 24;
                    break;
            }
            var par = new StartShipParams(ShipType.Turret, config, hull, shield, 0, 90, 2, 1, 0, 0, 0, 1f, 0f);
            return new ShipInventory(par, player, pilot);
        }
        var reloadTime = CalcTrickReload(pilot.Stats.CurRank, shipType);
        switch (config)
        {
            case ShipConfig.droid:
                switch (shipType)
                {
                    case ShipType.Light:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 19, 2, 0.2f, 29, 1, 0, 0, 1, 0.0f, 1f, reloadTime), player, pilot);
                    case ShipType.Middle:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 13, 16, 2.6f, 33, 1, 0, 0, 1, 0.0f, 1f, reloadTime), player, pilot);
                    case ShipType.Heavy:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 1, 26, 2.2f, 53, 2, 0, 0, 1, 0.0f, 1f, reloadTime), player, pilot);
                    case ShipType.Base:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 66, 20, 0.51f, 40, 0, 0, 6, 1, 0f, 1f, reloadTime), player, pilot);
                }

                break;
            case ShipConfig.raiders:
                switch (shipType)
                {
                    case ShipType.Light:
                        var ship = new ShipInventory(
                            new StartShipParams(shipType, config, 52, 34, 1f, 16, 1, 3, 0, 1, 0.0f, 1f, reloadTime), player, pilot);
                        ship.ShiledArmor = 2;
                        ship.BodyArmor = 1;
                        return ship;
                    case ShipType.Middle:
                        var ship1 = new ShipInventory(
                            new StartShipParams(shipType, config, 51, 38, 2.5f, 25, 1, 3, 0, 1, 0.05f, 1f, reloadTime), player, pilot);
                        ship1.ShiledArmor = 2;
                        return ship1;
                    case ShipType.Heavy:
                        var ship2 = new ShipInventory(
                            new StartShipParams(shipType, config, 44, 42, 3.0f, 55, 2, 2, 0, 1, 0.1f, 1f, reloadTime), player, pilot);
                        ship2.ShiledArmor = 2;
                        return ship2;
                    case ShipType.Base:
                        var ship3 = new ShipInventory(
                            new StartShipParams(shipType, config, 86, 60, 1.5f, 40, 0, 0, 6, 1, 0f, 1f, reloadTime), player, pilot);
                        ship3.ShiledArmor = 1;
                        ship3.BodyArmor = 1;
                        return ship3;
                }

                break;
            case ShipConfig.mercenary:
                switch (shipType)
                {
                    case ShipType.Light:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 70, 22, 0.8f, 5, 2, 2, 0, 1, 0.2f, 1f, reloadTime), player, pilot);
                    case ShipType.Middle:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 63, 24, 2.2f, 10, 2, 2, 0, 1, 0.3f, 1f, reloadTime), player, pilot);
                    case ShipType.Heavy:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 57, 34, 2.5f, 33, 3, 1, 0, 1, 0.4f, 1f, reloadTime), player, pilot);
                    case ShipType.Base:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 92, 56, 0.81f, 40, 0, 0, 5, 1, 0f, 1f, reloadTime), player, pilot);
                }

                break;
            case ShipConfig.federation:
                switch (shipType)
                {
                    case ShipType.Light:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 30, 32, 0.7f, 7, 3, 1, 0, 1, 0.2f, 1f, reloadTime), player, pilot);
                    case ShipType.Middle:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 54, 42, 1.9f, 13, 3, 1, 0, 1, 0.25f, 1f, reloadTime), player, pilot);
                    case ShipType.Heavy:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 78, 48, 1.6f, 38, 4, 0, 0, 1, 0.3f, 1f, reloadTime), player, pilot);
                    case ShipType.Base:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 70, 70, 0.71f, 40, 0, 0, 6, 1, 0f, 1f, reloadTime), player, pilot);
                }

                break;
            case ShipConfig.ocrons:
                switch (shipType)
                {
                    case ShipType.Light:
                        return new ShipInventory(new StartShipParams(shipType, config, 70, 0, 0.4f, 12, 2, 2, 0, 1, 0f, 1f, reloadTime),
                            player, pilot);
                    case ShipType.Middle:
                        return new ShipInventory(new StartShipParams(shipType, config, 57, 0, 2.2f, 11, 3, 1, 0, 1, 0f, 1f, reloadTime),
                            player, pilot);
                    case ShipType.Heavy:
                        var ship3 = new ShipInventory(new StartShipParams(shipType, config, 63, 0, 2.8f, 35, 3, 1, 0, 1, 0f, 1f, reloadTime), player, pilot);
                        ship3.BodyArmor = 1;
                        return ship3;
                    case ShipType.Base:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 132, 20, 0.75f, 40, 0, 0, 5, 1, 0f, 1f, reloadTime), player, pilot);
                }

                break;
            case ShipConfig.krios:
                switch (shipType)
                {
                    case ShipType.Light:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 53, 30, 0.7f, 29, 2, 2, 0, 1, 0.5f, 1f, reloadTime), player, pilot);
                    case ShipType.Middle:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 41, 50, 2.6f, 33, 2, 2, 0, 1, 0.6f, 1f, reloadTime), player, pilot);
                    case ShipType.Heavy:
                        var ship = new ShipInventory(
                            new StartShipParams(shipType, config, 31, 62, 2.2f, 53, 2, 1, 0, 1, 0.7f, 1f, reloadTime), player, pilot);
                        ship.ShiledArmor = 1;
                        return ship;
                    case ShipType.Base:
                        return new ShipInventory(
                            new StartShipParams(shipType, config, 40, 110, 0.6f, 40, 0, 0, 6, 1, 0.4f, 1f, reloadTime), player, pilot);
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
        for (int i = 0; i < level - 1; i++)
        {
            spell.Upgrade(ESpellUpgradeType.None);
        }
        return spell;
    }

    public static BaseSpellModulInv CreateSpell(SpellType spellType)
    {
        switch (spellType)
        {
            case SpellType.mineField:
                return new MineFieldSpell();
            //            case SpellType.invisibility:
            //                return new InvisibleSpell(3, 20);
            //            case SpellType.allToBase:
            //                return new AllToBaseSpell(2, 90);
            //            case SpellType.shildDamage:
            // case SpellType.randomDamage:
            //     return new RandomDamageSpell(3, 20);
            case SpellType.lineShot:
                return new LineShotSpell();
            case SpellType.throwAround:
                return new ThrowAroundSpell();
            case SpellType.engineLock:
                return new EngineLockSpell();
            //            case SpellType.spaceWall:
            //                return new ShieldWallSpell(2, 35);
            case SpellType.distShot:
                return new DistShotSpell();
            case SpellType.shildDamage:
                return new ShieldOffSpell();
            case SpellType.artilleryPeriod:
                return new ArtillerySpell();
            case SpellType.repairDrones:
                return new RepairDronesSpell();
            case SpellType.rechargeShield:
                return new RechargeShieldSpell();
            // case SpellType.roundWave:
            //     return new WaveStrikeOnShipSpell();
            case SpellType.machineGun:
                return new MachineGunSpell();
            case SpellType.vacuum:
                return new VacuumSpell();
            case SpellType.hookShot:
                return new HookShotSpell();
            //            case SpellType.mainShipBlink:
            //                return new Telepo();  

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
                return new WeaponBulletSpeedModul(level);
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
            case SimpleModulType.WeaponMultiTarget:
                return new WeaponMultiTargetModul(level);
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
        var weapons = ship.WeaponsModuls.GetNonNullActiveSlots();
        var sum = weapons.Sum(weaponsModul =>
                      BASE_WEAPON_VALUE + (weaponsModul.Level - 1) * WEAPON_LEVEL_COEF) +
                  ship.Moduls.GetNonNullActiveSlots().Sum(simple =>
                      BASE_SIMPLE_MODUL_VALUE + (simple.Level - 1) * BASE_SIMPLE_MODUL_VALUE_UPGRADE) +
                  ship.SpellsModuls.Where(spell => spell != null)
                      .Sum(spell => BASE_SPELL_VALUE + +(spell.Level - 1) * BASE_SPELL_VALUE_LEVEL);
        //        float shipCoef = ShipPowerCoef(ship.ShipType);
        //        var t = sum*shipCoef + BASE_SHIP_VALUE;
        float t;
        if (ship.ShipType == ShipType.Turret)
        {
            t = sum + BASE_TURRET_VALUE;
        }
        else
        {
            t = sum + BASE_SHIP_VALUE;
        }

        return t;
    }

    // public static float ShipPowerCoef(ShipType shipType)
    // {
    //     var shipCoef = 1f;
    //     switch (shipType)
    //     {
    //         case ShipType.Light:
    //             shipCoef = 1.6f;
    //             break;
    //         case ShipType.Middle:
    //             shipCoef = 1.3f;
    //             break;
    //         case ShipType.Heavy:
    //             shipCoef = 1f;
    //             break;
    //         case ShipType.Base:
    //             shipCoef = 1f;
    //             break;
    //     }
    //
    //     return shipCoef;
    // }

    public static float CalcPilotPower(IPilotParameters pilot)
    {
        var sumLevel = pilot.HealthLevel + pilot.ShieldLevel + pilot.SpeedLevel + pilot.TurnSpeedLevel;
        var ranAsInt = (int)pilot.Stats.CurRank;
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
    public const float MIN_GLOBAL_MAP_QUESTS = 1;
    public const float MAX_GLOBAL_MAP_QUESTS = 4;
    public const float MIN_GLOBAL_MAP_ADDITIONAL_POWER = 1;
    public const float MAX_GLOBAL_MAP_ADDITIONAL_POWER = 10;

    public const int MAX_GLOBAL_MAP_VERYEASY_BASE_POWER = 8;
    public const int MIN_GLOBAL_MAP_EASY_BASE_POWER = 9;
    public const int MIN_GLOBAL_MAP_NORMAL_BASE_POWER = 11;
    public const int MIN_GLOBAL_MAP_HARD_BASE_POWER = 13;
    public const int MIN_GLOBAL_MAP_IMPOSIBLE_BASE_POWER = 15;


    public const int MIN_GLOBAL_MAP_SECTOR_COUNT = 3;
    public const int MAX_GLOBAL_MAP_SECTOR_COUNT = 8;
    public const float SELL_COEF = 0.5f;

    #endregion

    #region REPUTATION

    public static int REPUTATION_REPAIR_ADD = 12;
    public static int REPUTATION_SCIENS_LAB_ADD = 12;
    public static int REPUTATION_FIND_WAY_ADD = 8;
    public static float CHARGE_SPEED_COEF_PER_LEVEL = 0.12f;
    public static float REPURARTION_TO_DIPLOMATY_COEF = .05f;
    public static float MONEY_QUEST_COEF = 0.09f;
    public static int PEACE_REPUTATION = 50;
    public static int START_REPUTATION = -35;
    public static int BATTLE_REPUTATION_AFTER_FIGHT = 4;
    public static int REPUTATION_FOR_REINFORCMENTS = 10;

    public const int REPUTATION_STEAL_REMOVE = 5;
    public const int REPUTATION_REPAIR_REMOVE = 4;
    public const int REPUTATION_ATTACK_PEACEFULL_REMOVE = 14;
    public const int REPUTATION_FRIGHTEN_SHIP_REMOVE = 9;
    public const int REPUTATION_HIRE_CRIMINAL_REMOVED = 10;
    public const float REPAIR_PERCENT_PERSTEP_PERLEVEL = 0.08f;


    #endregion

    public const int BASE_SHIP_EXP = 100;
    public const int BASE_DROID_SHIP_EXP = 50;
    public const int BASE_CONTROL_SHIP_EXP = 50;
    public const int BASE_TURRET_SHIP_EXP = 50;
    public const int KILL_EXP = 25;

    public static Dictionary<PilotRank, int> PilotRankExp = new Dictionary<PilotRank, int>()
    {
        {PilotRank.Private,500 * MoneyConsts.EXP_COEF},
        {PilotRank.Lieutenant,850 * MoneyConsts.EXP_COEF},
        {PilotRank.Captain,1050 * MoneyConsts.EXP_COEF },
        {PilotRank.Major,1400 * MoneyConsts.EXP_COEF},
    };

    public static int DROID_SUPPORTS_PER_SHOOT = 3;

    public static int GetExp(ShipType startParamsShipType, ShipConfig startParamsShipConfig)
    {
        if (startParamsShipConfig == ShipConfig.droid)
        {
            return BASE_DROID_SHIP_EXP;
        }

        switch (startParamsShipType)
        {
            case ShipType.Base:
                return BASE_CONTROL_SHIP_EXP;
            case ShipType.Turret:
                return BASE_TURRET_SHIP_EXP;
            default:
                return BASE_SHIP_EXP;
        }


    }

    public static ParameterItem CreateParameterItem(EParameterItemSubType middle, EParameterItemRarity rarity, ItemType? type = null, List<ItemType> types = null)
    {
        if (!type.HasValue)
        {
            List<ItemType> list;
            if (types == null)
            {
                list = new List<ItemType>() { ItemType.cocpit, ItemType.engine, ItemType.wings };
            }
            else
            {
                list = types;
            }
            type = list.RandomElement();
        }


        var data = new Dictionary<EParameterShip, float>();
        switch (type.Value)
        {
            case ItemType.cocpit:
                switch (middle)
                {
                    case EParameterItemSubType.Light:
                        //                        data.Add(EParameterShip.bodyPoints,10); 
                        data.Add(EParameterShip.turn, (int)MyExtensions.MinorRandom(18f));
                        data.Add(EParameterShip.speed, MyExtensions.MinorRandom(1.0f));
                        break;
                    case EParameterItemSubType.Middle:
                        //                        data.Add(EParameterShip.bodyPoints, 5);  
                        data.Add(EParameterShip.turn, (int)MyExtensions.MinorRandom(12f));
                        data.Add(EParameterShip.speed, MyExtensions.MinorRandom(-0.7f));
                        data.Add(EParameterShip.modulsSlots, 1);
                        break;
                    case EParameterItemSubType.Heavy:
//                        data.Add(EParameterShip.bodyPoints, -18);
                        data.Add(EParameterShip.turn, (int)MyExtensions.MinorRandom(-10f));
                        data.Add(EParameterShip.speed, MyExtensions.MinorRandom(-1.0f));
                        data.Add(EParameterShip.modulsSlots, 2);
                        break;
                }
                break;
            case ItemType.engine:
                switch (middle)
                {
                    case EParameterItemSubType.Light:
                        data.Add(EParameterShip.bodyPoints, (int)MyExtensions.MinorRandom(-5f));
                        data.Add(EParameterShip.speed, MyExtensions.MinorRandom(2.0f));
                        break;
                    case EParameterItemSubType.Middle:
                        data.Add(EParameterShip.bodyPoints, (int)MyExtensions.MinorRandom(5f));
                        data.Add(EParameterShip.speed, MyExtensions.MinorRandom(1.5f));
                        break;
                    case EParameterItemSubType.Heavy:
                        data.Add(EParameterShip.bodyPoints, (int)MyExtensions.MinorRandom(15f));
                        data.Add(EParameterShip.speed, 1f);
                        break;
                }
                break;
            case ItemType.wings:
                switch (middle)
                {
                    case EParameterItemSubType.Light:
                        data.Add(EParameterShip.bodyPoints, (int)MyExtensions.MinorRandom(-5f));
                        data.Add(EParameterShip.turn, (int)MyExtensions.MinorRandom(40f));
                        break;
                    case EParameterItemSubType.Middle:
//                        data.Add(EParameterShip.bodyPoints, 0);
                        data.Add(EParameterShip.turn, (int)MyExtensions.MinorRandom(30f));
                        data.Add(EParameterShip.bodyArmor, 1);
                        break;
                    case EParameterItemSubType.Heavy:
                        data.Add(EParameterShip.bodyPoints, (int)MyExtensions.MinorRandom(5f));
                        data.Add(EParameterShip.turn, (int)MyExtensions.MinorRandom(20f));
                        data.Add(EParameterShip.bodyArmor, 2);
                        break;
                }
                break;
        }
        List<EParameterShip> rndPoints = new List<EParameterShip>()
        {
            EParameterShip.bodyArmor,EParameterShip.bodyPoints,EParameterShip.shieldPoints,EParameterShip.speed,EParameterShip.turn
        };
        foreach (var f in data)
        {
            rndPoints.Remove(f.Key);
        }
        void AddRndPoint()
        {
            if (rndPoints.Count > 0)
            {
                var rnd = rndPoints.RandomElement();
                float val = 0f;
                switch (rnd)
                {
                    case EParameterShip.speed:
                        val = MyExtensions.Random(4, 8);
                        break;
                    case EParameterShip.turn:
                        val = MyExtensions.Random(5, 9);
                        break;
                    case EParameterShip.bodyPoints:
                        val = MyExtensions.Random(8, 16);
                        break;
                    case EParameterShip.shieldPoints:
                        val = MyExtensions.Random(6, 14);
                        break;
                    case EParameterShip.bodyArmor:
                        val = 1f;
                        break;
                }

                rndPoints.Remove(rnd);
                data.Add(rnd, val);
            }
        }
        switch (rarity)
        {
            case EParameterItemRarity.improved:
                AddRndPoint();
                break;
            case EParameterItemRarity.perfect:
                if (MyExtensions.IsTrue01(.5f))
                {
                    AddRndPoint();
                    AddRndPoint();
                }
                else
                {
                    if (MyExtensions.IsTrue01(.4f) && !data.ContainsKey(EParameterShip.modulsSlots))
                    {                      
                        data.Add(EParameterShip.modulsSlots, 1);
                    }
                    else
                    {
                        data.Add(EParameterShip.weaponSlots, 1);
                    }
                }
                break;
        }
        var item = new ParameterItem(type.Value, rarity, middle, data);
        return item;
    }
    public static  List<EParameterItemSubType> ParameterItemTypes = new List<EParameterItemSubType>() { EParameterItemSubType.Heavy, EParameterItemSubType.Light, EParameterItemSubType.Middle };


    public static WDictionary<EParameterItemRarity> GetParitiesCacheShop = new WDictionary<EParameterItemRarity>( 
        new Dictionary<EParameterItemRarity, float>()
    {
        {EParameterItemRarity.perfect,5},
        {EParameterItemRarity.normal,20},
        {EParameterItemRarity.improved,8},
    });


    public const float PARAMETER_LEVEL_COEF = 0.05f;
    public const float LOW_MONEY_COEF = 0.4f;
    public const float NORMAL_MONEY_COEF = 1f;
}