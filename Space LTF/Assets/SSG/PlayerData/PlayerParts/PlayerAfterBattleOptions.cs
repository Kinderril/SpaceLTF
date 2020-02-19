using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class PlayerAfterBattleOptions
{
    private int _lastStepGetDialog;
    private const int DialogFrequancy = 3;


    public PlayerAfterBattleOptions()
    {

    }

    public MessageDialogData GetDialog(int step, float cellPower, ShipConfig config)
    {
        var delta = step - _lastStepGetDialog;
        if (SkillWork(DialogFrequancy, delta))
        {
            _lastStepGetDialog = step;
            var ans = InitPosibleAnswers(cellPower, config);
            ans.Add(new AnswerDialogData(Namings.Tag("leave")));
            var msg = Namings.Format(Namings.DialogTag("afterBattleStart"));
            var dialog = new MessageDialogData(msg, ans);
            return dialog;
        }
        return null;
    }

    private List<AnswerDialogData> InitPosibleAnswers(float power, ShipConfig config)
    {
        var list = new List<AnswerDialogData>();
        list.Add(new AnswerDialogData(Namings.DialogTag("afterBattleBuyout"), null, () => Buyout(power, config)));
        list.Add(new AnswerDialogData(Namings.DialogTag("afterBattleTeachPilots"), null, () => TeachPilots()));
        list.Add(new AnswerDialogData(Namings.DialogTag("afterBattleUpgradeSpell"), null, () => UpgradeSpell()));
        list.Add(new AnswerDialogData(Namings.DialogTag("afterBattleOpenCells"), null, () => OpenCells()));
        list.Add(new AnswerDialogData(Namings.DialogTag("afterBattleHireAction"), null, () => HireAction(config)));
        list.Add(new AnswerDialogData(Namings.DialogTag("afterBattleSearchFor"), null, () => SearchFor(power, config)));
        list.Add(new AnswerDialogData(Namings.DialogTag("afterBattleKillAction"), null, () => KillAction(power, config)));
        list.Add(new AnswerDialogData(Namings.DialogTag("afterBattleRepairAction"), null, () => RepairAction(power, config)));
        var answers = list.RandomElement(3); ;
        return answers;
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


    #region RandomActions       
    private MessageDialogData TeachPilots()
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        var player = MainController.Instance.MainPlayer;
        var c = player.Army.Army.Count;
        var pilotsToTeach = player.Army.Army.RandomElement(MyExtensions.Random(1, c));
        foreach (var shipPilotData in pilotsToTeach)
        {
            if (shipPilotData.Ship.ShipType != ShipType.Base)
            {
                shipPilotData.Pilot.UpgradeRandomLevel(false, true);
            }
        }

        msg = Namings.Format(Namings.DialogTag("afterBattleTeachOk"), pilotsToTeach.Count);
        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }
    private MessageDialogData UpgradeSpell()
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        var ship =
            MainController.Instance.MainPlayer.Army.Army.FirstOrDefault(x => x.Ship.ShipType == ShipType.Base);
        if (ship != null)
        {
            var spell = ship.Ship.SpellsModuls.Where(x => x != null).ToList().RandomElement();
            if (spell.Upgrade(ESpellUpgradeType.None))
            {
                msg = Namings.Format(Namings.DialogTag("afterBattleUpgradeSpellOk"), spell.Name);
                return new MessageDialogData(msg, ans);
            }
        }

        msg = Namings.Format(Namings.DialogTag("afterBattleUpgradeSpellFail"));
        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }
    private MessageDialogData OpenCells()
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        var player = MainController.Instance.MainPlayer;
        var sectorID = player.MapData.CurrentCell.SectorId;
        var sctor = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.Id == sectorID);
        if (sctor != null)
        {
            for (int i = 0; i < sctor.Size; i++)
            {
                for (int j = 0; j < sctor.Size; j++)
                {
                    var cell = sctor.Cells[i, j];
                    if (cell.Data != null)
                    {
                        cell.Data.Scouted();
                    }
                }
            }
        }
        msg = Namings.DialogTag("afterBattleCellOpen");
        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }
    private MessageDialogData Buyout(float power, ShipConfig config)
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        var rep = MainController.Instance.MainPlayer.ReputationData.ReputationFaction[config];
        if (SkillWork(15, rep))
        {
            var scouts = MainController.Instance.MainPlayer.Parameters.Scouts.Level;
            var delta = MoneyConsts.MAX_PASSIVE_LEVEL - scouts;
            var coef = (float)power * Library.MONEY_QUEST_COEF;
            int monet = (int)(MyExtensions.Random(delta * 3, delta * 5) * coef);
            MainController.Instance.MainPlayer.MoneyData.AddMoney(monet);

            msg = Namings.Format(Namings.DialogTag("afterBattleBuyoutConfirm"), monet);
        }
        else
        {
            msg = Namings.Format(Namings.DialogTag("afterBattleBuyoutFail"));
        }

        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }
    private MessageDialogData SearchFor(float power, ShipConfig config)
    {
        WDictionary<bool> ws = new WDictionary<bool>(new Dictionary<bool, float>()
        {
            {true, MainController.Instance.MainPlayer.Parameters.Scouts.Level}, {false, 2},
        });
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        if (ws.Random())
        {
            var scouts = MainController.Instance.MainPlayer.Parameters.Scouts.Level;
            var coef = (float)power * Library.MONEY_QUEST_COEF;
            int monet = (int)(MyExtensions.Random(scouts * 3, scouts * 5) * coef);
            MainController.Instance.MainPlayer.MoneyData.AddMoney(monet);
            msg = Namings.Format(Namings.DialogTag("afterBattleSearchOk"), monet);        //"Credits add: {0}."
            MainController.Instance.MainPlayer.ReputationData.RemoveReputation(config, Library.REPUTATION_STEAL_REMOVE);
        }
        else
        {
            msg = Namings.Format(Namings.DialogTag("afterBattleSearchFail"));//
        }
        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }
    private MessageDialogData KillAction(float power, ShipConfig config)
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        var shallWork = true;//SkillWork(1, MainController.Instance.MainPlayer.Parameters.Scouts.Level);
        if (shallWork)
        {
            MainController.Instance.MainPlayer.ReputationData.RemoveReputation(config, 8);
            var coef = (float)power * Library.MONEY_QUEST_COEF;
            int monet = (int)(MyExtensions.Random(20, 30) * coef);
            MainController.Instance.MainPlayer.MoneyData.AddMoney(monet);
            msg = Namings.Format(Namings.DialogTag("afterBattleKillOk"), monet);//
        }
        else
        {
            MainController.Instance.MainPlayer.ReputationData.RemoveReputation(config, 16);
            msg = Namings.DialogTag("afterBattleKillFail");//
        }
        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }
    private MessageDialogData RepairAction(float power, ShipConfig config)
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        var shallWork = SkillWork(2, MainController.Instance.MainPlayer.Parameters.Repair.Level);
        if (shallWork)
        {
            MainController.Instance.MainPlayer.ReputationData.RemoveReputation(config, 15);
            msg = Namings.Format(Namings.DialogTag("afterBattleRepairOk"));
        }
        else
        {
            msg = Namings.DialogTag("afterBattleRepairFail");
        }
        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }
    private MessageDialogData HireAction(ShipConfig? config = null)
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        var pilot = Library.CreateDebugPilot();
        WDictionary<ShipType> types = new WDictionary<ShipType>(new Dictionary<ShipType, float>()
        {
            {ShipType.Heavy, 2}, {ShipType.Light, 2}, {ShipType.Middle, 2},
        });

        var configsD = new Dictionary<ShipConfig, float>();
        switch (config)
        {
            case ShipConfig.raiders:
                configsD.Add(ShipConfig.mercenary, 1);
                configsD.Add(ShipConfig.raiders, 2);
                break;
            case ShipConfig.mercenary:
                configsD.Add(ShipConfig.raiders, 1);
                configsD.Add(ShipConfig.mercenary, 5);
                break;
            case ShipConfig.federation:
                configsD.Add(ShipConfig.mercenary, 2);
                configsD.Add(ShipConfig.krios, 2);
                configsD.Add(ShipConfig.ocrons, 2);
                break;
            case ShipConfig.ocrons:
                configsD.Add(ShipConfig.federation, 2);
                configsD.Add(ShipConfig.krios, 2);
                break;
            case ShipConfig.krios:
                configsD.Add(ShipConfig.federation, 2);
                configsD.Add(ShipConfig.ocrons, 2);
                break;
        }

        WDictionary<ShipConfig> configs = new WDictionary<ShipConfig>(configsD);

        var type = types.Random();
        var cng = config.HasValue ? config.Value : configs.Random();
        var ship = Library.CreateShip(type, cng, MainController.Instance.MainPlayer, pilot);
        var hireMsg = Namings.DialogTag("afterBattleHireOk");//
        msg = Namings.Format(hireMsg, Namings.ShipConfig(cng), Namings.ShipType(type));
        int itemsCount = MyExtensions.Random(1, 2);
        for (int i = 0; i < itemsCount; i++)
        {
            if (ship.GetFreeWeaponSlot(out var inex))
            {
                var weapon = Library.CreateWeapon(true);
                ship.TryAddWeaponModul(weapon, inex);
            }
        }
        MainController.Instance.MainPlayer.Army.TryHireShip(new StartShipPilotData(pilot, ship));

        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }

    #endregion


}
