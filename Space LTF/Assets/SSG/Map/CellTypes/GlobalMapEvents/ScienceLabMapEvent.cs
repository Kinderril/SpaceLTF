
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
        return "Science laboratory";
    }

    public override MessageDialogData GetDialog()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData("Attack immediately", () => AttackNow(true)));
        mianAnswers.Add(new AnswerDialogData("Contact with fleet commander",null,contacWithCommander));
        mianAnswers.Add(new AnswerDialogData("Snd scouts to laboratory",null,sendScouts));


        var coef = (float)_power * Library.MONEY_QUEST_COEF;
        _moneyToBuy = (int)(MyExtensions.Random(10, 30) * coef);
        mianAnswers.Add(new AnswerDialogData(Namings.leave, null));
        var mesData = new MessageDialogData("You are close to krios science laboratory. But this one is under siege.", mianAnswers);
        return mesData;
    }

    private MessageDialogData sendScouts()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData($"Try to frighten them.", null, frighten));
        mianAnswers.Add(new AnswerDialogData("No, better I contact with them.", null, contacWithCommander));
        mianAnswers.Add(new AnswerDialogData(Namings.leave, null));
        var mesData = new MessageDialogData("This isn't regular army. We can try to frighten them.", mianAnswers);
        return mesData;
    }

    private MessageDialogData frighten()
    {
        var mianAnswers = new List<AnswerDialogData>();
        if (SkillWork(3, ScoutsLevel))
        {

            mianAnswers.Add(new AnswerDialogData("Go to station.",null, ImproveDialog));
            var mesData = new MessageDialogData("They running away!.", mianAnswers);
            return mesData;
        }
        else
        {

            mianAnswers.Add(new AnswerDialogData("Fight", () => AttackNow(true)));
            var mesData = new MessageDialogData("Fail! They just launch rockets to laboratory and now wants to kill you.", mianAnswers);
            return mesData;
        }
    }


    private MessageDialogData contacWithCommander()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData($"Maybe if just give you some credits and you will leave [Credits:{_moneyToBuy}]", null,moneyGive));
        mianAnswers.Add(new AnswerDialogData(Namings.leave, null));
        var mesData = new MessageDialogData("They look like simple thief.", mianAnswers);
        return mesData;
    }

    private MessageDialogData moneyGive()
    {
        var mianAnswers = new List<AnswerDialogData>();
        if (MainController.Instance.MainPlayer.MoneyData.HaveMoney(_moneyToBuy))
        {
            MainController.Instance.MainPlayer.MoneyData.RemoveMoney(_moneyToBuy);
            mianAnswers.Add(new AnswerDialogData("Go to station.", null, ImproveDialog));
            var mesData = new MessageDialogData("They leaving.", mianAnswers);
            return mesData;
        }
        else
        {
            mianAnswers.Add(new AnswerDialogData("Fight.", ()=>AttackNow(false), null));
            var mesData = new MessageDialogData("Bad joke! [Not enough credits]", mianAnswers);
            return mesData;
        }
    }      

    private void AttackNow(bool shallKillSciens)
    {
        _scienceKilled = shallKillSciens;
        _fightStarts = true;

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
                var mesData = new MessageDialogData("All scientists were killed.", mianAnswers);
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

        MainController.Instance.MainPlayer.ReputationData.AddReputation(ShipConfig.krios,Library.REPUTATION_SCIENS_LAB_ADD);
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData("Improve main ship.", null, improveMainShip));
        mianAnswers.Add(new AnswerDialogData("Improve battle ships.", null, improveBattleShips));
        var mesData = new MessageDialogData("Scientists are alive. And they can improve your army", mianAnswers);
        return mesData;
    }

    private MessageDialogData improveBattleShips()
    {
        var army = MainController.Instance.MainPlayer.Army.Where(x=>x.Ship.ShipType != ShipType.Base && x.Pilot.CanUpgradeByLevel(9999)).ToList();
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.Ok, null));
        if (army.Count > 0)
        {
            var rnd = army.RandomElement();
            var parameter = rnd.Pilot.UpgradeRandomLevel(false,true);
            return new MessageDialogData($"{Namings.ParameterName(parameter)} upgrade at ship {rnd.Ship.Name} Upgraded.", mianAnswers);
        }
        else
        {
            return new MessageDialogData("All your ships have max level.", mianAnswers);
        }
    }

    private MessageDialogData improveMainShip()
    {
        var playerParams = MainController.Instance.MainPlayer.Parameters;

        var allParams = new    List<PlayerParameter>();
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
            return new MessageDialogData($"{rnd.Name} Upgraded", mianAnswers);
        }
        else
        {

            return new MessageDialogData("Nothing to improve.", mianAnswers);
        }
    }

}

