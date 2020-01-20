
using System;
using System.Collections.Generic;


[System.Serializable]
public class BrokenNavigationMapEvent : BaseGlobalMapEvent
{
    private bool _isTrap;

    public BrokenNavigationMapEvent(int power, ShipConfig config)
        : base(config)
    {
        _power = power;
    }
    public override void Init()
    {
        _isTrap = MyExtensions.IsTrueEqual();
    }

    public override string Desc()
    {
        return "Broken navigation";
    }

    public override MessageDialogData GetDialog()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData("Come closer", null, comeCloser));
        mianAnswers.Add(new AnswerDialogData(Namings.Tag("leave"), null));
        var mesData = new MessageDialogData("You have a distress signal", mianAnswers);
        return mesData;
    }

    private MessageDialogData comeCloser()
    {
        var mianAnswers = new List<AnswerDialogData>();
        if (_isTrap)
        {

            var mesData = new MessageDialogData("It's a trap!", mianAnswers);
            mianAnswers.Add(new AnswerDialogData("Fight", Fight, null));
            return mesData;
        }
        else
        {
            var mesData = new MessageDialogData("You see a ocrons ship with broken navigation system. He asking for help", mianAnswers);
            mianAnswers.Add(new AnswerDialogData(String.Format("Send scout to deliver him to closest shelter. [Scouts: {0}]", ScoutsLevel), null, tryFindWay));
            mianAnswers.Add(new AnswerDialogData(String.Format("Try repair navigation system. [Repair {0}]", RepairLevel), null, tryRepair));
            mianAnswers.Add(new AnswerDialogData(Namings.Tag("leave"), null));
            return mesData;
        }
    }

    private void Fight()
    {
        var myArmyPower = ArmyCreator.CalcArmyPower(MainController.Instance.MainPlayer.Army);
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer,
            GetArmy(ShipConfig.mercenary, ArmyCreatorType.simple, (int)myArmyPower));
    }

    private MessageDialogData tryFindWay()
    {
        if (SkillWork(2, ScoutsLevel))
        {
            string d = "";

            if (MyExtensions.IsTrueEqual())
            {
                var m = Library.CreatSimpleModul(2);
                var canAdd = MainController.Instance.MainPlayer.Inventory.GetFreeSimpleSlot(out var slot);
                if (canAdd)
                {
                    MainController.Instance.MainPlayer.Inventory.TryAddSimpleModul(m, slot);
                    var itemName = Namings.SimpleModulName(m.Type);
                    d = $"Your gift: {itemName}";
                }
                else
                {
                    d = "Not free space for gift";
                }
            }
            else
            {
                var cellsToScout = MainController.Instance.MainPlayer.MapData.ScoutedCells(3, 5);
                d = $"{cellsToScout} points on global map scouted.";
            }


            var mianAnswers = new List<AnswerDialogData>();
            var mesData = new MessageDialogData($"You successfully find way to shelter. {d}.", mianAnswers);
            mianAnswers.Add(new AnswerDialogData("Ok", null, null));
            return mesData;

        }
        else
        {
            return failResult();
        }
    }

    private MessageDialogData tryRepair()
    {
        if (SkillWork(2, RepairLevel))
        {

            MainController.Instance.MainPlayer.ReputationData.AddReputation(ShipConfig.ocrons, Library.REPUTATION_FIND_WAY_ADD);

            var mianAnswers = new List<AnswerDialogData>();
            var mesData = new MessageDialogData($"You successfully repair ship. Reputation add {Library.REPUTATION_FIND_WAY_ADD}.", mianAnswers);
            mianAnswers.Add(new AnswerDialogData("Try hire", null, tryHire));
            mianAnswers.Add(new AnswerDialogData("Take money", TakeMoney, null));
            return mesData;

        }
        else
        {
            return failResult();
        }
    }

    private void TakeMoney()
    {
        var coef = (float)_power * Library.MONEY_QUEST_COEF;
        var money = (int)(GlobalMapCell.AddMoney(24, 29) * coef);
        MainController.Instance.MainPlayer.MoneyData.AddMoney(money);
    }

    private MessageDialogData tryHire()
    {
        if (MainController.Instance.MainPlayer.Army.CanAddShip())
        {
            var mianAnswers = new List<AnswerDialogData>();
            var ship = HireAction(1);
            mianAnswers.Add(new AnswerDialogData("Ok", null, null));
            var mesData = new MessageDialogData($"Ship hired {Namings.ShipConfig(ship.Ship.ShipConfig)}.", null);
            return mesData;
        }
        else
        {
            var mianAnswers = new List<AnswerDialogData>();
            var mesData = new MessageDialogData($"Not enough space.", null);
            mianAnswers.Add(new AnswerDialogData("Ok", null));
            return mesData;
        }
    }
}

