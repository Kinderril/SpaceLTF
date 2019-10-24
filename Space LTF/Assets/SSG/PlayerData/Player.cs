
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public delegate void ItemTransfer(IInventory from, IInventory to, IItemInv item);


[System.Serializable]
public class Player
{
    public const int MAX_ARMY = 5;
    public const string mainPlayer = "myPlayerData.data";
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
    public List<StartShipPilotData> Army = new List<StartShipPilotData>();
    public StartShipPilotData MainShip;

    [field: NonSerialized]
    public event Action<StartShipPilotData, bool> OnAddShip; 
    public string Name;

    [System.NonSerialized]
    public LastReward LastReward;
    
    public void PlayNewGame(StartNewGameData data)
    {
        MessagesToConsole = new PlayerMessagesToConsole();
        QuestData  = new PlayerQuestData(data.CoreElementsCount);
        ByStepDamage = new PlayerByStepDamage();
        ByStepDamage.Init(data.StepsBeforeDeath,this);
        MapData = new PlayerMapData();
        MapData.Init(data,ByStepDamage);
        Army = CreateStartArmy(data.shipConfig, data.posibleStartWeapons, data.posibleSpell);
        RepairData.Init(Army, MapData,Parameters);
        AfterBattleOptions = new PlayerAfterBattleOptions();
        AddModuls(1);
    }

    private void AddModuls(int count)
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

            var allVals = (SimpleModulType[]) Enum.GetValues(typeof(SimpleModulType));
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

            var allSpellType = (SpellType[]) Enum.GetValues(typeof(SpellType));
            foreach (var type in allSpellType)
            {
                if (type  != SpellType.BaitPriorityTarget && type != SpellType.priorityTarget &&  Inventory.GetFreeSpellSlot(out var index1))
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
        foreach (var spellType in posibleSpell)
        {
            if (bShip.Ship.GetFreeSpellSlot(out simpleIndex))
            {
                var m1 = Library.CreateSpell(spellType);
                bShip.Ship.TryAddSpellModul(m1, simpleIndex);
            }
        }
        AddWeaponsToShips(ref r, ship1, posibleStartWeapons);
        AddWeaponsToShips(ref r, ship2, posibleStartWeapons);
        
        ship1.Ship.TryAddSimpleModul(Library.CreatSimpleModul(1,false), 0);
        ship2.Ship.TryAddSimpleModul(Library.CreatSimpleModul(1, false), 0);

        List<StartShipPilotData> army = new List<StartShipPilotData>();
        army.Add(bShip);
        army.Add(ship1);
        army.Add(ship2);
        MainShip = bShip;
        return army;
    }

    private void AddWeaponsToShips(ref float r,StartShipPilotData ship,List<WeaponType> list)
    {
        var ship2Weapon = list.RandomElement();
        int count = MyExtensions.Random(2, 4);
        for (int i = 0; i < count; i++)
        {
            ArmyCreator.TryAddWeapon(new ArmyRemainPoints(r), ship.Ship, ship2Weapon, true,new ArmyCreatorLogs());
        }
    }

    public Player(string name,  Dictionary<PlayerParameterType, int> startData = null)
    {
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


    public bool CanAddShip()
    {
        return Army.Count < MAX_ARMY;
    }

    public bool TryHireShip(StartShipPilotData ship)
    {
        if (CanAddShip())
        {
            Army.Add(ship);
            if (OnAddShip != null)
            {
                OnAddShip(ship, true);
            }
            return true;
        }
        return false;
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



    public List<StartShipPilotData> GetShipsToBattle()
    {
        List<StartShipPilotData> list = new List<StartShipPilotData>();
        foreach (var data in Army)
        {
//            if (!data.Ship.Destroyed)
//            {
                list.Add(data);
//            }
        }

        return list;

    }


    public void WinBattleReward(Commander enemyCommander)
    {
        var power = enemyCommander.StartPower;
        var scoutsLevel = Parameters.Scouts.Level;
        WDictionary<BattleRewardType> rewardRnd =
            new WDictionary<BattleRewardType>(new Dictionary<BattleRewardType, float>()
            {
                {BattleRewardType.weapon, scoutsLevel},
                {BattleRewardType.modul, scoutsLevel},
                {BattleRewardType.money, 4 + scoutsLevel/4},
            });
        var reward = rewardRnd.Random();
        int slotIndex;
        float moneyCoef = 1f;
        Debug.Log("Player end battle. Reward setted.  reward:"+ reward.ToString());
        LastReward = new LastReward();
        switch (reward)
        {
            case BattleRewardType.money:
                moneyCoef = 1;
                break;
            case BattleRewardType.weapon:
                moneyCoef = 0.7f;
                var w = Library.CreateWeapon(power < 45);
                if (Inventory.GetFreeWeaponSlot(out slotIndex))
                {
                    Inventory.TryAddWeaponModul(w, slotIndex);
                    LastReward.Weapons.Add(w);
                }
                break;
            case BattleRewardType.modul:
                var isWeak = power < 45;
                moneyCoef = 0.7f;
                WDictionary<int> levels = new WDictionary<int>(new Dictionary<int, float>()
                {
                    {1,isWeak?5f:2f },
                    {2,isWeak?3f:4f},
                    {3,isWeak?0f:2f},
                });
                var m = Library.CreatSimpleModul(levels.Random(),MyExtensions.IsTrueEqual());
                if (Inventory.GetFreeSpellSlot(out slotIndex))
                {
                    LastReward.Moduls.Add(m);
                    Inventory.TryAddSimpleModul(m, slotIndex);
                }
                break;
        }
        int moneyToReward = (int) (moneyCoef*power);
        moneyToReward = Library.ModificationMoneyBattleReward(moneyToReward);
        LastReward.Money = moneyToReward;
        MoneyData.AddMoney(moneyToReward);
    }


    public void EndGame()
    {
        var path = Application.persistentDataPath + mainPlayer;
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public void RemoveShip(ShipInventory shipInventory)
    {
        var shipTo = Army.FirstOrDefault(x => x.Ship == shipInventory);
        if (shipTo == null)
        {
            Debug.LogError("can't find ship to destroy");
            return;
        }

        RemoveShip(shipTo);
    }

    public void RemoveShip(StartShipPilotData shipToDel)
    {

        Army.Remove(shipToDel);
        if (OnAddShip != null)
        {
            OnAddShip(shipToDel, false);
        }

    }
}

