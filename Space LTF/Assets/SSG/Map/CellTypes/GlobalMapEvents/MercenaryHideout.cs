
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
public class MercsConfig
{
    public ShipConfig Congif;
    public ShipType Type;
    public int PilotLevel;
    private int _cost;

    public MercsConfig(
        ShipConfig Congif,
        ShipType Type,
        int PilotLevel
        )
    {
        this.Congif = Congif;
        this.Type = Type;
        this.PilotLevel = PilotLevel;
        int cost = 10;
        switch (Congif)
        {
            case ShipConfig.raiders:
                cost = 8;
                break;
            case ShipConfig.federation:
                cost = 18;
                break;
            case ShipConfig.mercenary:
                cost = 9;
                break;
            case ShipConfig.ocrons:
                cost = 27;
                break;
            case ShipConfig.krios:
                cost = 22;
                break;
            case ShipConfig.droid:
                cost = 25;
                break;
        }
        _cost = PilotLevel * 3 + cost;
    }

    public int Cost => _cost;
}

[System.Serializable]
public class MercenaryHideout : BaseGlobalMapEvent
{

    private int creditsToEnter = 10;
    private List<MercsConfig> _mercsConfigs = null;
    
    public MercenaryHideout(ShipConfig config)
        : base(config)
    {
    }
    public override string Desc()
    {
        return Namings.MercGlobal; 
    }

    public override MessageDialogData GetDialog()
    {
        if (_mercsConfigs == null)
        {
            GenerateMercs();
        }
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData("Pay",null,TryPay));
        mianAnswers.Add(new AnswerDialogData(Namings.leave, null, null));

        MessageDialogData mesData = new MessageDialogData($"Space base inside big asteroid. Have impressive protection. \nEnter not free. Pay {creditsToEnter} to enter", mianAnswers);
        return mesData;
    }

    private void GenerateMercs()
    {
        WDictionary<ShipType> types = new WDictionary<ShipType>(new Dictionary<ShipType, float>()
        {
            {ShipType.Heavy, 2 },
            {ShipType.Light, 2 },
            {ShipType.Middle, 2 },
        });
        var configsD = new Dictionary<ShipConfig, float>();
        configsD.Add(ShipConfig.krios, 3);
        configsD.Add(ShipConfig.raiders, 5);
        configsD.Add(ShipConfig.ocrons, 3);
        configsD.Add(ShipConfig.federation, 2);
        configsD.Add(ShipConfig.mercenary, 5);
        WDictionary<ShipConfig> configs = new WDictionary<ShipConfig>(configsD);
        var maxLevel = MainController.Instance.MainPlayer.Army.Max(x => x.Pilot.CurLevel);
        _mercsConfigs = new List<MercsConfig>();
        for (int i = 0; i < 4; i++)
        {
            var type = types.Random();
            var cng = configs.Random();
            var merc = new MercsConfig(cng, type, MyExtensions.Random(1, maxLevel + 3));
            _mercsConfigs.Add(merc);
        }

    }

    private MessageDialogData TryPay()
    {
        MessageDialogData mesData;
        var mianAnswers = new List<AnswerDialogData>();
        var player = MainController.Instance.MainPlayer;
        if (!player.MoneyData.HaveMoney(creditsToEnter))
        {
            mianAnswers.Add(new AnswerDialogData(Namings.Ok));
            mesData = new MessageDialogData(Namings.NotEnoughtMoney, mianAnswers);
        }
        else
        {
            return MercToHire();
        }
        return mesData;
    }

    private MessageDialogData MercToHire()
    {
        var mianAnswers = new List<AnswerDialogData>();
        if (MainController.Instance.MainPlayer.CanAddShip())
        {                                                                                

            foreach (var mercsConfig in _mercsConfigs)
            {
                var ship = mercsConfig;
                var str = String.Format(Namings.HireMerc, Namings.ShipConfig(mercsConfig.Congif),
                    Namings.ShipType(mercsConfig.Type), mercsConfig.PilotLevel, mercsConfig.Cost);

                MessageDialogData HireShip()
                {
                    return TryHire(ship);
                }
                mianAnswers.Add(new AnswerDialogData(str, null, HireShip));
            }
            mianAnswers.Add(new AnswerDialogData(Namings.leave));
            var mesData = new MessageDialogData("You can try to hire somebody here.", mianAnswers);
            return mesData;
        }
        else
        {
            mianAnswers.Add(new AnswerDialogData(Namings.leave));
            var mesData = new MessageDialogData("You have maximum size of fleet.", mianAnswers);
            return mesData;

        }
    }

    private MessageDialogData TryHire(MercsConfig ship)
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.Ok,null, MercToHire));
        if (MainController.Instance.MainPlayer.MoneyData.HaveMoney(ship.Cost))
        {
            MainController.Instance.MainPlayer.MoneyData.RemoveMoney(ship.Cost);
            HireAction(1, ship.Congif, ship.Type, ship.PilotLevel);
            _mercsConfigs.Remove(ship);
            MessageDialogData mesData = new MessageDialogData("Hire complete", mianAnswers);
            return mesData;
        }
        else
        {
            MessageDialogData mesData = new MessageDialogData(Namings.NotEnoughtMoney, mianAnswers);
            return mesData;
        }


    }

}

