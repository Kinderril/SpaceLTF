
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[System.Serializable]
public class PrisonerCatchMapEvent : BaseGlobalMapEvent
{
    private int _itemCost = 15;

    public override string Desc()
    {
        return "Prisoner";
    }

    public override MessageDialogData GetDialog()
    {
        _itemCost = 20;
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(String.Format("Buy stolen item. [Cost:{0}]", _itemCost),BuyStolen));
        mianAnswers.Add(new AnswerDialogData(String.Format("Catch and return to police."),ReternToPolice));
        mianAnswers.Add(new AnswerDialogData(String.Format("Hire him"),HireHim));
        mianAnswers.Add(new AnswerDialogData(String.Format("Hide him from police. [Reputation:{0}]",ReputationLevel), HideHim));


        var mesData = new MessageDialogData("Criminal try to escapes from the police. But your fleet can catch him", mianAnswers);
        mianAnswers.Add(new AnswerDialogData("Leave", null));
        return mesData;
    }

    private void HireHim()
    {
        var rep = Library.REPUTATION_HIRE_CRIMINAL_REMOVED;
        var info = String.Format("Criminal hired. Reputation removed {0}", rep);
        MainController.Instance.MainPlayer.ReputationData.RemoveReputation(rep);
        WindowManager.Instance.InfoWindow.Init(() =>
        {
            HireAction(2);
        }, info );

    }

    public void Fight()
    {
        var myArmyPower = ArmyCreator.CalcArmyPower(MainController.Instance.MainPlayer.Army);
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer, 
            GetArmy(ShipConfig.federation, ArmyCreatorType.laser, (int)myArmyPower));
    }

    private void BuyStolen()
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
                d = String.Format("Your item: {0}", itemName);
            }
            else
            {
                d = "Not free space for item";
            }

            MainController.Instance.MainPlayer.ReputationData.AddReputation(Library.REPUTATION_FIND_WAY_ADD);
            WindowManager.Instance.InfoWindow.Init(null,
                String.Format("Complete. {0}", d));
        }
        else
        {
            ShowFail();
        }
    }

    private void HideHim()
    {
        if (SkillWork(60, ReputationLevel))
        {
            var money = GlobalMapCell.AddMoney(24, 47);
            WindowManager.Instance.InfoWindow.Init(null, 
                String.Format("You successfully hide criminals ship. Credits add {0}", money));
        }
        else
        {
            if (SkillWork(40, ReputationLevel))
            {
                ShowFail();
            }
            else
            {
                MainController.Instance.MainPlayer.ReputationData.RemoveReputation(Library.REPUTATION_HIRE_CRIMINAL_REMOVED);
                WindowManager.Instance.InfoWindow.Init(Fight,
                    String.Format("Fail! Now you will fight with police."));
            }
        }
    }
    

    private void ReternToPolice()
    {
        var rep = Library.REPUTATION_FIND_WAY_ADD;
        WindowManager.Instance.InfoWindow.Init(null, 
            String.Format("You successfully catch him. Reputation added {0}", rep));
        MainController.Instance.MainPlayer.ReputationData.AddReputation(rep);
    }
}

