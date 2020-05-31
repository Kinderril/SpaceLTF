using System;
using System.Collections.Generic;


[System.Serializable]
public abstract class BaseGlobalMapEvent
{
    protected int _power;
    public abstract string Desc();
    public abstract MessageDialogData GetDialog();

    protected ShipConfig _config;
    protected PlayerReputationData _reputation;

    protected BaseGlobalMapEvent(ShipConfig config)
    {
        _config = config;
        _reputation = MainController.Instance.MainPlayer.ReputationData;
    }

    public virtual void Init()
    {

    }

    public virtual MessageDialogData GetLeavedActionInner()
    {
        return null;
    }

    protected StartShipPilotData HireAction(int itemsCount = 1)
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
        var type = types.Random();
        var cng = configs.Random();
        return HireAction(itemsCount, cng, type, 1);
    }

    protected StartShipPilotData HireAction(int itemsCount, ShipConfig congif, ShipType shipType, int level)
    {
        var type = shipType;
        var cng = congif;
        var pilot = Library.CreateDebugPilot();
        var ship = Library.CreateShip(type, cng, MainController.Instance.MainPlayer, pilot);
        WindowManager.Instance.InfoWindow.Init(null, Namings.Format("You hired a new pilot. Type:{0}  Config:{1}",
            Namings.ShipConfig(cng), Namings.ShipType(type)));
        var data = new StartShipPilotData(pilot, ship);
        data.Ship.SetRepairPercent(0.1f);
        for (int i = 0; i < itemsCount; i++)
        {
            if (data.Ship.GetFreeWeaponSlot(out var inex))
            {
                var weapon = Library.CreateDamageWeapon(true);
                data.Ship.TryAddWeaponModul(weapon, inex);
            }
        }

        if (level > 1)
        {
            var upgs = ArmyCreator.PosiblePilotUpgrades(pilot);
            for (int i = 0; i < level; i++)
            {
                data.Pilot.UpgradeLevelByType(upgs.RandomElement(), false);
            }
        }
        MainController.Instance.MainPlayer.Army.TryHireShip(data);
        return data;
    }

    protected bool SkillWork(int baseVal, int skillVal)
    {
        WDictionary<bool> wd = new WDictionary<bool>(new Dictionary<bool, float>()
        {
            {true,skillVal },
            {false,baseVal},
        });
        return wd.Random();
    }

    protected Player GetArmy(ShipConfig config, ShipConfig config2, float power)
    {
        ArmyCreatorData data = ArmyCreatorLibrary.GetArmy(config, config2);
        return GetArmy(data, power);
    }
    protected Player GetArmy(ShipConfig config, float power)
    {
        ArmyCreatorData data = ArmyCreatorLibrary.GetArmy(config);
        return GetArmy(data, power);
    }

    private Player GetArmy(ArmyCreatorData data, float power)
    {

        var player = new PlayerAI("Army");
        float coreShipChance = 0;
        if (power > 15)
        {
            switch (data.ArmyConfig)
            {
                case ShipConfig.raiders:
                    coreShipChance = 0.3f;
                    break;
                case ShipConfig.federation:
                    coreShipChance = 0.5f;
                    break;
                case ShipConfig.mercenary:
                    coreShipChance = 0.5f;
                    break;
                case ShipConfig.ocrons:
                    coreShipChance = 0.5f;
                    break;
                case ShipConfig.krios:
                    coreShipChance = 0.5f;
                    break;
            }
        }
        bool withCore = MyExtensions.IsTrue01(coreShipChance);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 1, 3, data, withCore, player);
        player.Army.SetArmy(army);
        return player;
    }
    protected MessageDialogData moneyResult(int min, int max)
    {
        var ans = new List<AnswerDialogData>()
        {
            new     AnswerDialogData(Namings.Tag("Ok"))
        };
        int money = (int)(MyExtensions.Random(min, max) * Library.MONEY_QUEST_COEF);
        MainController.Instance.MainPlayer.MoneyData.AddMoney(money) ;
        var mesData = new MessageDialogData($"Credits add {money}.", ans);
        return mesData;
    }

    protected MessageDialogData failResult()
    {
        var ans = new List<AnswerDialogData>()
        {
            new     AnswerDialogData(Namings.Tag("Ok"))
        };
        _reputation.AddReputation(_config, 1);
        var mesData = new MessageDialogData("Action fail.", ans);
        return mesData;
    }


    protected MessageDialogData ShowFail(string msg, Action callback = null)
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("Ok")), null));
        var mesData = new MessageDialogData(msg, mianAnswers);
        return mesData;
    }

    // protected int ReputationLevel { get { return MainController.Instance.MainPlayer.ReputationData.Reputation; } }
    protected int ScoutsLevel { get { return MainController.Instance.MainPlayer.Parameters.Scouts.Level; } }
    // protected int DiplomacyLevel { get { return MainController.Instance.MainPlayer.Parameters.Diplomaty.Level; } }
    protected int RepairLevel { get { return MainController.Instance.MainPlayer.Parameters.Repair.Level; } }
    protected int ChargesCountLevel { get { return MainController.Instance.MainPlayer.Parameters.ChargesCount.Level; } }

    public virtual bool OneTimeUsed()
    {
        return true;
    }
}

