
using System;
using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class ExcavationsEvent : BaseGlobalMapEvent
{
    private int weaponTryies = 0;
    private const float speedToWin = 4f;
    private const int excvScounts = 3;
    private const int Nothing = 3;
    private int moneyTotal = 100;

    public ExcavationsEvent(int power, ShipConfig config)
        : base(config)
    {
        _power = power;
    }
    public override void Init()
    {
        var coef = (float)_power * Library.MONEY_QUEST_COEF;
        moneyTotal = (int)(MyExtensions.Random(34, 55) * coef);
        base.Init();
    }

    public override string Desc()
    {
        return "Excavations";
    }

    public override MessageDialogData GetDialog()
    {
        var mesData = new MessageDialogData(Namings.Format("You see excavations. Small fleet searching something at this place.\nBut every piece of this place is mined. You should be very careful."), Serach());
        return mesData;
    }

    private List<AnswerDialogData> Serach()
    {
        var ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData("Start search at other side", null,  bothSearch),
            new AnswerDialogData("Come closer.", null,   comeClose),
            new AnswerDialogData("Try attack.", null,   tryAttack),
            new AnswerDialogData(Namings.Tag("leave"), null,   null),
        };
        return ans;
    }

    private MessageDialogData tryAttack()
    {
        MessageDialogData mesData;
        List<AnswerDialogData> ans;
        var army = MainController.Instance.MainPlayer.Army;
        var speeddShip = army.Army.FirstOrDefault(x => ShipParameters.ParamUpdate(x.Ship.MaxSpeed, x.Pilot.SpeedLevel, ShipParameters.MaxSpeedCoef) > speedToWin);
        if (speeddShip != null)
        {
            ans = new List<AnswerDialogData>()
            {
                new AnswerDialogData(Namings.Tag("Ok"), null,  null),
            };
            string names = "";
            for (int i = 0; i < MyExtensions.Random(1, 2); i++)
            {
                var m = Library.CreatSimpleModul(MyExtensions.Random(1, 2), 3);
                var itemName = Namings.SimpleModulName(m.Type);
                var canAdd = MainController.Instance.MainPlayer.Inventory.GetFreeSimpleSlot(out var slot);
                if (canAdd)
                {
                    MainController.Instance.MainPlayer.Inventory.TryAddSimpleModul(m, slot);
                    names = $"{names} {itemName} ";
                }
            }
            mesData = new MessageDialogData(Namings.Format("They blow up almost all. But your fast ship safe some modules:{0}", names), ans);
            return mesData;
        }
        else
        {
            ans = new List<AnswerDialogData>()
            {
                new AnswerDialogData(Namings.Tag("leave"), null,  null),
            };
            mesData = new MessageDialogData(Namings.Format("They blow up all and run away.", moneyTotal), ans);
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
            new AnswerDialogData(Namings.Tag("leave"), null,  null),
        };
        mesData = new MessageDialogData(Namings.Format("They are searching for something", moneyTotal), ans);
        return mesData;
    }

    private MessageDialogData Threaten()
    {
        MessageDialogData mesData;
        List<AnswerDialogData> ans;
        ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.Tag("Ok"), null,  Threaten),
        };
        BrokeShipWithRandom();
        _reputation.RemoveReputation(_config, 5);
        mesData = new MessageDialogData(Namings.Format("They have nothing. And simply blow up everything. Some of your ships damaged.", moneyTotal), ans);
        return mesData;
    }

    private MessageDialogData assistance()
    {
        MessageDialogData mesData;
        List<AnswerDialogData> ans;
        ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.Tag("Ok"), null,  assistanceFine),
            new AnswerDialogData("No", null,  comeClose),
            new AnswerDialogData(Namings.Tag("leave"), null,  null),
        };
        mesData = new MessageDialogData(Namings.Format("Ok, lets go 50/50", moneyTotal), ans);
        return mesData;
    }
    private void BrokeShipWithRandom()
    {
        var player = MainController.Instance.MainPlayer;
        foreach (var data in player.Army.Army)
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
            new AnswerDialogData(Namings.Tag("Ok"), null,  null),
        };
        if (SkillWork(Nothing, excvScounts + ScoutsLevel))
        {
            var m = moneyTotal / 2;
            _reputation.AddReputation(_config, 5);
            mesData = new MessageDialogData(Namings.Format("All fine, you found some credits. Your part:{0}.", m), ans);
            MainController.Instance.MainPlayer.MoneyData.AddMoney(m);
        }
        else
        {
            mesData = new MessageDialogData(Namings.Format("Nothing. This isn't your day", moneyTotal), ans);
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
                    new AnswerDialogData(Namings.Tag("Ok"), null,  null),
                };
                mesData = new MessageDialogData(Namings.Format("You found a lot of credits {0}", moneyTotal), ans);
                return mesData;

            case 2:   //Они вин     
                ans = new List<AnswerDialogData>()
                {
                    new AnswerDialogData(Namings.Tag("Ok"), null,  null),
                };
                mesData = new MessageDialogData(Namings.Format("They found credits first {0}", moneyTotal), ans);
                return mesData;
        }
        mesData = new MessageDialogData("No one can't find anything", Serach());
        return mesData;

    }
}

