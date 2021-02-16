using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class RepairStationGlobalCell : GlobalMapCell
{
    public const int MAX_LEVEL = 3;
    public int RemainEnergyRepairs = 1;
    public float CollectedMoney { get; private set; } = 0;
    public int Level { get; private set; } = 1;

    public RepairStationGlobalCell(int id, int intX, int intZ, SectorData secto, ShipConfig config) : base(id, intX, intZ, secto, config)
    {
    }

    public override string Desc()
    {
        return $"{Namings.Tag("RepairStation")} {Namings.Tag("Level")}:{Level}";
    }

    public override void Take()
    {
        WindowManager.Instance.InfoWindow.Init(null, Namings.Tag("RepairCompete"));
    }

    protected override MessageDialogData GetDialog()
    {
        var answers = new List<AnswerDialogData>();
        var rep = MainController.Instance.MainPlayer.ReputationData.GetStatus(ConfigOwner);
        bool isEnemy = rep == EReputationStatus.enemy;
//        if (rep == EReputationStatus.enemy)
//        {
//            MessageDialogData mesData;
//            answers.Add(new AnswerDialogData(Namings.Tag("Ok")));
//            mesData = new MessageDialogData(Namings.DialogTag("repairEnemy"), answers);
//            return mesData;
//        }
//        else
//        {
        float costCoef = 1f;
        if (Sector.IsMy)
        {
            costCoef = 0.5f;
        }
        else  if (isEnemy)
        {
            costCoef = 2f;
        }

            var player = MainController.Instance.MainPlayer;
            var allShips = player.Army.AllArmy;
            var total = 0;
            bool haveCriticalDamages = false;
            foreach (var startShipPilotData in allShips)
            {
                var pointToRepair = startShipPilotData.Ship.HealthPointToRepair();
                var cost = Library.GetReapairCost(pointToRepair, startShipPilotData.Pilot.CurLevel) * costCoef;
                var thisShip = (int)Mathf.Clamp((int)cost * Library.REPAIR_DISCOUTNT, 1, 99999);
                total += thisShip;
                if (startShipPilotData.Ship.CriticalDamages > 0 || startShipPilotData.Ship.IsDead)
                {
                    haveCriticalDamages = true;
                }
            }

            if (haveCriticalDamages)
            {
                float count = player.Army.Army.Sum(startShipPilotData => startShipPilotData.Ship.CriticalDamages * (5 + startShipPilotData.Pilot.CurLevel * 0.2f));
                var critFixCost = (int)(count * Library.COST_REPAIR_CRIT * costCoef);
                answers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("fixCrit"), critFixCost), null, () => FixCrit(critFixCost)));
            }

            var haveSmtToRepair = player.Army.HaveSmtToRepair();
            string mainMsg;

       
            if (haveSmtToRepair)
            {

                mainMsg = Namings.DialogTag("repairStart");
                if (isEnemy && !Sector.IsMy)
                {
                    mainMsg = $"{mainMsg} {Namings.DialogTag("repairEnemy")}";
                }
            }
            else
            {
                mainMsg = Namings.DialogTag("nothingToRepair");
            }


            MessageDialogData mesData;
            if (total > 0 )
            {
                var canRepairFull = player.MoneyData.HaveMoney(total);
                if (canRepairFull && haveSmtToRepair)
                {
                    answers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("repairAll"), total), null, () =>
                    {
                        var d = RepairAll(total);
                        return d;
                    }));
                    answers.Add(new AnswerDialogData(Namings.Tag("leave"), null));
                }
                else
                {
                    if (haveSmtToRepair)
                    {
                        answers.Add(new AnswerDialogData(Namings.DialogTag("repairNotEnough"), null));
                    }
                    else
                    {
                        answers.Add(new AnswerDialogData(Namings.Tag("leave"), null));
                    }
                }
            }
            else
            {
                answers.Add(new AnswerDialogData(Namings.Tag("leave")));
            }
            if (Sector.IsMy)
            {
                if (Level < MAX_LEVEL)
                {
                    var cost = (int) (Level * 35f);
                    var b = new AnswerDialogData(
                        Namings.Format(Namings.Tag("cellUpgradeRepair"), cost, Level), null,
                        () => TryRequestUpgrade(cost, this));
                    answers.Add(b);
                }

            var a = new AnswerDialogData(Namings.Format(Namings.Tag("TakeReward"), (int)CollectedMoney), TakeReward);
                answers.Add(a);
            }

        mesData = new MessageDialogData($"{mainMsg} {Namings.Format(Namings.Tag("PerairStLevel"), Level)}", answers);
            return mesData;
    }
    private MessageDialogData TryRequestUpgrade(int cost, RepairStationGlobalCell shop)
    {
        return TryRequestSmt(cost, UpgradeThis(shop), "armyBornCenterRepairUpgraded");
    }
    private Action UpgradeThis(RepairStationGlobalCell shop)
    {
        var cell = shop;
        void Act()
        {
            cell.UpgradeLevel();
        }
        return Act;
    }

    private void UpgradeLevel()
    {
        if (Level >= MAX_LEVEL)
        {
            return;
        }

        Level++;

    }

    public override void UpdateStep(int step)
    {
        base.UpdateStep(step);
        if (_sector.IsMy)
        {
            float perTurn = 0f;
            switch (Level)
            {
                case 1:
                    perTurn = 4 * MoneyConsts.CELL_MONEY_COEF;
                    break;
                case 2:
                    perTurn = 5 * MoneyConsts.CELL_MONEY_COEF;
                    break;
                case 3:
                    perTurn = 6 * MoneyConsts.CELL_MONEY_COEF;
                    break;
            }

            CollectedMoney += perTurn;
        }

    }

    private void TakeReward()
    {
        CollectedMoney = 0f;
        MainController.Instance.MainPlayer.MoneyData.AddMoney((int)CollectedMoney);
    }

    private MessageDialogData RepairAll(int total)
    {
        RepairFor(total);
        var answers = new List<AnswerDialogData>();
        answers.Add(new AnswerDialogData(Namings.Tag("Ok"), null));
        var mesData = new MessageDialogData(Namings.DialogTag("repairAll"), answers);
        return mesData;
    }

    private MessageDialogData FixCrit(int cost)
    {
        var player = MainController.Instance.MainPlayer;
        var haveMoney = player.MoneyData.HaveMoney(cost);
        var answers = new List<AnswerDialogData>();
        MessageDialogData mesData;
        answers.Add(new AnswerDialogData(Namings.Tag("Ok"), null, () => GetDialog()));
        if (haveMoney)
        {
            foreach (var startShipPilotData in player.Army.AllArmy)
            {
                startShipPilotData.Ship.RestoreAllCriticalDamages();
            }
            mesData = new MessageDialogData(Namings.DialogTag("repairCritFixed"), answers);
        }
        else
        {
            mesData = new MessageDialogData(Namings.Tag("NotEnoughtMoney"), answers);
        }
        return mesData;
    }

//    private MessageDialogData Statilize()
//    {
//        var answers = new List<AnswerDialogData>();
//        answers.Add(new AnswerDialogData(Namings.Tag("Ok")));
//        var player = MainController.Instance.MainPlayer;
//        RemainEnergyRepairs--;
//        player.ByStepDamage.Repair();
//        var mesData =
//            new MessageDialogData(Namings.Format("Module stabilized for {0} days", player.ByStepDamage._curRemainSteps),
//                answers);
//        return mesData;
//    }

    public override Color Color()
    {
        return new Color(255f / 255f, 204f / 255f, 153f / 255f);
    }

    public override bool OneTimeUsed()
    {
        return false;
    }

    public override bool CanCellDestroy()
    {
        return true;
    }

    private void RepairFor(int count)
    {
        var player = MainController.Instance.MainPlayer;
        //var perShip = count/player.Army.Count;
        player.MoneyData.RemoveMoney(count);
        foreach (var startShipPilotData in player.Army.Army)
        {
            startShipPilotData.Ship.SetRepairPercent(1f);
        }
    }
}