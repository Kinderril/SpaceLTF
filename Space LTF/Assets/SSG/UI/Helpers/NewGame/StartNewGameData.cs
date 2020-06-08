using System.Collections.Generic;
using UnityEngine;

public enum EGameMode
{
                 sandBox,
//                 sandBox,
                 simpleTutor,
                 advTutor,
                 safePlayer,
}

public class StartNewGameData
{
    public EStartGameDifficulty Difficulty;
    public int StepsBeforeDeath;
    public int QuestsOnStart;
    public int SectorSize;
    public int SectorCount;
    public List<WeaponType> posibleStartWeapons;
    public ShipConfig shipConfig;
    public List<SpellType> posibleSpell;
    public Dictionary<PlayerParameterType, int> startParametersLevels;
    public int PowerPerTurn;
    public EGameMode GameNode;

    public StartNewGameData(Dictionary<PlayerParameterType, int> startParametersLevels,
        ShipConfig shipConfig, List<WeaponType> posibleStartWeapons, int SectorSize, int SectorCount,
         int questsOnStart, EStartGameDifficulty difficulty, List<SpellType> posibleSpell
        , int PowerPerTurn, EGameMode gameNode)
    {
        Debug.Log(($"StartNewGameData {shipConfig.ToString()} SectorSize:{SectorSize} " +
                  $" SectorCount:{SectorCount}  questsOnStart:{questsOnStart}" +
                  $"  Difficulty:{Difficulty}  PowerPerTurn:{PowerPerTurn}   posibleSpell:{posibleSpell}").Red());
        this.startParametersLevels = startParametersLevels;
        this.shipConfig = shipConfig;
        this.SectorSize = SectorSize;
        this.SectorCount = SectorCount;
        this.QuestsOnStart = questsOnStart;
        this.StepsBeforeDeath = 999;
        this.posibleStartWeapons = posibleStartWeapons;
        this.Difficulty = difficulty;
        this.posibleSpell = posibleSpell;
        this.PowerPerTurn = PowerPerTurn;
        GameNode = gameNode;
    }


    public float CalcDifficulty()
    {
        var deltaPower = ((int)(Difficulty) + 1) * 0.2f;
        var deltaSize = (SectorSize - Library.MIN_GLOBAL_SECTOR_SIZE) * 0.05f;
        var deltaCore = (QuestsOnStart - Library.MIN_GLOBAL_MAP_QUESTS) * 0.12f;

        // var deltaDeath = (Library.MAX_GLOBAL_MAP_DEATHSTART - StepsBeforeDeath) * 0.08f;
        var powerPerTurn = (PowerPerTurn - Library.MIN_GLOBAL_MAP_ADDITIONAL_POWER) * 0.1f;

        return deltaCore + deltaSize + deltaPower + powerPerTurn;
    }
    public List<StartShipPilotData> CreateStartArmySimpleTutor(Player player)
    {
        ArmyCreatorLogs logs = new ArmyCreatorLogs();
        float r = 1000;
        int simpleIndex;
        var bShip = ArmyCreator.CreateBaseShip(new ArmyRemainPoints(r), ShipConfig.mercenary, player);
        if (bShip.Ship.GetFreeSpellSlot(out simpleIndex))
        {
            var m1 = Library.CreateSpell(SpellType.machineGun);
            bShip.Ship.TryAddSpellModul(m1, simpleIndex);
        }
        List<StartShipPilotData> army = new List<StartShipPilotData>();
        army.Add(bShip);
        return army;
    }

    public List<StartShipPilotData> CreateStartArmyAdvTutor(Player player)
    {
        ArmyCreatorLogs logs = new ArmyCreatorLogs();
        float r = 1000;
        int simpleIndex;
        var bShip = ArmyCreator.CreateBaseShip(new ArmyRemainPoints(r), ShipConfig.federation, player);
        var battleShip = ArmyCreator.CreateShipByConfig(new ArmyRemainPoints(r), ShipConfig.federation, player, logs);
        battleShip.Ship.RemoveItem(battleShip.Ship.CocpitSlot);
        battleShip.Ship.RemoveItem(battleShip.Ship.EngineSlot);
        battleShip.Ship.RemoveItem(battleShip.Ship.WingSlot);

        if (bShip.Ship.GetFreeSpellSlot(out simpleIndex))
        {
            var m1 = Library.CreateSpell(SpellType.distShot);
            bShip.Ship.TryAddSpellModul(m1, simpleIndex);
        }
        List<StartShipPilotData> army = new List<StartShipPilotData>();
        army.Add(bShip);
        army.Add(battleShip);
        return army;

    }


    public virtual List<StartShipPilotData> CreateStartArmy(Player player)
    {
        ArmyCreatorLogs logs = new ArmyCreatorLogs();
        float r = 1000;
        var bShip = ArmyCreator.CreateBaseShip(new ArmyRemainPoints(r), shipConfig, player);
        r += Library.BASE_SHIP_VALUE;

        var listTyper = new List<ShipType>() { ShipType.Light, ShipType.Heavy, ShipType.Middle };
        var t1 = listTyper.RandomElement();
        listTyper.Remove(t1);
        var t2 = listTyper.RandomElement();
        var ship1 = ArmyCreator.CreateShipByConfig(new ArmyRemainPoints(r), t1, shipConfig, player, logs);
        r += Library.BASE_SHIP_VALUE;
        var ship2 = ArmyCreator.CreateShipByConfig(new ArmyRemainPoints(r), t2, shipConfig, player, logs);

        if (MainController.Instance.Statistics.AllTimeCollectedPoints < 15)
        {
            NewGameAddSpellsBasic(bShip);
        }
        else
        {
            NewGameAddSpellsRandom(bShip);
        }

        AddWeaponsToShips(ref r, ship1, posibleStartWeapons);
        AddWeaponsToShips(ref r, ship2, posibleStartWeapons);

        ship1.Ship.TryAddSimpleModul(Library.CreatSimpleModul(1, 2), 0);
        ship2.Ship.TryAddSimpleModul(Library.CreatSimpleModul(1, 2), 0);

        List<StartShipPilotData> army = new List<StartShipPilotData>();
        army.Add(bShip);
        army.Add(ship1);
        army.Add(ship2);
        return army;
    }

    private void NewGameAddSpellsBasic(StartShipPilotData bShip)
    {
        int simpleIndex;
        if (bShip.Ship.GetFreeSpellSlot(out simpleIndex))
        {
            var m1 = Library.CreateSpell(SpellType.rechargeShield);
            bShip.Ship.TryAddSpellModul(m1, simpleIndex);
        }
        if (bShip.Ship.GetFreeSpellSlot(out simpleIndex))
        {
            var m1 = Library.CreateSpell(SpellType.engineLock);
            bShip.Ship.TryAddSpellModul(m1, simpleIndex);
        }
        if (bShip.Ship.GetFreeSpellSlot(out simpleIndex))
        {
            var m1 = Library.CreateSpell(SpellType.machineGun);
            bShip.Ship.TryAddSpellModul(m1, simpleIndex);
        }
    }
    public static void NewGameAddSpellsRandom(StartShipPilotData bShip)
    {
        List<SpellType> healSpells = new List<SpellType>() { SpellType.rechargeShield, SpellType.repairDrones };
        List<SpellType> controlSpells = new List<SpellType>() { SpellType.engineLock, SpellType.hookShot, SpellType.throwAround, SpellType.shildDamage };
        List<SpellType> damageSpells = new List<SpellType>() { SpellType.artilleryPeriod, SpellType.distShot, SpellType.lineShot, SpellType.machineGun, SpellType.mineField };

        int simpleIndex;
        if (bShip.Ship.GetFreeSpellSlot(out simpleIndex))
        {
            var m1 = Library.CreateSpell(healSpells.RandomElement());
            bShip.Ship.TryAddSpellModul(m1, simpleIndex);
        }
        if (bShip.Ship.GetFreeSpellSlot(out simpleIndex))
        {
            var m1 = Library.CreateSpell(controlSpells.RandomElement());
            bShip.Ship.TryAddSpellModul(m1, simpleIndex);
        }
        if (bShip.Ship.GetFreeSpellSlot(out simpleIndex))
        {
            var m1 = Library.CreateSpell(damageSpells.RandomElement());
            bShip.Ship.TryAddSpellModul(m1, simpleIndex);
        }

    }

    public static void AddWeaponsToShips(ref float r, StartShipPilotData ship, List<WeaponType> list)
    {
        var ship2Weapon = list.RandomElement();
        if (list.Count > 1)
        {
            list.Remove(ship2Weapon);
        }
        int count = MyExtensions.Random(2, 4);
        for (int i = 0; i < count; i++)
        {
            ArmyCreator.TryAddWeapon(new ArmyRemainPoints(r), ship.Ship, ship2Weapon, true, new ArmyCreatorLogs());
        }
    }

    public virtual int GetStartPower()
    {

        int startPower = Library.MIN_GLOBAL_MAP_EASY_BASE_POWER;

        switch (Difficulty)
        {
            case EStartGameDifficulty.VeryEasy:
                startPower = Library.MAX_GLOBAL_MAP_VERYEASY_BASE_POWER;
                break;
            case EStartGameDifficulty.Easy:
                startPower = Library.MIN_GLOBAL_MAP_EASY_BASE_POWER;
                break;
            case EStartGameDifficulty.Normal:
                startPower = Library.MIN_GLOBAL_MAP_NORMAL_BASE_POWER;
                break;
            case EStartGameDifficulty.Hard:
                startPower = Library.MIN_GLOBAL_MAP_HARD_BASE_POWER;
                break;
            case EStartGameDifficulty.Imposilbe:
                startPower = Library.MIN_GLOBAL_MAP_IMPOSIBLE_BASE_POWER;
                break;
        }

        return startPower;
    }
}