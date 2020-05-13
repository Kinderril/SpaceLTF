using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class UpgradeMainShipTutorGlobalCell : GlobalMapCell
{
//    public int RemainEnergyRepairs = 1;

    public UpgradeMainShipTutorGlobalCell(int id, int intX, int intZ, SectorData secto, ShipConfig config) : base(id, intX, intZ, secto, config)
    {
    }

    public override string Desc()
    {
        return Namings.Tag("MainShipUpgradeTutor");
    }

    public override void Take()
    {

    }
    public override bool CanGotFromIt(bool withAction)
    {
        
        var canGO =  CanGo();
        if (!canGO && withAction)
            OpenTutor();
        return canGO;
    }

    private bool CanGo()
    {
        var player = MainController.Instance.MainPlayer;
        if (player.Parameters.ChargesSpeed.Level > 3 || player.Parameters.Repair.Level > 3 || player.Parameters.ChargesCount.Level > 3)
        {
            return true;
        }
        return false;
    }

    protected override MessageDialogData GetDialog()
    {
        var answers = new List<AnswerDialogData>();
        var player = MainController.Instance.MainPlayer;
        if (!player.MoneyData.HaveMoney(5000))
            player.MoneyData.AddMoney(10000);

        MessageDialogData mesData;
        answers.Add(new AnswerDialogData(Namings.Tag("Ok"), OpenTutor));
        mesData = new MessageDialogData(Namings.DialogTag("TutorialUpgradeMainShip"), answers);
        return mesData;

    }

    private void OpenTutor()
    {
        var curWindow = WindowManager.Instance.CurrentWindow as MapWindow;
        if (curWindow != null)
        {
            curWindow.StartUpgradeMainShipTutor();
        }
    }       

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
}