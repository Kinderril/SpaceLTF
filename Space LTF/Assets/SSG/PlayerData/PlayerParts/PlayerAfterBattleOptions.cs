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
    afterBattleRepairAction,
    afterBattleMicrochip,
}


[Serializable]
public class PlayerAfterBattleOptions
{
    private const int START_MAX_COUNT = 10000;
    private const int DialogFrequancy = 3;
//    private Player _player;

    private Dictionary<EAfterBattleAnswers, int> _answersCount = new Dictionary<EAfterBattleAnswers, int>
    {
        {EAfterBattleAnswers.afterBattleBuyout, START_MAX_COUNT},
        {EAfterBattleAnswers.afterBattleTeachPilots, START_MAX_COUNT},
        {EAfterBattleAnswers.afterBattleUpgradeSpell, START_MAX_COUNT},
        {EAfterBattleAnswers.afterBattleOpenCells, START_MAX_COUNT},
        {EAfterBattleAnswers.afterBattleHireAction, START_MAX_COUNT},
        {EAfterBattleAnswers.afterBattleSearchFor, START_MAX_COUNT},
        {EAfterBattleAnswers.afterBattleKillAction, START_MAX_COUNT},
        {EAfterBattleAnswers.afterBattleRepairAction, START_MAX_COUNT}   ,
        {EAfterBattleAnswers.afterBattleMicrochip, START_MAX_COUNT}
    };

    private int _lastStepGetDialog;


    public MessageDialogData GetDialog(int step, float cellPower, ShipConfig config,Player player)
    {

//        var msg1 = Namings.Format(Namings.DialogTag("afterBattleStart"));
//        var list = new List<AnswerDialogData>();
//        list.Add(GetAnswer(EAfterBattleAnswers.afterBattleBuyout, cellPower, config));
//        var dialog1 = new MessageDialogData(msg1, list);
//        return dialog1;

        var delta = step - _lastStepGetDialog;
        var isWork = SkillWork(DialogFrequancy, delta);
//#if UNITY_EDITOR
//        isWork = true;
//#endif
        if (isWork)
        {
            _lastStepGetDialog = step;
            var ans = InitPosibleAnswers(cellPower, config, player);
            ans.Add(new AnswerDialogData(Namings.Tag("leave")));
            var msg = Namings.Format(Namings.DialogTag("afterBattleStart"));
            var dialog = new MessageDialogData(msg, ans);
            return dialog;
        }

        return null;
    }

    private List<AnswerDialogData> InitPosibleAnswers(float power, ShipConfig config, Player player)
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
            preList.Add(EAfterBattleAnswers.afterBattleMicrochip);
        }
        foreach (var answerse in preList)
        {
            var answer = GetAnswer(answerse, power, config, player);
            if (answer != null)
                list.Add(answer);
        }
        var answers = list.RandomElement(3);
        return answers;
    }

    private AnswerDialogData GetAnswer(EAfterBattleAnswers type, float power, ShipConfig config, Player _player)
    {
        void IncCallback()
        {
            _answersCount[type]--;
        }

        switch (type)
        {
            case EAfterBattleAnswers.afterBattleBuyout:
                var rep = _player.ReputationData.ReputationFaction[config];
                var chance = GetPercent(15, rep);
                var strChance = Namings.Format(Namings.Tag("ChanceAfterBattle"), chance.ToString("0"));
                return new AnswerDialogData($"{Namings.DialogTag("afterBattleBuyout")} {strChance}", IncCallback,
                    () => Buyout(power, config, _player));
            case EAfterBattleAnswers.afterBattleTeachPilots:
                return new AnswerDialogData(Namings.DialogTag("afterBattleTeachPilots"), IncCallback,
                    () => TeachPilots(_player));
            case EAfterBattleAnswers.afterBattleUpgradeSpell:
                return new AnswerDialogData(Namings.DialogTag("afterBattleUpgradeSpell"), IncCallback,
                    () => UpgradeSpell(_player));
            case EAfterBattleAnswers.afterBattleOpenCells:
                return new AnswerDialogData(Namings.DialogTag("afterBattleOpenCells"), IncCallback, 
                    () => OpenCells(_player));
            case EAfterBattleAnswers.afterBattleHireAction:
                return new AnswerDialogData(Namings.DialogTag("afterBattleHireAction"), IncCallback,
                    () => HireAction(_player,config));
            case EAfterBattleAnswers.afterBattleSearchFor:
                var chance3 = GetPercent(2, _player.Parameters.Scouts.Level);
                var strChance3 = Namings.Format(Namings.Tag("ChanceAfterBattle"), chance3.ToString("0"));
                return new AnswerDialogData($"{Namings.DialogTag("afterBattleSearchFor")} {strChance3}", IncCallback,
                    () => SearchFor(power, config, _player));
            case EAfterBattleAnswers.afterBattleKillAction:
                return new AnswerDialogData(Namings.DialogTag("afterBattleKillAction"), IncCallback,
                    () => KillAction(power, config, _player));
            case EAfterBattleAnswers.afterBattleRepairAction:
                var chance1 = GetPercent(2, _player.Parameters.Repair.Level);
                var strChance1 = Namings.Format(Namings.Tag("ChanceAfterBattle"), chance1.ToString("0"));
                return new AnswerDialogData($"{Namings.DialogTag("afterBattleRepairAction")} {strChance1}", IncCallback,
                    () => RepairAction(power, config, _player));
            case EAfterBattleAnswers.afterBattleMicrochip:
                var chance2 = GetPercent(BASE_VAL_MICROCHIP, _player.Parameters.Repair.Level);
                var strChance2 = Namings.Format(Namings.Tag("ChanceAfterBattle"), chance2.ToString("0"));
                return new AnswerDialogData($"{Namings.DialogTag("afterBattleMicrochip")} {strChance2}", IncCallback,
                    () => GetMicrochip(power, config, _player));
            default:
                return null;
        }
    }

    private const float BASE_VAL_MICROCHIP = 2;

    protected bool SkillWork(int baseVal, int skillVal)
    {
        if (skillVal < 0f)
        {
            return false;
        }

        var wd = new WDictionary<bool>(new Dictionary<bool, float>
        {
            {true, skillVal},
            {false, baseVal}
        });
        return wd.Random();
    }


    #region RandomActions

    private MessageDialogData TeachPilots(Player player)
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        var c = player.Army.Army.Count;
        var pilotsToTeach = player.Army.Army.RandomElement(MyExtensions.Random(1, c));
        foreach (var shipPilotData in pilotsToTeach)
            if (shipPilotData.Ship.ShipType != ShipType.Base)
                shipPilotData.Pilot.UpgradeRandomLevel(false, true);

        msg = Namings.Format(Namings.DialogTag("afterBattleTeachOk"), pilotsToTeach.Count);
        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }

    private MessageDialogData UpgradeSpell(Player player)
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        var ship =
            player.Army.Army.FirstOrDefault(x => x.Ship.ShipType == ShipType.Base);
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

    private MessageDialogData OpenCells(Player player)
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
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

    private float GetPercent(float baseVal,float curVal)
    {
        if (curVal < 0)
        {
            return 0;
        }

        return (curVal / (baseVal + curVal))*100;
    }

    private MessageDialogData Buyout(float power, ShipConfig config, Player _player)
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        var rep = _player.ReputationData.ReputationFaction[config];
        if (SkillWork(15, rep))
        {
            var scouts = _player.Parameters.Scouts.Level;
            var delta = MoneyConsts.MAX_PASSIVE_LEVEL - scouts;
            var coef = power * Library.MONEY_QUEST_COEF;
            var monet = (int)(MyExtensions.Random(delta * 3, delta * 5) * coef * _player.SafeLinks.CreditsCoef);
            _player.MoneyData.AddMoney(monet);

            msg = Namings.Format(Namings.DialogTag("afterBattleBuyoutConfirm"), monet);
        }
        else
        {
            msg = Namings.Format(Namings.DialogTag("afterBattleBuyoutFail"));
        }

        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }

    private MessageDialogData GetMicrochip(float power, ShipConfig config, Player _player)
    {
        var ws = new WDictionary<bool>(new Dictionary<bool, float>
        {
            {true, _player.Parameters.Repair.Level}, {false,BASE_VAL_MICROCHIP}
        });
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        if (ws.Random())
        {
            msg = Namings.Format(Namings.DialogTag("afterBattleMircochipOk"));
            _player.MoneyData.AddMicrochips(1*_player.SafeLinks.MicrochipCoef);
        }
        else
        {
            msg = Namings.Format(Namings.DialogTag("afterBattleMircochipFail")); //
        }

        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }

    private MessageDialogData SearchFor(float power, ShipConfig config, Player _player)
    {
        var ws = new WDictionary<bool>(new Dictionary<bool, float>
        {
            {true, _player.Parameters.Scouts.Level}, {false, 2}
        });
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        if (ws.Random())
        {
            var scouts = _player.Parameters.Scouts.Level;
            var coef = power * Library.MONEY_QUEST_COEF;
            var monet = (int)(MyExtensions.Random(scouts * 3, scouts * 5) * coef * _player.SafeLinks.CreditsCoef);
            _player.MoneyData.AddMoney(monet);
            msg = Namings.Format(Namings.DialogTag("afterBattleSearchOk"), monet); //"Credits add: {0}."
//            _player.ReputationData.RemoveReputation(config, Library.REPUTATION_STEAL_REMOVE);
        }
        else
        {
            msg = Namings.Format(Namings.DialogTag("afterBattleSearchFail")); //
        }

        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }

    private MessageDialogData KillAction(float power, ShipConfig config, Player _player)
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
//        var shallWork = true; //SkillWork(1, _player.Parameters.Scouts.Level);
//        if (shallWork)
//        {
//            _player.ReputationData.RemoveReputation(config, 8);
            var coef = power * Library.MONEY_QUEST_COEF;
            var monet = (int)(MyExtensions.Random(20, 30) * coef * _player.SafeLinks.CreditsCoef);
            _player.MoneyData.AddMoney(monet);
            msg = Namings.Format(Namings.DialogTag("afterBattleKillOk"), monet); //
//        }
//        else
//        {
////            _player.ReputationData.RemoveReputation(config, 16);
////            msg = Namings.DialogTag("afterBattleKillFail"); //
//        }

        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }

    private MessageDialogData RepairAction(float power, ShipConfig config, Player _player)
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        var shallWork = SkillWork(2, _player.Parameters.Repair.Level);
        if (shallWork)
        {
//            _player.ReputationData.RemoveReputation(config, 15);
            msg = Namings.Format(Namings.DialogTag("afterBattleRepairOk"));
        }
        else
        {
            msg = Namings.DialogTag("afterBattleRepairFail");
        }

        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }

    private MessageDialogData HireAction(Player _player, ShipConfig? config = null)
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
        var ship = Library.CreateShip(type, cng, _player.SafeLinks, pilot);
        var hireMsg = Namings.DialogTag("afterBattleHireOk"); //
        msg = Namings.Format(hireMsg, Namings.ShipConfig(cng), Namings.ShipType(type));
        var itemsCount = MyExtensions.Random(1, 2);
        for (var i = 0; i < itemsCount; i++)
            if (ship.GetFreeWeaponSlot(out var inex))
            {
                var weapon = Library.CreateDamageWeapon(true);
                ship.TryAddWeaponModul(weapon, inex);
            }

        _player.Army.TryHireShip(new StartShipPilotData(pilot, ship));

        var dialog = new MessageDialogData(msg, ans);
        return dialog;
    }

    #endregion
}