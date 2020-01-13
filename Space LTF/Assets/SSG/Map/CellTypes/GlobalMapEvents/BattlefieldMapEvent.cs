
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[System.Serializable]
public class BattlefieldMapEvent : BaseGlobalMapEvent
{
    public BattlefieldMapEvent(int power,ShipConfig config)   
        :base(config)
    {
        _power = power;
    }
    public override string Desc()
    {
        return "Old battlefield";
    }
    
    public override MessageDialogData GetDialog()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(         String.Format("Hide and provocate them",ScoutsLevel),null,provacation));
        mianAnswers.Add(new AnswerDialogData($"Try to reconcile all sides", null, reconcile));
        mianAnswers.Add(new AnswerDialogData(Namings.leave, null));
        

        var mesData = new MessageDialogData("You see some ships stands against each other.", mianAnswers);
        return mesData;
    }

    private MessageDialogData reconcile()
    {
        var isRep = MainController.Instance.MainPlayer.ReputationData.ReputationFaction[_config] > 40;
        if (isRep)
        {
            var mianAnswers = new List<AnswerDialogData>();
            MainController.Instance.MainPlayer.ReputationData.AddReputation(ShipConfig.mercenary,Library.REPUTATION_SCIENS_LAB_ADD);
            mianAnswers.Add(new AnswerDialogData("Thanks!", () => GetItemsAfterBattle(false), null));
            var mesData = new MessageDialogData("Your diplomacy skills are perfect. They will not fight and send you a gift for helping. Reputation add {Library.REPUTATION_RELEASE_PEACEFULL_ADD}.", mianAnswers);
            return mesData;
        }
        else
        {
            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData("Run!", null, runOpt));
            mianAnswers.Add(new AnswerDialogData("Shoot near!", null, shootNear));
            mianAnswers.Add(new AnswerDialogData("Fight!", Fight, null));
            var mesData = new MessageDialogData("Your diplomacy skills is awful. But now they want to kill you instead of each other.", mianAnswers);
            return mesData;
        }
    }

    private MessageDialogData shootNear()
    {
        MessageDialogData mesData;
var mianAnswers = new List<AnswerDialogData>();
        if (MyExtensions.IsTrueEqual())
        {
            mianAnswers.Add(new AnswerDialogData("Shit.", Fight, null));
            mesData = new MessageDialogData("This is not your day. They attacking you!", mianAnswers);
        }
        else
        {
            MainController.Instance.MainPlayer.ReputationData.AddReputation(ShipConfig.krios,Library.REPUTATION_SCIENS_LAB_ADD); 
            mianAnswers.Add(new AnswerDialogData("Ufff... Great.", null, null));
            mesData = new MessageDialogData($"This stop trying attack anybody. Maybe this is not bad. Reputation add {Library.REPUTATION_SCIENS_LAB_ADD}.", mianAnswers);

        }
        return mesData;
    }

    private MessageDialogData runOpt()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.Ok, null, null));
        var mesData = new MessageDialogData("You run away", mianAnswers);
        return mesData;
    }

    private MessageDialogData provacation()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData($"Catch moment and fire with artillery", null, artillery));
        mianAnswers.Add(new AnswerDialogData("Wait", null,endBattle));
        var mesData = new MessageDialogData("They starts battle.", mianAnswers);
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
            var mesData = new MessageDialogData($"Battle ends. They kill each other. And you find some credits {money}.", mianAnswers);
            return mesData;
        }
        else
        {
            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData("I see", null, null));
            var mesData = new MessageDialogData("Battle ends. Win side take trophies and go away. You can do nothing", mianAnswers);
            return mesData;
        }
    }

    private MessageDialogData artillery()
    {
        var player = MainController.Instance.MainPlayer;
        var rockectWeapons = player.Army.Where(x =>
            x.Ship.WeaponsModuls.FirstOrDefault(y => y != null && (y.WeaponType == WeaponType.rocket || y.WeaponType == WeaponType.casset)) != null);
        if (rockectWeapons.Any() && MyExtensions.IsTrue01(.8f))
        {
            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData("Good",()=> GetItemsAfterBattle(true), null));
            var mesData = new MessageDialogData("Good shoot. You destroy all of them. After battle you find some items", mianAnswers);
            return mesData;
        }
        else
        {
            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData("Fight", Fight, null));
            var mesData = new MessageDialogData("Now they want to destroy you.", mianAnswers);
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
            GetArmy(ShipConfig.federation, ArmyCreatorType.rocket, (int)myArmyPower));
    }
}

