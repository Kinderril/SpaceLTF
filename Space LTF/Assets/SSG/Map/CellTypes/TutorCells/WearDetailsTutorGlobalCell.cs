using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class WearDetailsTutorGlobalCell : GlobalMapCell
{
//    public int RemainEnergyRepairs = 1;

    public WearDetailsTutorGlobalCell(int id, int intX, int intZ, SectorData secto, ShipConfig config) : base(id, intX, intZ, secto, config)
    {
    }

    public override string Desc()
    {
        return Namings.Tag("WearDetailsTutor");
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
        Player _greenPlayer = MainController.Instance.MainPlayer;
        var battleShips = _greenPlayer.Army.Army.Where(x => x.Ship.ShipType != ShipType.Base).ToList();
        if (battleShips.Count == 0)
        {
            return true;
        }
        else
        {
            var ship = battleShips.First();
            var items = ship.Ship.CocpitSlot != null && ship.Ship.CocpitSlot != null && ship.Ship.CocpitSlot != null;
            return items;
        }
        return false;
    }

    protected override MessageDialogData GetDialog()
    {
        var answers = new List<AnswerDialogData>();

        MessageDialogData mesData;
        answers.Add(new AnswerDialogData(Namings.Tag("Ok"), OpenTutor));
        mesData = new MessageDialogData(Namings.DialogTag("WearDetailsTutor"), answers);
        return mesData;

    }

    private void OpenTutor()
    {
        var curWindow = WindowManager.Instance.CurrentWindow as MapWindow;
        if (curWindow != null)
        {
            curWindow.WearBattleShipDetailsTutor();
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