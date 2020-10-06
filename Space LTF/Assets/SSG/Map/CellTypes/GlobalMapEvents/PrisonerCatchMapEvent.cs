
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
        mianAnswers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("prisonerBuy"), _itemCost), null, BuyStolen));
        mianAnswers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("prisonerCatch")), null, ReternToPolice));
        if (MainController.Instance.MainPlayer.Army.CanAddShip())
            mianAnswers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("prisonerHire")), null,HireHim));
        mianAnswers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("prisonerHide"), Reputation), null, HideHim));


        var mesData = new MessageDialogData(Namings.Format(Namings.DialogTag("prisonerStart"), Namings.ShipConfig(_config)), mianAnswers);
        mianAnswers.Add(new AnswerDialogData(Namings.Tag("leave"), null));
        return mesData;
    }

    private MessageDialogData HireHim()
    {
        var rep = Library.REPUTATION_HIRE_CRIMINAL_REMOVED;
        var info = Namings.Format(Namings.DialogTag("prisonerHired"), rep);
        foreach (var cfg in Library.ConfigsNoDroid())
        {
            MainController.Instance.MainPlayer.ReputationData.RemoveReputation(cfg, rep);
        }
       
        HireAction(2);
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("Ok")), null));
        var mesData = new MessageDialogData(Namings.Format(info), mianAnswers);
        return mesData;
    }

    public void Fight()
    {
//        MainController.Instance.MainPlayer.ReputationData.RemoveReputation(_config, Library.REPUTATION_HIRE_CRIMINAL_REMOVED);
        var myArmyPower = ArmyCreator.CalcArmyPower(MainController.Instance.MainPlayer.Army);
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer,
            GetArmy(_config, (int)myArmyPower));
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
                d = Namings.Format(Namings.DialogTag("prisonerYourItem"), itemName);
            }
            else
            {
                d = Namings.DialogTag("prisonerNoSpace");
            }

//            MainController.Instance.MainPlayer.ReputationData.AddReputation(_config, Library.REPUTATION_FIND_WAY_ADD);



            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("Ok")), null));
            var mesData = new MessageDialogData(Namings.Format(Namings.DialogTag("prisonerComplete"), d), mianAnswers);
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
            var player = MainController.Instance.MainPlayer;
            var coef = (float)_power * Library.MONEY_QUEST_COEF;
            var money = (int)(MyExtensions.Random(14, 30) * coef * player.SafeLinks.CreditsCoef);
            player.MoneyData.AddMoney(money);
            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("Ok")), null));
            var msg = Namings.Format(Namings.DialogTag("prisonerHideOk"), money);
            var mesData = new MessageDialogData(msg, mianAnswers);
            return mesData;
        }
        else
        {
            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("Ok")), null));
            var msg = Namings.Format(Namings.DialogTag("prisonerFailFight"));
            var mesData = new MessageDialogData(msg, mianAnswers);
            return mesData;
        }
    }


    private MessageDialogData ReternToPolice()
    {
        var rep = Library.REPUTATION_FIND_WAY_ADD;
        MainController.Instance.MainPlayer.ReputationData.AddReputation(_config, rep);

        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("Ok")), null));
        var msg = Namings.Format(Namings.DialogTag("prisonerCatchOk"), rep);
        var mesData = new MessageDialogData(msg, mianAnswers);
        return mesData;
    }

    public PrisonerCatchMapEvent(ShipConfig config) : base(config)
    {
    }
}

