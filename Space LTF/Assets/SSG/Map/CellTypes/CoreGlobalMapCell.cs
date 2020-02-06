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
    public bool _diplomacyFail = false;

    private Player _cachedArmy = null;

    private PlayerQuestData Quest => MainController.Instance.MainPlayer.QuestData;


    public CoreGlobalMapCell(int power, int id, int intX, int intZ, SectorData sector)
        : base(power, ShipConfig.mercenary, id, ArmyCreatorType.destroy, intX, intZ, sector)
    {
        _power = power;
        //        Debug.LogError($"CoreGlobalMapCell:{intX}  {intZ}");
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

    protected override MessageDialogData GetDialog()
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
        answerDialog.Add(new AnswerDialogData(Namings.DialogTag("coreAttack"), Fight));
        var mesData = new MessageDialogData(Namings.DialogTag("coreTargetProtect"), answerDialog);
        return mesData;
    }

    private MessageDialogData GetSimlpleDialog()
    {
        List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
        answerDialog.Add(new AnswerDialogData(Namings.DialogTag("coreAttack"), Fight));
        var mesData = new MessageDialogData(Namings.DialogTag("coreTargetProtect"), answerDialog);
        return mesData;
    }

    private int MoneyToBuy()
    {
        return Power * 3;
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
        var isFriend = player.ReputationData.IsFriend(ConfigOwner);
        if (isFriend)
        {
            if (!_diplomacyFail)
                answerDialog.Add(new AnswerDialogData(Namings.TryFormat(Namings.DialogTag("coreUseDiplomacy")), null, Diplomaty));
            if (player.MoneyData.HaveMoney(MoneyToBuy()))
                answerDialog.Add(new AnswerDialogData(Namings.TryFormat(Namings.DialogTag("coreBuy"), MoneyToBuy()), null, Buy));
        }
        //        if (player.Parameters.Scouts.Level >= 1)
        answerDialog.Add(new AnswerDialogData(Namings.TryFormat(Namings.DialogTag("coreSendScouts")), null, Steal));
        var msg = isFriend
            ? Namings.TryFormat(Namings.DialogTag("coreFleetHaveFriend"), scoutsField)
            : Namings.TryFormat(Namings.DialogTag("coreFleetHave"), scoutsField);
        var mesData = new MessageDialogData(msg, answerDialog);
        return mesData;
    }

    private MessageDialogData Buy()
    {

        MainController.Instance.MainPlayer.MoneyData.RemoveMoney(MoneyToBuy());
        SetTake();
        MainController.Instance.MainPlayer.QuestData.AddElement();
        MainController.Instance.MainPlayer.ReputationData.AddReputation(ConfigOwner, 10);
        List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
        answerDialog.Add(new AnswerDialogData(Namings.DialogTag("Ok")));
        var mesData = new MessageDialogData(Namings.TryFormat(Namings.DialogTag("coreWasPurchase"), Quest.mainElementsFound, Quest.MaxMainElements), answerDialog);
        return mesData;
    }

    private MessageDialogData Diplomaty()
    {
        List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
        var player = MainController.Instance.MainPlayer;
        var val = player.ReputationData.ReputationFaction[ConfigOwner];
        if (SkillWork(100 - val, val))
        {
            _diplomacyFail = true;
            answerDialog.Add(new AnswerDialogData(Namings.DialogTag("Ok"), null, GetTalkDialog));
            var mesData = new MessageDialogData(Namings.TryFormat(Namings.DialogTag("coreDiplomacyFail")), answerDialog);
            return mesData;
        }
        else
        {
            SetTake();
            MainController.Instance.MainPlayer.QuestData.AddElement();
            answerDialog.Add(new AnswerDialogData(Namings.DialogTag("Ok")));
            var mesData = new MessageDialogData(Namings.TryFormat(Namings.DialogTag("coreElementYour"), Quest.mainElementsFound, Quest.MaxMainElements), answerDialog);
            return mesData;
        }
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
            RemoveDimplomacyByForce();
            MainController.Instance.MainPlayer.QuestData.AddElement();
            List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
            answerDialog.Add(new AnswerDialogData(Namings.DialogTag("Ok")));
            var mesData = new MessageDialogData(Namings.TryFormat(Namings.DialogTag("coreElementYour"), Quest.mainElementsFound, Quest.MaxMainElements), answerDialog);
            return mesData;
        }
        else
        {
            _power = (int)(Power * 1.26f);
            List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
            answerDialog.Add(new AnswerDialogData(Namings.DialogTag("coreFight"), Fight));
            var mesData = new MessageDialogData(Namings.DialogTag("coreFailSteal"), answerDialog);
            return mesData;
        }
    }

    private void RemoveDimplomacyByForce()
    {
        var rep = MainController.Instance.MainPlayer.ReputationData;
        var isFriend = rep.IsFriend(ConfigOwner);
        if (isFriend)
        {
            rep.RemoveReputation(ConfigOwner, 15);
        }
        else
        {
            rep.RemoveReputation(ConfigOwner, 7);
        }
    }

    protected override MessageDialogData GetLeavedActionInner()
    {
        if (_afterFightActivated)
        {
            MainController.Instance.MainPlayer.QuestData.AddElement();
            List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
            answerDialog.Add(new AnswerDialogData(Namings.DialogTag("ok"), null, null));
            var mesData = new MessageDialogData(Namings.DialogTag("corePartYours"), answerDialog);
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
        RemoveDimplomacyByForce();
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer, GetArmy(), false);
    }

    protected override Player GetArmy()
    {
        if (_cachedArmy != null)
        {
            return _cachedArmy;
        }

        List<Func<float, List<StartShipPilotData>>> posibleArmies = new List<Func<float, List<StartShipPilotData>>>();
        switch (ConfigOwner)
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
            var army = rnd(Power);
            var player = new Player("boss");
            player.Army.SetArmy(army);
            _cachedArmy = player;
        }

        _cachedArmy = base.GetArmy();
        return _cachedArmy;
    }

    public override Color Color()
    {
        return new Color(204f / 255f, 255f / 255f, 153f / 255f);
    }

    public override bool OneTimeUsed()
    {
        return true;
    }
}