using System;
using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class ScienceLabMapEvent : BaseGlobalMapEvent
{
    private bool _scienceKilled = false;
    private bool _fightStarts = false;
    private int _moneyToBuy;
    public ScienceLabMapEvent(int power, ShipConfig config)
        : base(config)
    {
        _power = power;
    }
    public override string Desc()
    {
        return Namings.Tag("ScienceLaboratory");
    }

    public override MessageDialogData GetDialog()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("scLabAttackNow"), () => AttackNow(true)));
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("scLabContact"), null, contacWithCommander));
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("scLabSedScouts"), null, sendScouts));


        var coef = (float)_power * Library.MONEY_QUEST_COEF;
        _moneyToBuy = (int)(MyExtensions.Random(10, 30) * coef);
        mianAnswers.Add(new AnswerDialogData(Namings.Tag("leave"), null));
        var mesData = new MessageDialogData(String.Format(Namings.DialogTag("scLabStart"), Namings.ShipConfig(_config)), mianAnswers);
        return mesData;
    }

    private MessageDialogData sendScouts()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("scLabFrighten"), null, frighten));
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("scLabBetterContact"), null, contacWithCommander));
        mianAnswers.Add(new AnswerDialogData(Namings.Tag("leave"), null));
        var mesData = new MessageDialogData(Namings.DialogTag("scLabNotRegular"), mianAnswers);
        return mesData;
    }

    private MessageDialogData frighten()
    {
        var mianAnswers = new List<AnswerDialogData>();
        if (SkillWork(3, ScoutsLevel))
        {
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("scLabGoStation"), null, ImproveDialog));
            var mesData = new MessageDialogData(Namings.DialogTag("scLabTheyRun"), mianAnswers);
            return mesData;
        }
        else
        {

            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("scLabFight"), () => AttackNow(true)));
            var mesData = new MessageDialogData(Namings.DialogTag("scLabTotalFail"), mianAnswers);
            return mesData;
        }
    }


    private MessageDialogData contacWithCommander()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(
            string.Format(Namings.DialogTag("scLabGiveCredits"), _moneyToBuy), null, moneyGive));
        mianAnswers.Add(new AnswerDialogData(Namings.Tag("leave"), null));
        var mesData = new MessageDialogData(Namings.DialogTag("scLabSimpleThiefs"), mianAnswers);
        return mesData;
    }

    private MessageDialogData moneyGive()
    {
        var mianAnswers = new List<AnswerDialogData>();
        if (MainController.Instance.MainPlayer.MoneyData.HaveMoney(_moneyToBuy))
        {
            MainController.Instance.MainPlayer.MoneyData.RemoveMoney(_moneyToBuy);
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("scLabGoStation"), null, ImproveDialog));
            var mesData = new MessageDialogData(Namings.DialogTag("scLabTheLeave"), mianAnswers);
            return mesData;
        }
        else
        {
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("scLabFight"), () => AttackNow(false), null));
            var mesData = new MessageDialogData(Namings.DialogTag("scLabBadJoke"), mianAnswers);
            return mesData;
        }
    }

    private void AttackNow(bool shallKillSciens)
    {
        _scienceKilled = shallKillSciens;
        _fightStarts = true;

        if (_scienceKilled)
            _reputation.RemoveReputation(_config, 5);
        var myArmyPower = ArmyCreator.CalcArmyPower(MainController.Instance.MainPlayer.Army);
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer,
            GetArmy(ShipConfig.ocrons, ArmyCreatorType.destroy, (int)myArmyPower));

    }

    public override MessageDialogData GetLeavedActionInner()
    {
        if (_fightStarts)
        {
            if (_scienceKilled)
            {
                var mianAnswers = new List<AnswerDialogData>();
                mianAnswers.Add(new AnswerDialogData(Namings.Ok, null));
                var mesData = new MessageDialogData(Namings.DialogTag("scLabAllKilled"), mianAnswers);
                return mesData;
            }
            else
            {

                return ImproveDialog();
            }
        }
        else
        {
            return null;
        }
    }

    private MessageDialogData ImproveDialog()
    {

        _reputation.AddReputation(_config, Library.REPUTATION_SCIENS_LAB_ADD);
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("scLabImproveMain"), null, improveMainShip));
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("scLabImproveBattle"), null, improveBattleShips));
        var mesData = new MessageDialogData(Namings.DialogTag("scLabScientsChoose"), mianAnswers);
        return mesData;
    }

    private MessageDialogData improveBattleShips()
    {
        var army = MainController.Instance.MainPlayer.Army.Army.Where(x => x.Ship.ShipType != ShipType.Base && x.Pilot.CanUpgradeByLevel(9999)).ToList();
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.Ok, null));
        if (army.Count > 0)
        {
            var rnd = army.RandomElement();
            var parameter = rnd.Pilot.UpgradeRandomLevel(false, true);
            return new MessageDialogData(
                string.Format(Namings.DialogTag("scLabShipUpgraded"), Namings.ParameterName(parameter), rnd.Ship.Name), mianAnswers);
        }
        else
        {
            return new MessageDialogData(Namings.DialogTag("scLabAllMax"), mianAnswers);
        }
    }

    private MessageDialogData improveMainShip()
    {
        var playerParams = MainController.Instance.MainPlayer.Parameters;

        var allParams = new List<PlayerParameter>();
        if (playerParams.ChargesCount.CanUpgrade())
        {
            allParams.Add(playerParams.ChargesCount);
        }
        if (playerParams.ChargesSpeed.CanUpgrade())
        {
            allParams.Add(playerParams.ChargesSpeed);
        }
        if (playerParams.Repair.CanUpgrade())
        {
            allParams.Add(playerParams.Repair);
        }
        if (playerParams.Scouts.CanUpgrade())
        {
            allParams.Add(playerParams.Scouts);
        }
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.Ok, null));
        if (allParams.Count > 0)
        {
            var rnd = allParams.RandomElement();
            return new MessageDialogData(string.Format(Namings.DialogTag("scParamUpgraded"), rnd.Name), mianAnswers);
        }
        else
        {

            return new MessageDialogData(Namings.DialogTag("scLabNotingUpgrade"), mianAnswers);
        }
    }

}

