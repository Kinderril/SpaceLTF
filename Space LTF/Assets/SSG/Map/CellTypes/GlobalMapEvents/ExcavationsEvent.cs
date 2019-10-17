
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[System.Serializable]
public class ExcavationsEvent : BaseGlobalMapEvent
{
    private int weaponTryies = 0;
    private const float speedToWin = 4f;
    private const int excvScounts = 3;
    private const int Nothing = 3;
    private const int moneyTotal = 100;

    public override string Desc()
    {
        return "Excavations";
    }

    public override MessageDialogData GetDialog()
    {
        var mesData = new MessageDialogData("You see excavations. Small fleet searching something at this place.\nBut every piece of this place is mined. You should be very careful.", Serach());
        return mesData;
    }

    private List<AnswerDialogData> Serach()
    {
        var ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData("Start search at other side", null,  bothSearch),
            new AnswerDialogData("Come closer.", null,   comeClose),
            new AnswerDialogData("Try attack.", null,   tryAttack),
            new AnswerDialogData(Namings.leave, null,   null),
        };
        return ans;
    }

    private MessageDialogData tryAttack()
    {
        MessageDialogData mesData;
        List<AnswerDialogData> ans;
        var army = MainController.Instance.MainPlayer.Army;
        var speeddShip = army.FirstOrDefault(x => ShipParameters.ParamUpdate(x.Ship.MaxSpeed, x.Pilot.SpeedLevel, ShipParameters.MaxSpeedCoef) > speedToWin);
        if (speeddShip != null)
        {
            ans = new List<AnswerDialogData>()
            {
                new AnswerDialogData(Namings.Ok, null,  null),
            };
            string names = "";
            for (int i = 0; i < MyExtensions.Random(1,2); i++)
            {
                var m = Library.CreatSimpleModul(MyExtensions.Random(1, 2), SkillWork(4, ScoutsLevel));
                var itemName = Namings.SimpleModulName(m.Type);
                var canAdd = MainController.Instance.MainPlayer.Inventory.GetFreeSimpleSlot(out var slot);
                if (canAdd)
                {
                    MainController.Instance.MainPlayer.Inventory.TryAddSimpleModul(m, slot);
                    names = $"{names} {itemName} ";
                }
            }
            mesData = new MessageDialogData(String.Format("They blow up almost all. But your fast ship safe some modules:{0}", names), ans);
            return mesData;
        }
        else
        {
            ans = new List<AnswerDialogData>()
            {
                new AnswerDialogData(Namings.leave, null,  null),
            };
            mesData = new MessageDialogData(String.Format("They blow up all and run away.", moneyTotal), ans);
            return mesData;
        }
    }

    private MessageDialogData comeClose()
    {
        MessageDialogData mesData;
        List<AnswerDialogData> ans;
        ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData("Threaten", null,  Threaten),
            new AnswerDialogData("Offer assistance", null,  assistance),
            new AnswerDialogData(Namings.leave, null,  null),
        };
        mesData = new MessageDialogData(String.Format("They are searching for something", moneyTotal), ans);
        return mesData;
    }

    private MessageDialogData Threaten()
    {
        MessageDialogData mesData;
        List<AnswerDialogData> ans;
        ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.Ok, null,  Threaten),
        };
        BrokeShipWithRandom();
        mesData = new MessageDialogData(String.Format("They have nothing. And simply blow up everything. Some of your ships damaged.", moneyTotal), ans);
        return mesData;     
    }

    private MessageDialogData assistance()
    {
        MessageDialogData mesData;
        List<AnswerDialogData> ans;
        ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.Ok, null,  assistanceFine),
            new AnswerDialogData("No", null,  comeClose),
            new AnswerDialogData(Namings.leave, null,  null),
        };
        mesData = new MessageDialogData(String.Format("Ok, lets go 50/50", moneyTotal), ans);
        return mesData;
    }
    private void BrokeShipWithRandom()
    {
        var player = MainController.Instance.MainPlayer;
        foreach (var data in player.Army)
        {
            if (MyExtensions.IsTrueEqual())
            {
                var per = data.Ship.HealthPercent;
                data.Ship.SetRepairPercent(per * 0.8f);
            }
        }
    }

    private MessageDialogData assistanceFine()
    {
        MessageDialogData mesData;
        List<AnswerDialogData> ans;
        ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.Ok, null,  null),
        };
        if (SkillWork(Nothing, excvScounts + ScoutsLevel))
        {
            var m = moneyTotal / 2;
            mesData = new MessageDialogData(String.Format("All fine, you found some credits. Your part:{0}.", m), ans);
            MainController.Instance.MainPlayer.MoneyData.AddMoney(m);
        }
        else
        {
            mesData = new MessageDialogData(String.Format("Nothing. This isn't your day", moneyTotal), ans);
        }

        return mesData;
    }

    private MessageDialogData bothSearch()
    {
        MessageDialogData mesData;
        List<AnswerDialogData> ans;
        WDictionary<int> testResult = new WDictionary<int>(new Dictionary<int, float>()
        {
            { 0,ScoutsLevel},   
              {1,Nothing},
              {2,excvScounts},
        });
        var r = testResult.Random();
        switch (r)
        {
            case 0:   //Я вин
                ans = new List<AnswerDialogData>()
                {
                    new AnswerDialogData(Namings.Ok, null,  null),
                };
                mesData = new MessageDialogData(String.Format("You found a lot of credits {0}", moneyTotal), ans);
                return mesData;
            
            case 2:   //Они вин     
                ans = new List<AnswerDialogData>()
                {
                    new AnswerDialogData(Namings.Ok, null,  null),
                };
                mesData = new MessageDialogData(String.Format("They found credits first {0}", moneyTotal), ans);
                return mesData;
        }
        mesData = new MessageDialogData("No one can't find anything", Serach());
        return mesData;

    }
}

