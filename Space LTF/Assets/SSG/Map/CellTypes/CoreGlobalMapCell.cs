using System;
using System.Collections.Generic;
using UnityEngine;

public enum GlobalCellType
{
    simple,
    army,
    talk,
}

[Serializable]
public class CoreGlobalMapCell : ArmyGlobalMapCell
{
    public GlobalCellType CellType;
    private bool _afterFightActivated = false;
//    public bool HaveInfo = false;
    public bool Taken = false;

    private Player _cachedArmy = null;

    private PlayerQuestData Quest => MainController.Instance.MainPlayer.QuestData;


    public CoreGlobalMapCell(int power, int id, int intX, int intZ,SectorData  secto) 
        : base( power,ShipConfig.mercenary, id, ArmyCreatorType.destroy, intX, intZ, secto)
    {
        _power = power;
        WDictionary<GlobalCellType> chances = new WDictionary<GlobalCellType>(new Dictionary<GlobalCellType, float>()
        {
            { GlobalCellType.simple, 2},
            { GlobalCellType.army, 4},
            { GlobalCellType.talk, 5},
        });
        WDictionary<ArmyCreatorType> armyTypes = new WDictionary<ArmyCreatorType>(new Dictionary<ArmyCreatorType, float>()
        {
            { ArmyCreatorType.simple, 2},
            { ArmyCreatorType.laser, 4},
            { ArmyCreatorType.destroy, 2},
            { ArmyCreatorType.rocket, 2},
        });
        _armyType = armyTypes.Random();
        CellType = chances.Random();
    }

    public override void OpenInfo()
    {
        base.OpenInfo();
//        HaveInfo = true;
    }

    public override string Desc()
    {
        if (Taken)
        {
            return "Retranslaitor (Taken)";
        }
        return "Retranslaitor";
    }

    public override void Take()
    {

    }

    public override MessageDialogData GetDialog()
    {
        switch (CellType)
        {
            case GlobalCellType.army:
                return GetArmyDialog();
            case GlobalCellType.talk:
                return GetTalkDialog();
        }
        return GetSimlpleDialog();
    }


    private MessageDialogData GetArmyDialog()
    {
        List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
        answerDialog.Add(new AnswerDialogData("Attack.", Fight));
        var mesData = new MessageDialogData("Target is under protection.", answerDialog);
        return mesData;
    }

    private MessageDialogData GetSimlpleDialog()
    {
        List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
        answerDialog.Add(new AnswerDialogData("Attack.", Fight));
        var mesData = new MessageDialogData("Target is under protection.", answerDialog);
        return mesData;
    }

    private int MoneyToBuy()
    {
        return _power*3;
    }

    private MessageDialogData GetTalkDialog()
    {
        var player = MainController.Instance.MainPlayer;
        List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();

        var scoutData = GetArmy().ScoutData.GetInfo(player.Parameters.Scouts.Level);
        string scoutsField = "";
        for (int i = 0; i < scoutData.Count; i++)
        {
            var info = scoutData[i];
            scoutsField = $"{scoutsField}\n{info}\n";
        }

        answerDialog.Add(new AnswerDialogData("Attack.", Fight));
        if (player.MoneyData.HaveMoney(MoneyToBuy()))
            answerDialog.Add(new AnswerDialogData(String.Format("Buy for {0} credits.", MoneyToBuy()), null,Buy));
        if (player.Parameters.Diplomaty.Level >= 3)
            answerDialog.Add(new AnswerDialogData(String.Format("Use diplomacy."),null, Diplomaty));
//        if (player.Parameters.Scouts.Level >= 1)
        answerDialog.Add(new AnswerDialogData(String.Format("Send scouts to steal."), null,Steal));
        var mesData = new MessageDialogData($"Some other fleet already have your target. \n {scoutsField}", answerDialog);
        return mesData;
    }

    private MessageDialogData Buy()
    {
        
        MainController.Instance.MainPlayer.MoneyData.RemoveMoney(MoneyToBuy());
        SetTake();
        MainController.Instance.MainPlayer.QuestData.AddElement();
        List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
        answerDialog.Add(new AnswerDialogData("Ok."));
        var mesData = new MessageDialogData(String.Format("Element was purchased. {0}/{1}", Quest.mainElementsFound, Quest.MaxMainElements), answerDialog);
        return mesData;
    }

    private MessageDialogData Diplomaty()
    {
        SetTake();
        MainController.Instance.MainPlayer.QuestData.AddElement();
        MainController.Instance.MainPlayer.QuestData.AddElement();
        List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
        answerDialog.Add(new AnswerDialogData("Ok."));
        var mesData = new MessageDialogData(String.Format("Element is yours. {0}/{1}", Quest.mainElementsFound, Quest.MaxMainElements), answerDialog);
        return mesData;
    }

    private void SetTake()
    {
        Taken = true;
    }

    private MessageDialogData Steal()
    {
        WDictionary<bool> wd = new WDictionary<bool>(new Dictionary<bool, float>()
        {
            { true,MainController.Instance.MainPlayer.Parameters.Scouts.Level},
            { false,3.5f},
        });
        if (wd.Random())
        {

            SetTake();
            MainController.Instance.MainPlayer.QuestData.AddElement();
            List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
            answerDialog.Add(new AnswerDialogData("Ok."));
            var mesData = new MessageDialogData(String.Format("Element is yours. {0}/{1}", Quest.mainElementsFound, Quest.MaxMainElements), answerDialog);
            return mesData;
        }
        else
        {
            _power = (int)(_power * 1.26f);
            List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
            answerDialog.Add(new AnswerDialogData("Very bad. Fight.", Fight));
            var mesData = new MessageDialogData("Fail. While you were trying to steal an item, reinforcements came to them, and now you can't runaway.", answerDialog);
            return mesData;
        }
    }

    protected override MessageDialogData GetLeavedActionInner()
    {
        if (_afterFightActivated)
        {
            MainController.Instance.MainPlayer.QuestData.AddElement();
            List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
            answerDialog.Add(new AnswerDialogData("Ok.", null, null));
            var mesData = new MessageDialogData("This part is yours now.", answerDialog);
            return mesData;
        }
        else
        {
            return base.GetLeavedActionInner();
        }
    }

    private void Fight()
    {
        _afterFightActivated = true;
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer, GetArmy(),false);
    }

    protected override Player GetArmy()
    {
        if (_cachedArmy != null)
        {
            return _cachedArmy;
        }

        List<Func<float ,List<StartShipPilotData>> > posibleArmies = new List<Func<float, List<StartShipPilotData>>>();
        switch (_config)
        {
            case ShipConfig.raiders:
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossBeamDistRaiders);
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossLightMinesRaiders);
                break;
            case ShipConfig.federation:
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossEngineLockersFederation);
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossImpulseCritsFed);
                break;
            case ShipConfig.mercenary:
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossCassetSprayMerc);
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossManyEMIMerc);
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossEMIFireMerc);
                break;
            case ShipConfig.ocrons:
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossHeavyWithSpeedOcrons);
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossMaxSelfDamageOcrons);
                break;
            case ShipConfig.krios:
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossIgnoreShieldKrios);
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossRocketTurnDistKrios);
                break;
            case ShipConfig.droid:
                break;
        }

        if (posibleArmies.Count > 0)
        {
            var rnd = posibleArmies.RandomElement();
            var army = rnd(_power);
            var player = new Player("boss");
            player.Army = army;
            _cachedArmy =  player;
        }

        _cachedArmy = base.GetArmy();
        return _cachedArmy;
    }

    public override Color Color()
    {
        return new Color(204f/255f, 255f/255f, 153f/255f);
    }

    public override bool OneTimeUsed()
    {
        return true;
    }
}