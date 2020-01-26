
using System;
using System.Collections.Generic;


[System.Serializable]
public class PrisonerCatchMapEvent : BaseGlobalMapEvent
{
    private int _itemCost = 15;

    public override string Desc()
    {
        return Namings.Tag("Prisoner");
    }

    private int Reputation =>
        MainController.Instance.MainPlayer.ReputationData.ReputationFaction[_config];

    public override MessageDialogData GetDialog()
    {
        _itemCost = 20;
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(String.Format(Namings.DialogTag("prisonerBuy"), _itemCost), null, BuyStolen));
        mianAnswers.Add(new AnswerDialogData(String.Format(Namings.DialogTag("prisonerCatch")), null, ReternToPolice));
        mianAnswers.Add(new AnswerDialogData(String.Format(Namings.DialogTag("prisonerHire")), HireHim));
        mianAnswers.Add(new AnswerDialogData(String.Format(Namings.DialogTag("prisonerHide"), Reputation), null, HideHim));


        var mesData = new MessageDialogData(String.Format(Namings.DialogTag("prisonerStart"), Namings.ShipConfig(_config)), mianAnswers);
        mianAnswers.Add(new AnswerDialogData(Namings.Tag("leave"), null));
        return mesData;
    }

    private void HireHim()
    {
        var rep = Library.REPUTATION_HIRE_CRIMINAL_REMOVED;
        var info = String.Format(Namings.DialogTag("prisonerHired"), rep);
        MainController.Instance.MainPlayer.ReputationData.RemoveReputation(_config, rep);
        WindowManager.Instance.InfoWindow.Init(() =>
        {
            HireAction(2);
        }, info);

    }

    public void Fight()
    {
        MainController.Instance.MainPlayer.ReputationData.RemoveReputation(_config, Library.REPUTATION_HIRE_CRIMINAL_REMOVED);
        var myArmyPower = ArmyCreator.CalcArmyPower(MainController.Instance.MainPlayer.Army);
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer,
            GetArmy(_config, ArmyCreatorType.laser, (int)myArmyPower));
    }

    private MessageDialogData BuyStolen()
    {
        if (MainController.Instance.MainPlayer.MoneyData.HaveMoney(_itemCost))
        {
            MainController.Instance.MainPlayer.MoneyData.RemoveMoney(_itemCost);
            string d = "";
            int slot;
            var m = Library.CreatSimpleModul(2);
            var itemName = Namings.SimpleModulName(m.Type);
            var canAdd = MainController.Instance.MainPlayer.Inventory.GetFreeSimpleSlot(out slot);
            if (canAdd)
            {
                MainController.Instance.MainPlayer.Inventory.TryAddSimpleModul(m, slot);
                d = String.Format(Namings.DialogTag("prisonerYourItem"), itemName);
            }
            else
            {
                d = Namings.DialogTag("prisonerNoSpace");
            }

            MainController.Instance.MainPlayer.ReputationData.AddReputation(_config, Library.REPUTATION_FIND_WAY_ADD);



            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData(String.Format(Namings.DialogTag("Ok")), null));
            var mesData = new MessageDialogData(String.Format(Namings.DialogTag("prisonerComplete"), d), mianAnswers);
            return mesData;
        }
        else
        {
            return ShowFail(Namings.DialogTag("prisonerBuyFail"));
        }
    }

    private MessageDialogData HideHim()
    {
        if (SkillWork(60, Reputation))
        {
            var coef = (float)_power * Library.MONEY_QUEST_COEF;
            var money = (int)(MyExtensions.Random(14, 30) * coef);
            MainController.Instance.MainPlayer.MoneyData.AddMoney(money);
            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData(String.Format(Namings.DialogTag("Ok")), null));
            var msg = String.Format(Namings.DialogTag("prisonerHideOk"), money);
            var mesData = new MessageDialogData(msg, mianAnswers);
            return mesData;
        }
        else
        {
            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData(String.Format(Namings.DialogTag("Ok")), null));
            var msg = String.Format(Namings.DialogTag("prisonerFailFight"));
            var mesData = new MessageDialogData(msg, mianAnswers);
            return mesData;
        }
    }


    private MessageDialogData ReternToPolice()
    {
        var rep = Library.REPUTATION_FIND_WAY_ADD;
        MainController.Instance.MainPlayer.ReputationData.AddReputation(_config, rep);

        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(String.Format(Namings.DialogTag("Ok")), null));
        var msg = String.Format(Namings.DialogTag("prisonerCatchOk"), rep);
        var mesData = new MessageDialogData(msg, mianAnswers);
        return mesData;
    }

    public PrisonerCatchMapEvent(ShipConfig config) : base(config)
    {
    }
}

