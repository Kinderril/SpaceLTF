
using System;
using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class BattlefieldMapEvent : BaseGlobalMapEvent
{
    public BattlefieldMapEvent(int power, ShipConfig config)
        : base(config)
    {
        _power = power;
    }
    public override string Desc()
    {
        return Namings.Tag("battlefield");
    }

    public override MessageDialogData GetDialog()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(String.Format(Namings.DialogTag("battlefield_provocate"), ScoutsLevel), null, provacation));
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("battlefield_reconcile"), null, reconcile));
        mianAnswers.Add(new AnswerDialogData(Namings.Tag("leave"), null));

        var mesData = new MessageDialogData(String.Format(Namings.DialogTag("battlefield_start"), Namings.ShipConfig(_config)), mianAnswers);
        return mesData;
    }

    private MessageDialogData reconcile()
    {
        var isRep = MainController.Instance.MainPlayer.ReputationData.ReputationFaction[_config] > 20;
        if (isRep)
        {
            var mianAnswers = new List<AnswerDialogData>();
            MainController.Instance.MainPlayer.ReputationData.AddReputation(_config, 10);
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("battlefield_thanks"), () => GetItemsAfterBattle(false), null));
            var mesData = new MessageDialogData(Namings.DialogTag("battlefield_diplomacyWin"), mianAnswers);
            return mesData;
        }
        else
        {
            var mianAnswers = new List<AnswerDialogData>();
            MainController.Instance.MainPlayer.ReputationData.RemoveReputation(_config, 5);
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("battlefield_run"), null, runOpt));
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("battlefield_shoot"), null, shootNear));
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("battlefield_fight"), Fight, null));
            var mesData = new MessageDialogData(Namings.DialogTag("battlefield_diplomacyFail"), mianAnswers);
            return mesData;
        }
    }

    private MessageDialogData shootNear()
    {
        MessageDialogData mesData;
        var mianAnswers = new List<AnswerDialogData>();
        if (MyExtensions.IsTrueEqual())
        {
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("battlefield_shit"), Fight, null));
            mesData = new MessageDialogData(Namings.DialogTag("battlefield_theyAttack"), mianAnswers);
        }
        else
        {
            MainController.Instance.MainPlayer.ReputationData.AddReputation(_config, 10);
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("battlefield_ufff"), null, null));
            mesData = new MessageDialogData(String.Format(Namings.DialogTag("battlefield_stopAttack")), mianAnswers);

        }
        return mesData;
    }

    private MessageDialogData runOpt()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.Ok, null, null));
        var mesData = new MessageDialogData(Namings.DialogTag("battlefield_youRun"), mianAnswers);
        return mesData;
    }

    private MessageDialogData provacation()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("battlefield_artillery"), null, artillery));
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("battlefield_wait"), null, endBattle));
        var mesData = new MessageDialogData(Namings.DialogTag("battlefield_theyBattle"), mianAnswers);
        return mesData;
    }

    private MessageDialogData endBattle()
    {
        if (MyExtensions.IsTrue01(.2f))
        {
            var coef = (float)_power * Library.MONEY_QUEST_COEF;
            var money = (int)(MyExtensions.Random(14, 30) * coef);
            MainController.Instance.MainPlayer.MoneyData.AddMoney(money);
            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData(Namings.Ok, null, null));
            var mesData = new MessageDialogData(String.Format(Namings.DialogTag("battlefield_killEachOther"), money), mianAnswers);
            return mesData;
        }
        else
        {
            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("battlefield_isee"), null, null));
            var mesData = new MessageDialogData(Namings.DialogTag("battlefield_battleHaveWinner"), mianAnswers);
            return mesData;
        }
    }

    private MessageDialogData artillery()
    {
        var player = MainController.Instance.MainPlayer;
        var rockectWeapons = player.Army.Army.Where(x =>
            x.Ship.WeaponsModuls.FirstOrDefault(y => y != null && (y.WeaponType == WeaponType.rocket || y.WeaponType == WeaponType.casset)) != null);
        if (rockectWeapons.Any() && MyExtensions.IsTrue01(.8f))
        {
            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("battlefield_good"), () => GetItemsAfterBattle(true), null));
            var mesData = new MessageDialogData(Namings.DialogTag("battlefield_goodSHot"), mianAnswers);
            MainController.Instance.MainPlayer.ReputationData.RemoveReputation(_config, 5);
            return mesData;
        }
        else
        {
            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("battlefield_fight"), Fight, null));
            var mesData = new MessageDialogData(Namings.DialogTag("battlefield_killYou"), mianAnswers);
            return mesData;
        }
    }

    private void GetItemsAfterBattle(bool low)
    {
        var m = Library.CreateWeapon(low);
        var canAdd = MainController.Instance.MainPlayer.Inventory.GetFreeWeaponSlot(out var slot);
        if (canAdd)
        {
            MainController.Instance.MainPlayer.Inventory.TryAddWeaponModul(m, slot);
        }
    }

    private void Fight()
    {
        var myArmyPower = ArmyCreator.CalcArmyPower(MainController.Instance.MainPlayer.Army);
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer,
            GetArmy(_config, (int)myArmyPower));
    }
}

