
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
        _isTrap = MyExtensions.IsTrue01(0.2f);
    }

    public override string Desc()
    {
        return Namings.Tag("BrokenNavigation");
    }

    public override MessageDialogData GetDialog()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("navigation_closer"), null, comeCloser));
        mianAnswers.Add(new AnswerDialogData(Namings.Tag("leave"), null));
        var mesData = new MessageDialogData(Namings.DialogTag("navigation_start"), mianAnswers);
        return mesData;
    }

    private MessageDialogData comeCloser()
    {
        var mianAnswers = new List<AnswerDialogData>();
        if (_isTrap)
        {

            var mesData = new MessageDialogData(Namings.DialogTag("navigation_trap"), mianAnswers);
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("navigation_fight"), Fight, null));
            return mesData;
        }
        else
        {
            var mesData = new MessageDialogData(Namings.Format(Namings.DialogTag("navigation_askHelp"), Namings.ShipConfig(_config)), mianAnswers);
            mianAnswers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("navigation_shelter"), ScoutsLevel), null, tryFindWay));
            mianAnswers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("navigation_repair"), RepairLevel), null, tryRepair));
            mianAnswers.Add(new AnswerDialogData(Namings.Tag("leave"), null));
            return mesData;
        }
    }

    private void Fight()
    {
        var myArmyPower = ArmyCreator.CalcArmyPower(MainController.Instance.MainPlayer.Army);
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer,
            GetArmy(_config, (int)myArmyPower));
    }

    private MessageDialogData tryFindWay()
    {
        if (SkillWork(2, ScoutsLevel))
        {
            string d = "";

            if (MyExtensions.IsTrue01(.8f))
            {
                var m = Library.CreatSimpleModul(2);
                var canAdd = MainController.Instance.MainPlayer.Inventory.GetFreeSimpleSlot(out var slot);
                if (canAdd)
                {
                    MainController.Instance.MainPlayer.Inventory.TryAddSimpleModul(m, slot);
                    var itemName = Namings.SimpleModulName(m.Type);
                    d = Namings.Format(Namings.DialogTag("navigation_gift"), itemName);
                }
                else
                {
                    d = Namings.DialogTag("navigation_noFree");
                }
            }
            else
            {
                var cellsToScout = MainController.Instance.MainPlayer.MapData.ScoutedCells(3, 5);
                d = Namings.Format(Namings.DialogTag("navigation_scouted"), cellsToScout); ;
            }


            var mianAnswers = new List<AnswerDialogData>();
            _reputation.AddReputation(_config, 8);
            var mesData = new MessageDialogData(Namings.Format(Namings.DialogTag("navigation_shelterOk"), d), mianAnswers);
            mianAnswers.Add(new AnswerDialogData(Namings.Tag("Ok"), null, null));
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
            _reputation.AddReputation(_config, 6);
            var mianAnswers = new List<AnswerDialogData>();
            var mesData = new MessageDialogData(Namings.DialogTag("navigation_repairOk"), mianAnswers);
            if (MainController.Instance.MainPlayer.Army.CanAddShip())
                mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("navigation_tryHire"), null, tryHire));
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("navigation_takeMoney"), TakeMoney, null));
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
        var money = (int)(GlobalMapCell.AddMoney(24, 29) * coef * MainController.Instance.MainPlayer.SafeLinks.CreditsCoef);
        MainController.Instance.MainPlayer.MoneyData.AddMoney(money);
    }

    private MessageDialogData tryHire()
    {
        if (MainController.Instance.MainPlayer.Army.CanAddShip())
        {
            var mianAnswers = new List<AnswerDialogData>();
            var ship = HireAction(1);
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("Ok"), null, null));
            var mesData = new MessageDialogData(Namings.Format(Namings.DialogTag("navigation_hired"), Namings.ShipConfig(ship.Ship.ShipConfig)), null);
            return mesData;
        }
        else
        {
            var mianAnswers = new List<AnswerDialogData>();
            var mesData = new MessageDialogData(Namings.DialogTag("navigation_noFree"), null);
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("Ok"), null));
            return mesData;
        }
    }
}

