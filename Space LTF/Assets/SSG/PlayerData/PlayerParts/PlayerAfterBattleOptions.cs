using System;
using System.Collections.Generic;
using System.Linq;

public enum EAfterBattleAnswers
{
    afterBattleBuyout,
    afterBattleTeachPilots,
    afterBattleUpgradeSpell,
    afterBattleOpenCells,
    afterBattleHireAction,
    afterBattleSearchFor,
    afterBattleKillAction,
    afterBattleRepairAction
}


[Serializable]
public class PlayerAfterBattleOptions
{
    private const int START_MAX_COUNT = 10000;
    private const int DialogFrequancy = 3;

    private Dictionary<EAfterBattleAnswers, int> _answersCount = new Dictionary<EAfterBattleAnswers, int>
    {
        {EAfterBattleAnswers.afterBattleBuyout, START_MAX_COUNT},
        {EAfterBattleAnswers.afterBattleTeachPilots, START_MAX_COUNT},
        {EAfterBattleAnswers.afterBattleUpgradeSpell, START_MAX_COUNT},
        {EAfterBattleAnswers.afterBattleOpenCells, START_MAX_COUNT},
        {EAfterBattleAnswers.afterBattleHireAction, START_MAX_COUNT},
        {EAfterBattleAnswers.afterBattleSearchFor, START_MAX_COUNT},
        {EAfterBattleAnswers.afterBattleKillAction, START_MAX_COUNT},
        {EAfterBattleAnswers.afterBattleRepairAction, START_MAX_COUNT}
    };

    private int _lastStepGetDialog;


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
        var preList = new List<EAfterBattleAnswers>();
        var maxData = _answersCount.Values.Max();
        var curVal = maxData;
        int iterations = 0;
        while (preList.Count < 3 || curVal >= 0 || iterations < 10)
        {
            foreach (var answer in _answersCount)
                if (answer.Value == curVal)
                    preList.Add(answer.Key);

            iterations++;
            curVal--;
        }

        var list = new List<AnswerDialogData>();
        if (preList.Count == 0)
        {
            preList.Add(EAfterBattleAnswers.afterBattleBuyout);
            preList.Add(EAfterBattleAnswers.afterBattleTeachPilots);
            preList.Add(EAfterBattleAnswers.afterBattleUpgradeSpell);
            preList.Add(EAfterBattleAnswers.afterBattleOpenCells);
            preList.Add(EAfterBattleAnswers.afterBattleHireAction);
            preList.Add(EAfterBattleAnswers.afterBattleKillAction);
            preList.Add(EAfterBattleAnswers.afterBattleRepairAction);
            preList.Add(EAfterBattleAnswers.afterBattleSearchFor);
        }
        foreach (var answerse in preList)
        {
            var answer = GetAnswer(answerse, power, config);
            if (answer != null)
                list.Add(answer);
        }
        var answers = list.RandomElement(3);
        return answers;
    }

    private AnswerDialogData GetAnswer(EAfterBattleAnswers type, float power, ShipConfig config)
    {
        void IncCallback()
        {
            _answersCount[type]--;
        }

        switch (type)
        {
            case EAfterBattleAnswers.afterBattleBuyout:
                return new AnswerDialogData(Namings.DialogTag("afterBattleBuyout"), IncCallback,
                    () => Buyout(power, config));
            case EAfterBattleAnswers.afterBattleTeachPilots:
                return new AnswerDialogData(Namings.DialogTag("afterBattleTeachPilots"), IncCallback,
                    () => TeachPilots());
            case EAfterBattleAnswers.afterBattleUpgradeSpell:
                return new AnswerDialogData(Namings.DialogTag("afterBattleUpgradeSpell"), IncCallback,
                    () => UpgradeSpell());
            case EAfterBattleAnswers.afterBattleOpenCells:
                return new AnswerDialogData(Namings.DialogTag("afterBattleOpenCells"), IncCallback, () => OpenCells());
            case EAfterBattleAnswers.afterBattleHireAction:
                return new AnswerDialogData(Namings.DialogTag("afterBattleHireAction"), IncCallback,
                    () => HireAction(config));
            case EAfterBattleAnswers.afterBattleSearchFor:
                return new AnswerDialogData(Namings.DialogTag("afterBattleSearchFor"), IncCallback,
                    () => SearchFor(power, config));
            case EAfterBattleAnswers.afterBattleKillAction:
                return new AnswerDialogData(Namings.DialogTag("afterBattleKillAction"), IncCallback,
                    () => KillAction(power, config));
            case EAfterBattleAnswers.afterBattleRepairAction:
                return new AnswerDialogData(Namings.DialogTag("afterBattleRepairAction"), IncCallback,
                    () => RepairAction(power, config));
            default:
                return null;
        }
    }

    protected bool SkillWork(int baseVal, int skillVal)
    {
        var wd = new WDictionary<bool>(new Dictionary<bool, float>
        {
            {true, skillVal},
            {false, baseVal}
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
            if (shipPilotData.Ship.ShipType != ShipType.Base)
                shipPilotData.Pilot.UpgradeRandomLevel(false, true);

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
            for (var i = 0; i < sctor.Size; i++)
                for (var j = 0; j < sctor.Size; j++)
                {
                    var cell = sctor.Cells[i, j];
                    if (cell.Data != null) cell.Data.Scouted();
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
            var coef = power * Library.MONEY_QUEST_COEF;
            var monet = (int)(MyExtensions.Random(delta * 3, delta * 5) * coef);
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
        var ws = new WDictionary<bool>(new Dictionary<bool, float>
        {
            {true, MainController.Instance.MainPlayer.Parameters.Scouts.Level}, {false, 2}
        });
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        if (ws.Random())
        {
            var scouts = MainController.Instance.MainPlayer.Parameters.Scouts.Level;
            var coef = power * Library.MONEY_QUEST_COEF;
            var monet = (int)(MyExtensions.Random(scouts * 3, scouts * 5) * coef);
            MainController.Instance.MainPlayer.MoneyData.AddMoney(monet);
            msg = Namings.Format(Namings.DialogTag("afterBattleSearchOk"), monet); //"Credits add: {0}."
            MainController.Instance.MainPlayer.ReputationData.RemoveReputation(config, Library.REPUTATION_STEAL_REMOVE);
        }
        else
        {
            msg = Namings.Format(Namings.DialogTag("afterBattleSearchFail")); //
        }

        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }

    private MessageDialogData KillAction(float power, ShipConfig config)
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        var shallWork = true; //SkillWork(1, MainController.Instance.MainPlayer.Parameters.Scouts.Level);
        if (shallWork)
        {
            MainController.Instance.MainPlayer.ReputationData.RemoveReputation(config, 8);
            var coef = power * Library.MONEY_QUEST_COEF;
            var monet = (int)(MyExtensions.Random(20, 30) * coef);
            MainController.Instance.MainPlayer.MoneyData.AddMoney(monet);
            msg = Namings.Format(Namings.DialogTag("afterBattleKillOk"), monet); //
        }
        else
        {
            MainController.Instance.MainPlayer.ReputationData.RemoveReputation(config, 16);
            msg = Namings.DialogTag("afterBattleKillFail"); //
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
        var types = new WDictionary<ShipType>(new Dictionary<ShipType, float>
        {
            {ShipType.Heavy, 2}, {ShipType.Light, 2}, {ShipType.Middle, 2}
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

        var configs = new WDictionary<ShipConfig>(configsD);

        var type = types.Random();
        var cng = config.HasValue ? config.Value : configs.Random();
        var ship = Library.CreateShip(type, cng, MainController.Instance.MainPlayer, pilot);
        var hireMsg = Namings.DialogTag("afterBattleHireOk"); //
        msg = Namings.Format(hireMsg, Namings.ShipConfig(cng), Namings.ShipType(type));
        var itemsCount = MyExtensions.Random(1, 2);
        for (var i = 0; i < itemsCount; i++)
            if (ship.GetFreeWeaponSlot(out var inex))
            {
                var weapon = Library.CreateDamageWeapon(true);
                ship.TryAddWeaponModul(weapon, inex);
            }

        MainController.Instance.MainPlayer.Army.TryHireShip(new StartShipPilotData(pilot, ship));

        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }

    #endregion
}