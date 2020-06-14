using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SecretDeal : BaseGlobalMapEvent
{
    private float fullCoef = 1.4f;
    private float halfCoef = 0.7f;

    private ShipConfig _secondSide;
    public SecretDeal(int power, ShipConfig config)
        : base(config)
    {
        _power = power;
        List<ShipConfig> secondConfigs = new List<ShipConfig>()
        {
            ShipConfig.federation,
            ShipConfig.ocrons,
            ShipConfig.mercenary,
            ShipConfig.raiders,
            ShipConfig.krios,
        };
        secondConfigs.Remove(config);
        _secondSide = config;

    }
    public override string Desc()
    {
        return "Quiet place";
    }

    public override MessageDialogData GetDialog()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretSendScouts"), null, scoutsSend));
        mianAnswers.Add(new AnswerDialogData(Namings.Tag("leave"), null));
        var mesData =
            new MessageDialogData(Namings.DialogTag("secretStart"),
                mianAnswers);
        return mesData;
    }

    private MessageDialogData scoutsSend()
    {
        var mianAnswers = new List<AnswerDialogData>();
        if (SkillWork(3, ScoutsLevel))
        {
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretHideWait"), null, hideAndWait));
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretSendFake"), null, fakeSignal));
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretAttack"), () => Fight(fullCoef, true)));
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretConnectWith"), null, connectWithCommander));
            var mesData =
                new MessageDialogData(
                    Namings.Format(Namings.DialogTag("secretScoutResult"), ScoutsLevel),
                    mianAnswers);
            return mesData;
        }
        else
        {
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretTryPrevent"), null, preventConflict));
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretRun"), null, tryRun));
            var mesData =
                new MessageDialogData(
                    Namings.Format(Namings.DialogTag("secretSeeHugeArmy"), ScoutsLevel),
                    mianAnswers);
            return mesData;
        }
    }

    private MessageDialogData tryRunWithAll()
    {
        var player = MainController.Instance.MainPlayer;
        var coef = (float)_power * Library.MONEY_QUEST_COEF;
        var money = (int)(MyExtensions.Random(24, 47) * coef * player.SafeLinks.CreditsCoef);
        player.MoneyData.AddMoney(money);
        var m = Library.CreatSimpleModul(2, 4);
        var itemName = Namings.SimpleModulName(m.Type);
        var canAdd = MainController.Instance.MainPlayer.Inventory.GetFreeSimpleSlot(out var slot);
        if (canAdd)
        {
            player.Inventory.TryAddSimpleModul(m, slot);
        }

        return tryRunWithCoef(-1, Namings.Format(Namings.DialogTag("secretHaveCredits"), money, itemName));
    }

    private MessageDialogData tryRun()
    {
        return tryRunWithCoef(0, "");
    }

    private MessageDialogData tryRunWithCoef(int coef, string add)
    {
        var mianAnswers = new List<AnswerDialogData>();
        var absCoef = Mathf.Abs(coef);
        if (absCoef > 0)
        {
            _reputation.RemoveReputation(_config, 10 * absCoef);
            _reputation.RemoveReputation(_secondSide, 10 * absCoef);
        }

        if (SkillWork(2, ScoutsLevel + coef))
        {
            mianAnswers.Add(new AnswerDialogData(Namings.Tag("Ok"), null, null));
            var mesData = new MessageDialogData(Namings.Format(Namings.DialogTag("secretYouRun"), add, ScoutsLevel), mianAnswers);
            return mesData;
        }
        else
        {
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretFight"), () => Fight(fullCoef, false), null));
            var mesData = new MessageDialogData(Namings.Format(Namings.DialogTag("secretRunFail"), ScoutsLevel), mianAnswers);
            return mesData;
        }
    }

    private MessageDialogData preventConflict()
    {
        return TalkTToCommander(1);
    }

    private MessageDialogData TalkTToCommander(int coef)
    {
        var mianAnswers = new List<AnswerDialogData>();
        if (SkillWork(20, MainController.Instance.MainPlayer.ReputationData.ReputationFaction[_config] * coef))
        {
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretApplyDeal"), null, applyDeal));
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretLeave"), null, tryRun));
            var mesData =
                new MessageDialogData(Namings.DialogTag("secretTheyMessage"),
                    mianAnswers);
            return mesData;
        }
        else
        {
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretFight"), () => Fight(fullCoef, true), null));
            var mesData =
                new MessageDialogData(
                    Namings.DialogTag("secretTheyGoKill"),
                    mianAnswers);
            return mesData;
        }
    }

    private MessageDialogData connectWithCommander()
    {
        return TalkTToCommander(2);
    }

    private MessageDialogData fakeSignal()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretAttack"), () => Fight(halfCoef, true), null));
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretConnectWith"), null, connectWithCommander));
        var mesData = new MessageDialogData(Namings.DialogTag("secretPartGo"), mianAnswers);
        return mesData;
    }

    private MessageDialogData hideAndWait()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretDoProvocation"), null, provacation));
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretWait"), null, waitDeal));
        var mesData = new MessageDialogData(Namings.Format(Namings.DialogTag("secretSecondCome"), Namings.ShipConfig(_secondSide)), mianAnswers);
        return mesData;
    }

    private MessageDialogData waitDeal()
    {
        var mianAnswers = new List<AnswerDialogData>();
        if (MyExtensions.IsTrue01(.2f))
        {
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretSearchPlace"), null, searchResult));
        }
        else
        {
            mianAnswers.Add(new AnswerDialogData(Namings.Tag("Ok"), null));
        }
        var mesData = new MessageDialogData(Namings.DialogTag("secretTheyEnd"), mianAnswers);
        return mesData;
    }

    private MessageDialogData searchResult()
    {
        var player = MainController.Instance.MainPlayer;
        var coef = (float)_power * Library.MONEY_QUEST_COEF;
        var money = (int)(MyExtensions.Random(5, 9) * coef * player.SafeLinks.CreditsCoef);
        player.MoneyData.AddMoney(money);
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("secretTake"), money)));
        var mesData = new MessageDialogData(Namings.DialogTag("secretForgotCredits"), mianAnswers);
        return mesData;

    }

    private MessageDialogData applyDeal()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretTakeAll"), null, tryRunWithAll));
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretRun"), null, tryRun));
        var mesData = new MessageDialogData(Namings.DialogTag("secretAppearSecond"), mianAnswers);
        return mesData;
    }


    private MessageDialogData provacation()
    {
        if (MyExtensions.IsTrueEqual())
        {
            var player = MainController.Instance.MainPlayer;
            var mianAnswers = new List<AnswerDialogData>();
            var coef = (float)_power * Library.MONEY_QUEST_COEF;
            var _moneyToBuy = (int)(MyExtensions.Random(24, 47) * coef * player.SafeLinks.CreditsCoef);
            player.MoneyData.AddMoney(_moneyToBuy);
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("Ok"), null, null));
            var mesData = new MessageDialogData(
                Namings.Format(Namings.DialogTag("secretFindGoods"), _moneyToBuy),
                mianAnswers);
            return mesData;
        }
        else
        {
            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("secretFight"), null, null));
            var mesData = new MessageDialogData(Namings.DialogTag("secretProvoceFail"), mianAnswers);
            return mesData;
        }
    }


    private void Fight(float coef, bool onlyStartFleet)
    {
        var myArmyPower = ArmyCreator.CalcArmyPower(MainController.Instance.MainPlayer.Army) * coef;
        Player opponent;
        if (onlyStartFleet)
        {
            opponent = GetArmy(_config, (int)myArmyPower);
        }
        else
        {
            var cngs = new List<ShipConfig>() { ShipConfig.federation, ShipConfig.ocrons, ShipConfig.krios };
            cngs.Remove(_config);
            ShipConfig sc = cngs.RandomElement();
            opponent = GetArmy(_config, sc, (int)myArmyPower);
        }
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer, opponent);
    }

}