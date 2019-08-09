﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RepairStationGlobalCell : GlobalMapCell
{

    public RepairStationGlobalCell( int id, int intX, int intZ, SectorData secto) : base( id, intX, intZ, secto)
    {
    }

    public override string Desc()
    {
        return "Repair station";
    }

    public override void Take()
    {
        WindowManager.Instance.InfoWindow.Init(null,"Repair compete");
    }

    public override MessageDialogData GetDialog()
    {
        var player = MainController.Instance.MainPlayer;
        var allShips = player.Army;
        var total = 0;
        foreach (var startShipPilotData in allShips)
        {
            var pointToRepair = startShipPilotData.Ship.HealthPointToRepair();
            var cost = Library.GetReapairCost(pointToRepair, startShipPilotData.Pilot.CurLevel);
            var thisShip = (int)Mathf.Clamp((int)cost * Library.REPAIR_DISCOUTNT, 1, 99999);
            total += thisShip;
        }

        
        //var half = total/2;
        MessageDialogData mesData;
        if (total > 0)
        {
            var canRepairFull = player.MoneyData.HaveMoney(total);
            //var canRepairHalf = player.MoneyData.HaveMoney(half);
            var answers = new List<AnswerDialogData>();
          
            if (canRepairFull)
            {
                answers.Add(new AnswerDialogData(String.Format("Yes repair all.{0} credits", total), () =>
                {
                    RepairFor(total);
                    Take();
                }));
                answers.Add(new AnswerDialogData("No", null));
            }
            else
            {
                answers.Add(new AnswerDialogData("I don't have enought credits", null));
            }
            mesData = new MessageDialogData("Do you want to rapair your ships?.", answers);
        }
        else
        {
            mesData = new MessageDialogData("We can repair ships. But you don't need it?", new List<AnswerDialogData>
            {
                new AnswerDialogData("Ok", null)
            });
        }
        return mesData;
    }

    public override Color Color()
    {
        return new Color(255f/255f, 204f/255f, 153f/255f);
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
        foreach (var startShipPilotData in player.Army)
        {
            startShipPilotData.Ship.SetRepairPercent(1f);
        }
    }
}