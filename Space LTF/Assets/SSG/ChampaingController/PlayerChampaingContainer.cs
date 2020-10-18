using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;

[System.Serializable]
public class PlayerChampaingContainer
{
    public PlayerCampaing Player { get; private set; }
    public int Act { get; private set; }
    private ShipConfig _config = ShipConfig.mercenary;
    private EStartGameDifficulty _difficulty = EStartGameDifficulty.Normal;
    private PlayerReputationData _reputationData;
    private PlayerSafe _safe;
    private const int LAST_ACT = 3;

    public void StartNewGame(ShipConfig config,EStartGameDifficulty difficulty)
    {
        _config = config;
        _difficulty = difficulty;
        Act = 0;
    }

    public void SaveTo(string path)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, this);
        file.Close();
        Debug.Log($"Game Saved PlayerChampaing path:{path}");
        WindowManager.Instance.InfoWindow.Init(null,  Namings.Tag("GameSaved"));
    }

    public static bool LoadGame( string path, out PlayerChampaingContainer player)
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            var save = (PlayerChampaingContainer)bf.Deserialize(file);
            file.Close();
            player = save;
            player.LoadData();
            Debug.Log("Game Loaded PlayerChampaing");
            return true;
        }
        Debug.Log("No game saved!");
        player = null;
        return false;
    }

    private void LoadData()
    {
        Player.LoadData();
    }

    [CanBeNull]
    public MessageDialogData Dialog()
    {
        if (Act == 0)
        {
            return StartDialog();
        }

        if (Act == 3)                                                                                        
        {
            return EndDialog(_config);
        }

        return null;
    }

    private MessageDialogData EndDialog(ShipConfig config)
    {
        switch (config)
        {
            case ShipConfig.raiders:
                break;
            case ShipConfig.federation:
                break;
            case ShipConfig.mercenary:
                return DialogsLibrary.GetPairDialogByTag(GetDialogsTagMerc(), DialogEnds);
            case ShipConfig.ocrons:
                break;
            case ShipConfig.krios:
                break;
        }

        DialogEnds();
        return null;
    }

    private void DialogEnds()
    {
        WindowManager.Instance.OpenWindow(MainState.start);
    }

    private List<string> GetDialogsTagMerc()
    {
        var list = new List<string>();
        list.Add("cmMerc_dialog_final_M1"); 
        list.Add("cmMerc_dialog_final_A1"); 
        list.Add("cmMerc_dialog_final_M2"); 
        list.Add("cmMerc_dialog_final_A2"); 
        list.Add("cmMerc_dialog_final_M3"); 
        list.Add("cmMerc_dialog_final_A3"); 
        list.Add("cmMerc_dialog_final_M4"); 
        list.Add("DC");
        return list;
    }

    private MessageDialogData StartDialog()
    {

        var mianAnswers = new List<AnswerDialogData>();
        foreach (ShipConfig value in Enum.GetValues(typeof(ShipConfig)))
        {
            if (value != ShipConfig.droid)
                mianAnswers.Add(new AnswerDialogData(Namings.ShipConfig(value), null, () => ChooseConfig(value)));
        }

        MessageDialogData data = new MessageDialogData(Namings.Tag("ChooseStartShip"), mianAnswers);
        return data;
    }

    private MessageDialogData ChooseConfig(ShipConfig value)
    {
        var desc = Namings.Tag($"chooseOptionDescStart_{value}");
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.Tag("Ok"),null,()=>StartGameChooseDiff(value)));
        mianAnswers.Add(new AnswerDialogData(Namings.Tag("No"),null, StartDialog));

        MessageDialogData data = new MessageDialogData(desc, mianAnswers);
        return data;
    }

    private MessageDialogData StartGameChooseDiff(ShipConfig config)
    {

        var mianAnswers = new List<AnswerDialogData>();
        foreach (EStartGameDifficulty value in Enum.GetValues(typeof(EStartGameDifficulty)))
        {
            mianAnswers.Add(new AnswerDialogData(Namings.Tag(value.ToString()), null, () => chooseDiff(value, config)));
        }

        MessageDialogData data = new MessageDialogData(Namings.Tag("ChooseStartDiff"), mianAnswers);
        return data;

    }

    private MessageDialogData chooseDiff(EStartGameDifficulty value, ShipConfig config)
    {

        var desc = Namings.Tag($"chooseOptionDescStart_{value}");
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.Tag("Ok"), ()=>StartGame(value, config)));
        mianAnswers.Add(new AnswerDialogData(Namings.Tag("No"), null,()=> StartGameChooseDiff(config)));

        MessageDialogData data = new MessageDialogData(desc, mianAnswers);
        return data;
    }

    private void StartGame(EStartGameDifficulty value, ShipConfig config)
    {
        StartNewGame(config,value);
    }

    public string GetActDesc( )
    {
        if (Act == 0)
        {
            return Namings.Tag("Start akt");
        }

        if (_reputationData.Allies.HasValue)
        {
            return Namings.Tag($"Dairy_{_reputationData.Allies.ToString()}_{Act}");
        }
        return "No text";

    }

    public void EndAct()
    {
        Act++;
        _safe = Player.SafeLinks;
        _safe.ClearEvents();
        _reputationData = Player.ReputationData;
        _reputationData.ClearEvents();
//        if (Act > 3)
//        {
//            EndCampaingScreen();
//        }
//        else
//        {
//        }
        WindowManager.Instance.OpenWindow(MainState.startNewChampaing, this);
    }

    private void EndCampaingScreen()
    {
        
        Debug.LogError("TODO END CAMPAING SCREEN");
    }


    public void PlayNextAct()
    {
        if (Act >= LAST_ACT)
        {
            EndCampaingScreen();
            return;
        }

        var posibleStartSpells = new List<SpellType>()
        {
            SpellType.lineShot,
            SpellType.engineLock,
            SpellType.shildDamage,
            SpellType.mineField,
            SpellType.throwAround,
            SpellType.distShot,
            SpellType.artilleryPeriod,
            SpellType.repairDrones,
            SpellType.vacuum,
            SpellType.hookShot,
//            SpellType.spaceWall,
        };
        var posibleSpells = posibleStartSpells.RandomElement(2);
        if (_safe == null)
        {
            _safe = new PlayerSafe(false,false);
        }

        if (_reputationData == null)
        {
            _reputationData = new PlayerReputationData();
            _reputationData.Init();
        }
        Player = new PlayerCampaing($"Campaing player {Act}", _safe,_reputationData);
        Debug.Log($"Play act {Act}");
        int sizeSector = 5;
        int powerPerTurn = 0;
        switch (Act)
        {
            case 0:
                sizeSector = 4;
                powerPerTurn = 0;
                break;
            case 1:
                sizeSector = 5;
                powerPerTurn = 1;
                break;
            case 2:
                sizeSector = 6;
                powerPerTurn = 2;
                break;
//            case 3:
//                sizeSector = 5;
//                powerPerTurn = 3;
//                break;
        }

        ShipConfig allies = _config;
        if (_reputationData.Allies.HasValue)
        {
            allies = _reputationData.Allies.Value;
        }
        else
        {
            if (Act != 0)
            {
                Debug.LogError("No allies when act is not 0");
#if UNITY_EDITOR
                Debug.LogError("Set extra reputation");
                _reputationData.SetAllies(ShipConfig.mercenary);
#endif
            }
        }

        var data = new StartNewGameChampaing(Player, new Dictionary<PlayerParameterType, int>(), _config,
            new List<WeaponType>(), sizeSector, 0, _difficulty, posibleSpells, powerPerTurn, Act, allies);
        MainController.Instance.CreateNewPlayerAndStartGame(data);
    }

    public void DebugSetAct(int act)
    {
        Act = act;
    }
}
