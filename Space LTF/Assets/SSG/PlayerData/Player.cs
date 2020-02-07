﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public delegate void ItemTransfer(IInventory from, IInventory to, IItemInv item);


[System.Serializable]
public class Player
{
    public static string mainPlayer = $"myPlayerData_{MainController.VERSION}.data";
    //    public int CoinsCount = 7;
    public PlayerInventory Inventory;
    public PlayerParameters Parameters;
    [System.NonSerialized]
    public PlayerScoutData ScoutData;
    public PlayerRepairData RepairData;
    public PlayerQuestData QuestData;
    public PlayerMoneyData MoneyData;
    public LastScoutsData LastScoutsData;
    public PlayerReputationData ReputationData;
    public PlayerAfterBattleOptions AfterBattleOptions;
    public PlayerMapData MapData;
    public PlayerByStepDamage ByStepDamage;
    public PlayerMessagesToConsole MessagesToConsole;
    public PlayerArmy Army { get; private set; }
    public StartShipPilotData MainShip;
    public QuestsOnStartController QuestsOnStartController;

    public string Name;

    [System.NonSerialized]
    public LastReward LastReward;

    public void PlayNewGame(StartNewGameData data)
    {
        MessagesToConsole = new PlayerMessagesToConsole();
        QuestData = new PlayerQuestData(data.CoreElementsCount);
        ByStepDamage = new PlayerByStepDamage();
        ByStepDamage.Init(data.StepsBeforeDeath, this);
        MapData = new PlayerMapData();
        MapData.Init(data, ByStepDamage);
        MapData.GalaxyData.GalaxyEnemiesArmyController.InitQuests(QuestData);
        Army.SetArmy(CreateStartArmy(data.shipConfig, data.posibleStartWeapons, data.posibleSpell));
        RepairData.Init(Army, MapData, Parameters);
        AfterBattleOptions = new PlayerAfterBattleOptions();
        ReputationData.AddReputation(data.shipConfig, Library.START_REPUTATION);
        var mid1 = (Library.MIN_GLOBAL_SECTOR_SIZE + Library.MAX_GLOBAL_SECTOR_SIZE) * .5f;
        var mid2 = (Library.MIN_GLOBAL_MAP_SECTOR_COUNT + Library.MAX_GLOBAL_MAP_SECTOR_COUNT) * .5f;

        var midSize = mid1 * mid2;

        var size = ((float)(MapData.GalaxyData.SizeOfSector * MapData.GalaxyData.AllSectors.Count / 3f));
        var coef = size / midSize;
        QuestsOnStartController = new QuestsOnStartController(coef);
        QuestsOnStartController.InitQuests();
        AddModuls();
    }

    private void AddModuls()
    {
        //        int index;
        //        if (Inventory.GetFreeSimpleSlot(out index))
        //        {
        //            var modul = Library.CreatSimpleModul(SimpleModulType.fireMines,1);
        //            Inventory.TryAddSimpleModul(modul,index);
        //        }
        //        if (Inventory.GetFreeSimpleSlot(out index))
        //        {
        //            var modul = Library.CreatSimpleModul(SimpleModulType.frontShield,1);
        //            Inventory.TryAddSimpleModul(modul,index);
        //        } 
        //        if (Inventory.GetFreeSimpleSlot(out index))
        //        {
        //            var modul = Library.CreatSimpleModul(SimpleModulType.armor,1);
        //            Inventory.TryAddSimpleModul(modul,index);
        //        }

        //        for (int i = 0; i < count; i++)
        //        {
        //            if (Inventory.GetFreeSimpleSlot(out var index))
        //            {
        //                var modul = Library.CreatSimpleModul(1,MyExtensions.IsTrueEqual());
        //                Inventory.TryAddSimpleModul(modul,index);
        //            }
        //        }


#if UNITY_EDITOR

        foreach (var inventoryModul in Inventory.Moduls)
        {
            if (inventoryModul.CurrentInventory == null)
            {
                Debug.LogError($"Item with null inv {inventoryModul.Name}");
            }
        }
#endif

#if UNITY_EDITOR
        if (DebugParamsController.AllModuls)
        {

            var allVals = (SimpleModulType[])Enum.GetValues(typeof(SimpleModulType));
            foreach (var type in allVals)
            {
                if (Inventory.GetFreeSimpleSlot(out var index1))
                {
                    var modul = Library.CreatSimpleModul(type, 1);
                    Inventory.TryAddSimpleModul(modul, index1);
                }

            }

            //            for (int i = 0; i < 3; i++)
            //            {
            //                if (Inventory.GetFreeWeaponSlot(out var index2))
            //                {
            //                    var modul1 = Library.CreateWeapon(WeaponType.beam);
            //                    Inventory.TryAddWeaponModul(modul1, index2);
            //                }
            //            }

            var allSpellType = (SpellType[])Enum.GetValues(typeof(SpellType));
            foreach (var type in allSpellType)
            {
                if (type != SpellType.BaitPriorityTarget && type != SpellType.priorityTarget && Inventory.GetFreeSpellSlot(out var index1))
                {
                    var modul = Library.CreateSpell(type);
                    Inventory.TryAddSpellModul(modul, index1);
                }

            }
        }
#endif

    }

    private List<StartShipPilotData> CreateStartArmy(ShipConfig config, List<WeaponType> posibleStartWeapons,
        List<SpellType> posibleSpell)
    {
        ArmyCreatorLogs logs = new ArmyCreatorLogs();
        float r = 1000;
        var bShip = ArmyCreator.CreateBaseShip(new ArmyRemainPoints(r), config, this);
        r += Library.BASE_SHIP_VALUE;
        var ship1 = ArmyCreator.CreateShipByConfig(new ArmyRemainPoints(r), config, this, logs);
        r += Library.BASE_SHIP_VALUE;
        var ship2 = ArmyCreator.CreateShipByConfig(new ArmyRemainPoints(r), config, this, logs);

        int simpleIndex;
//        foreach (var spellType in posibleSpell)
//        {
//            if (bShip.Ship.GetFreeSpellSlot(out simpleIndex))
//            {
//                var m1 = Library.CreateSpell(spellType);
//                bShip.Ship.TryAddSpellModul(m1, simpleIndex);
//            }
//        }
        List<SpellType> healSpells = new List<SpellType>(){SpellType.rechargeShield,SpellType.repairDrones};
        List<SpellType> controlSpells = new List<SpellType>(){SpellType.engineLock,SpellType.vacuum,SpellType.throwAround,SpellType.shildDamage};
        List<SpellType> damageSpells = new List<SpellType>(){SpellType.artilleryPeriod,SpellType.distShot,SpellType.lineShot,SpellType.machineGun,SpellType.mineField};

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

        AddWeaponsToShips(ref r, ship1, posibleStartWeapons);
        AddWeaponsToShips(ref r, ship2, posibleStartWeapons);

        ship1.Ship.TryAddSimpleModul(Library.CreatSimpleModul(1, 2), 0);
        ship2.Ship.TryAddSimpleModul(Library.CreatSimpleModul(1, 2), 0);

        List<StartShipPilotData> army = new List<StartShipPilotData>();
        army.Add(bShip);
        army.Add(ship1);
        army.Add(ship2);
        MainShip = bShip;
        return army;
    }

    private void AddWeaponsToShips(ref float r, StartShipPilotData ship, List<WeaponType> list)
    {
        var ship2Weapon = list.RandomElement();
        int count = MyExtensions.Random(2, 4);
        for (int i = 0; i < count; i++)
        {
            ArmyCreator.TryAddWeapon(new ArmyRemainPoints(r), ship.Ship, ship2Weapon, true, new ArmyCreatorLogs());
        }
    }

    public Player(string name, Dictionary<PlayerParameterType, int> startData = null)
    {
        Army = new PlayerArmy();
        MoneyData = new PlayerMoneyData();
        ScoutData = new PlayerScoutData(this);
        Parameters = new PlayerParameters(this, startData);
        Inventory = new PlayerInventory(this);
        RepairData = new PlayerRepairData();
        LastScoutsData = new LastScoutsData();
        ReputationData = new PlayerReputationData();
        //        MapData  = new PlayerMapData();
        //        MapData.Init();
        Name = name;
    }




    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + mainPlayer);
        //        MoneyData.Dispose();
        bf.Serialize(file, this);
        file.Close();
        Debug.Log("Game Saved");
    }


    public static bool LoadGame(out Player player)
    {
        if (File.Exists(Application.persistentDataPath + mainPlayer))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + mainPlayer, FileMode.Open);
            Player save = (Player)bf.Deserialize(file);
            file.Close();
            player = save;
            Debug.Log("Game Loaded");
            return true;
        }
        Debug.Log("No game saved!");
        player = null;
        return false;
    }





    public void WinBattleReward(Commander enemyCommander)
    {
        LastReward = new LastReward(enemyCommander,this);
    }


    public void EndGame()
    {
        var path = Application.persistentDataPath + mainPlayer;
        MapData.Dispose();
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        if (QuestsOnStartController != null)
        {
            QuestsOnStartController.DisposeQuests();
        }
    }
}

