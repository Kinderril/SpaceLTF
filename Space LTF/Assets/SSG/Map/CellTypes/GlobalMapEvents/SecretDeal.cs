using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[System.Serializable]
public class SecretDeal : BaseGlobalMapEvent
{
    private float fullCoef = 1.4f;
    private float halfCoef = 0.7f;

    public override string Desc()
    {
        return "Quiet place";
    }

    public override MessageDialogData GetDialog()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData($"Send scouts", null, scoutsSend));
        mianAnswers.Add(new AnswerDialogData(Namings.leave, null));
        var mesData =
            new MessageDialogData("A lot of space garbage and other useless thing here. Do you want to investigate?",
                mianAnswers);
        return mesData;
    }

    private MessageDialogData scoutsSend()
    {
        var mianAnswers = new List<AnswerDialogData>();
        if (SkillWork(3, ScoutsLevel))
        {
            mianAnswers.Add(new AnswerDialogData($"Hide and wait", null, hideNadWait));
            mianAnswers.Add(new AnswerDialogData($"Send fake signal", null, fakeSignal));
            mianAnswers.Add(new AnswerDialogData($"Attack!", () => Fight(fullCoef)));
            mianAnswers.Add(new AnswerDialogData($"Connect with commander.", null, connectWithCommander));
            var mesData =
                new MessageDialogData($"Scouts find a huge fleet, waiting for something. [Scouts:{ScoutsLevel}]",
                    mianAnswers);
            return mesData;
        }
        else
        {
            mianAnswers.Add(new AnswerDialogData($"Try to prevent conflict", null, preventConflict));
            mianAnswers.Add(new AnswerDialogData($"Run", null, tryRun));
            var mesData =
                new MessageDialogData($"Suddenly you see a huge army. And they are ready! [Scouts:{ScoutsLevel}]",
                    mianAnswers);
            return mesData;
        }
    }

    private MessageDialogData tryRunWithAll()
    {
        var money = GlobalMapCell.AddMoney(24, 47);
        var m = Library.CreatSimpleModul(2,4);
        var itemName = Namings.SimpleModulName(m.Type);
        var canAdd = MainController.Instance.MainPlayer.Inventory.GetFreeSimpleSlot(out var slot);
        if (canAdd)
        {
            MainController.Instance.MainPlayer.Inventory.TryAddSimpleModul(m, slot);
        }

        return tryRunWithCoef(-1, $"And now you have [credits{money} and {itemName}]");
    }

    private MessageDialogData tryRun()
    {
        return tryRunWithCoef(0, "");
    }

    private MessageDialogData tryRunWithCoef(int coef, string add)
    {
        var mianAnswers = new List<AnswerDialogData>();
        if (SkillWork(2, ScoutsLevel + coef))
        {
            ;
            mianAnswers.Add(new AnswerDialogData(Namings.Ok, null, null));
            var mesData = new MessageDialogData($"You run away. {add} [Scouts:{ScoutsLevel}]", mianAnswers);
            return mesData;
        }
        else
        {
            mianAnswers.Add(new AnswerDialogData($"Fight", () => Fight(fullCoef), null));
            var mesData = new MessageDialogData($"Running away failed! [Scouts:{ScoutsLevel}]", mianAnswers);
            return mesData;
        }
    }

    private MessageDialogData preventConflict()
    {
        return TalkTToCommander(0);
    }

    private MessageDialogData TalkTToCommander(int coef)
    {
        var mianAnswers = new List<AnswerDialogData>();
        if (SkillWork(2, DiplomacyLevel + coef))
        {
            mianAnswers.Add(new AnswerDialogData($"Apply deal", null, applyDeal));
            mianAnswers.Add(new AnswerDialogData($"Run", null, tryRun));
            var mesData =
                new MessageDialogData($"They send you a message with trade options. [Diplomacy:{DiplomacyLevel}]",
                    mianAnswers);
            return mesData;
        }
        else
        {
            mianAnswers.Add(new AnswerDialogData($"Fight", () => Fight(fullCoef), null));
            var mesData =
                new MessageDialogData(
                    $"Now they going to kill. Diplomacy is not your good side. [Diplomacy:{DiplomacyLevel}]",
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
        mianAnswers.Add(new AnswerDialogData($"Attack.", () => Fight(halfCoef), null));
        mianAnswers.Add(new AnswerDialogData($"Connect with commander.", null, connectWithCommander));
        var mesData = new MessageDialogData("Part of fleet going to chekc signal.", mianAnswers);
        return mesData;
    }

    private MessageDialogData hideNadWait()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData($"Do provocation.", null, provacation));
        mianAnswers.Add(new AnswerDialogData($"Wait.", null, waitDeal));
        var mesData = new MessageDialogData("Second army coming.", mianAnswers);
        return mesData;
    }

    private MessageDialogData waitDeal()
    {
        var mianAnswers = new List<AnswerDialogData>();
        if (MyExtensions.IsTrue01(.2f))
        {
            mianAnswers.Add(new AnswerDialogData($"Search all place.", null,searchResult));
        }
        else
        {
            mianAnswers.Add(new AnswerDialogData(Namings.Ok, null));
        }
        var mesData = new MessageDialogData("They complete trade, and ready to jump.", mianAnswers);
        return mesData;
    }

    private MessageDialogData searchResult()
    {
        var money = GlobalMapCell.AddMoney(5, 9);
        MainController.Instance.MainPlayer.MoneyData.AddMoney(money);
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData($"Take. [Credits:{money}]"));
        var mesData = new MessageDialogData("Looks like they forgot some credits.", mianAnswers);
        return mesData;

    }

    private MessageDialogData applyDeal()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData($"Take all and run.", null, tryRunWithAll));
        mianAnswers.Add(new AnswerDialogData($"Run.", null, tryRun));
        var mesData = new MessageDialogData("While trading, appears another fleet.", mianAnswers);
        return mesData;
    }


    private MessageDialogData provacation()
    {
        if (MyExtensions.IsTrueEqual())
        {
            var mianAnswers = new List<AnswerDialogData>();
            var money = GlobalMapCell.AddMoney(24, 47);
            mianAnswers.Add(new AnswerDialogData($"Ok.", null, null));
            var mesData = new MessageDialogData($"After massive battle you find some goods. [Credits:{money}]",
                mianAnswers);
            return mesData;
        }
        else
        {
            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData($"Fight.", null, null));
            var mesData = new MessageDialogData("Your provocation failed. Now fight.", mianAnswers);
            return mesData;
        }
    }


    private void Fight(float coef)
    {
        var myArmyPower = ArmyCreator.CalcArmyPower(MainController.Instance.MainPlayer.Army) * coef;
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer,
            GetArmy(ShipConfig.raiders, ArmyCreatorType.laser, (int) myArmyPower));
    }

}