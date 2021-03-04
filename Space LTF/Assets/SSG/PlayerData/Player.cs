
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
    public static string mainPlayer = $"myPlayerData_B.data";
    //    public static string mainPlayer = $"myPlayerData_{MainController.VERSION}.data";
    //    public int CoinsCount = 7;
    public PlayerInventory Inventory => SafeLinks.Inventory;
    public PlayerParameters Parameters => SafeLinks.Parameters;
    [System.NonSerialized]
    public PlayerScoutData ScoutData;
    public PlayerRepairData RepairData;
    public PlayerQuestData QuestData;
    public PlayerDifficultyPart Difficulty;
    public PlayerMoneyData MoneyData;
    public LastScoutsData LastScoutsData;
    public PlayerReputationData ReputationData;
    public PlayerAfterBattleOptions AfterBattleOptions;
    public PlayerMapData MapData;
//    public PlayerByStepDamage ByStepDamage;
    public PlayerMessagesToConsole MessagesToConsole;
    public PlayerSafe SafeLinks ;
    public PlayerArmy Army { get; private set; }
    public StartShipPilotData MainShip => Army.MainShip;
    private EGameMode _eGameMode;

    public string Name;

    [System.NonSerialized]
    public LastReward LastReward;

    public void PlayNewGame(StartNewGameData data)
    {
        MessagesToConsole = new PlayerMessagesToConsole();
        MapData = new PlayerMapData();
        MapData.Init(data);
        QuestData = new PlayerQuestData(this,data.QuestsOnStart);
        List<StartShipPilotData> startArmy;
        _eGameMode = data.GameNode;
        switch (_eGameMode)     
        {
            case EGameMode.simpleTutor:
                startArmy = data.CreateStartArmySimpleTutor(this);
                break;
            case EGameMode.advTutor:
                startArmy = data.CreateStartArmyAdvTutor(this);
                break;
            case EGameMode.sandBox:
            case EGameMode.champaing:
            default:
                startArmy = data.CreateStartArmy(this);
                break;
        }

        switch (data.GameNode)
        {
            case EGameMode.safePlayer:
                Difficulty = new PlayerDifficultyExprolerPart();
                break;
            default:
            case EGameMode.simpleTutor:
            case EGameMode.advTutor:
            case EGameMode.sandBox:
            case EGameMode.champaing:
                Difficulty = new PlayerDifficultyPart();
                break;
        }
        Difficulty.Init(data.Difficulty);
        Army.SetArmy(startArmy);
        RepairData.Init(Army, MapData, Parameters);
        AfterBattleOptions = new PlayerAfterBattleOptions();

        var campStart = data as StartNewGameChampaing;
        AddModuls();
        if (campStart != null)
        {
            QuestData.StartGame(_eGameMode, campStart?.Act ?? 0, campStart.ShiConfigAllise);
        }
        else
        {
            QuestData.StartGame(_eGameMode,0, ShipConfig.droid);
        }
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
        void CreateRndParameterItem()
        {

            var rnd1 = new List<EParameterItemSubType>()
                {EParameterItemSubType.Middle, EParameterItemSubType.Heavy, EParameterItemSubType.Light};

            var rnd2 = new List<EParameterItemRarity>()
            {
                EParameterItemRarity.normal, EParameterItemRarity.improved, EParameterItemRarity.perfect
            };

            if (Inventory.GetFreeSlot(out var index01, ItemType.cocpit))
            {
                var paramItem = Library.CreateParameterItem(rnd1.RandomElement(), rnd2.RandomElement());
                Inventory.TryAddItem(paramItem);
            }
        }


        if (DebugParamsController.AllSpells)
        {

            var allSpellType = (SpellType[])Enum.GetValues(typeof(SpellType));
            foreach (var type in allSpellType)
            {
                if (Inventory.GetFreeSpellSlot(out var index1))
                {
                    var modul = Library.CreateSpell(type);
                    Inventory.TryAddSpellModul(modul, index1);
                }

            }
        }

        if (DebugParamsController.AllModuls)
        {

            if (Inventory.GetFreeSlot(out var index021, ItemType.cocpit))
            {
                var paramItem = Library.CreateParameterItem(EParameterItemSubType.Heavy, EParameterItemRarity.improved, ItemType.cocpit);
                Inventory.TryAddItem(paramItem);
            }
            CreateRndParameterItem();
            CreateRndParameterItem();
            CreateRndParameterItem();
            CreateRndParameterItem();
            CreateRndParameterItem();
            CreateRndParameterItem();
            var allVals = (SimpleModulType[])Enum.GetValues(typeof(SimpleModulType));
            foreach (var type in allVals)
            {
                if (Inventory.GetFreeSimpleSlot(out var index1))
                {
                    var modul = Library.CreatSimpleModul(type, 1);
                    Inventory.TryAddSimpleModul(modul, index1);
                }
            }


            var allWeaponType = new List<WeaponType>()
            {
              WeaponType.healBodySupport,
              WeaponType.healShieldSupport
            };
            foreach (var type in allWeaponType)
            {
                if (Inventory.GetFreeWeaponSlot(out var index1))
                {
                    var modul = Library.CreateWeaponByType(type);
                    Inventory.TryAddWeaponModul(modul, index1);
                }

            }
        }
#endif

    }

    public Player(string name)
    {
        SafeLinks = new PlayerSafe(false, SaveMode.onelife);
        Army = new PlayerArmy(SafeLinks);
        MoneyData = new PlayerMoneyData(SafeLinks);
        ScoutData = new PlayerScoutData(this);
        RepairData = new PlayerRepairData();
        LastScoutsData = new LastScoutsData();
        ReputationData = new PlayerReputationData();
        ReputationData.Init();
        Name = name;
    }

    public Player(string name, PlayerSafe linkedData)
    {
        SafeLinks = linkedData;
        Army = new PlayerArmy(SafeLinks);
        MoneyData = new PlayerMoneyData(SafeLinks);
        ScoutData = new PlayerScoutData(this);
        RepairData = new PlayerRepairData();
        LastScoutsData = new LastScoutsData();
        ReputationData = new PlayerReputationData();
        ReputationData.Init();
        Name = name;
    }     
    public Player(string name, PlayerSafe linkedData,PlayerReputationData rep)
    {
        SafeLinks = linkedData;
        Army = new PlayerArmy(SafeLinks);
        MoneyData = new PlayerMoneyData(SafeLinks);
        ScoutData = new PlayerScoutData(this);
        RepairData = new PlayerRepairData();
        LastScoutsData = new LastScoutsData();
        ReputationData = rep;
        Name = name;
    }

    public void SaveOnMoveGame()
    {

        switch (SafeLinks.ShallSafeEveryMove)
        {
            case SaveMode.onelife:
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + mainPlayer);
                //        MoneyData.Dispose();
                bf.Serialize(file, this);
                file.Close();
                Debug.Log("Game Saved");
                break;
            case SaveMode.campaing:
                MainController.Instance.Campaing.SaveGame(CampaingLoader.AUTO_SAVE,true);
                break;
        }
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
            player.LoadData();
            Debug.Log("Game Loaded");
            return true;
        }
        Debug.Log("No game saved!");
        player = null;
        return false;
    }

    public void LoadData()
    {
        RepairData.Init(Army, MapData, Parameters);
        QuestData.AfterLoadCheck();
        MapData.GalaxyData.AfterLoadCheck();

    }

    public virtual ETurretBehaviour GetTurretBehaviour()
    {
        return ETurretBehaviour.nearBase;
    }
    public void WinBattleReward(Commander enemyCommander, bool winFull)
    {
        if (enemyCommander.Player is PlayerAI enemyPlayer)
        {
            LastReward = enemyPlayer.GetReward(this);
        }
        else
        {
            LastReward = new LastReward();
        }

    }


    public void EndGame()
    {
        var path = Application.persistentDataPath + mainPlayer;
        MapData.Dispose();
        if (File.Exists(path))
        {
            File.Delete(path);
        }

    }

}

