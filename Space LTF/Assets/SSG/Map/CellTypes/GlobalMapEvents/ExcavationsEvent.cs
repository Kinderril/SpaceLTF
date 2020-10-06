
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
        return Namings.Tag("Excavations");
    }

    public override MessageDialogData GetDialog()
    {
        var ss = Namings.Format(Namings.Tag("ExcavationStart"));
        var mesData = new MessageDialogData(ss, Serach());
        return mesData;
    }

    private List<AnswerDialogData> Serach()
    {
        var ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.Tag("Start search at other side"), null,  bothSearch),
            new AnswerDialogData(Namings.Tag("Come closer."), null,   comeClose),
            new AnswerDialogData(Namings.Tag("Try attack."), null,   tryAttack),
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
            mesData = new MessageDialogData(Namings.Format(Namings.Tag("BlowUpAlmost"), names), ans);
            return mesData;
        }
        else
        {
            ans = new List<AnswerDialogData>()
            {
                new AnswerDialogData(Namings.Tag("leave"), null,  null),
            };
            mesData = new MessageDialogData(Namings.Format(Namings.Tag("They blow up"), moneyTotal), ans);
            return mesData;
        }
    }

    private MessageDialogData comeClose()
    {
        MessageDialogData mesData;
        List<AnswerDialogData> ans;
        ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.Tag("Threaten"), null,  Threaten),
            new AnswerDialogData(Namings.Tag("Offer assistance"), null,  assistance),
            new AnswerDialogData(Namings.Tag("leave"), null,  null),
        };
        mesData = new MessageDialogData(Namings.Format(Namings.Tag("TheySearching"), moneyTotal), ans);
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
//        _reputation.RemoveReputation(_config, 5);
        mesData = new MessageDialogData(Namings.Format(Namings.Tag("TheyHaveNothing"), moneyTotal), ans);
        return mesData;
    }

    private MessageDialogData assistance()
    {
        MessageDialogData mesData;
        List<AnswerDialogData> ans;
        ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.Tag("Ok"), null,  assistanceFine),
            new AnswerDialogData(Namings.Tag("No"), null,  comeClose),
            new AnswerDialogData(Namings.Tag("leave"), null,  null),
        };
        mesData = new MessageDialogData(Namings.Format( Namings.Tag("OKgo5050"), moneyTotal), ans);
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
            var player = MainController.Instance.MainPlayer;
            var m = (int)((moneyTotal / 2f) * player.SafeLinks.CreditsCoef);
//            _reputation.AddReputation(_config, 5);
            mesData = new MessageDialogData(Namings.Format(Namings.Tag("YourRartMoney"), m), ans);
            player.MoneyData.AddMoney(m);
        }
        else
        {
            mesData = new MessageDialogData(Namings.Format(Namings.Tag("NotYourDayNothung"), moneyTotal), ans);
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
                mesData = new MessageDialogData(Namings.Format(Namings.Tag("foundLotCredits"), moneyTotal), ans);
                return mesData;

            case 2:   //Они вин     
                ans = new List<AnswerDialogData>()
                {
                    new AnswerDialogData(Namings.Tag("Ok"), null,  null),
                };
                mesData = new MessageDialogData(Namings.Format( Namings.Tag("foundLotCreditsThey"), moneyTotal), ans);
                return mesData;
        }
        mesData = new MessageDialogData(Namings.Tag("nobodyFind"), Serach());
        return mesData;

    }
}

