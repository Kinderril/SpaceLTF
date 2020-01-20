using System;

public class EnemyGlobalMapMoverObjet : GlobalMapMoverObject
{
    public MovingArmy Owner;
    private GlobalMapController _mapController;

    public void Init(GlobalMapController mapController, GlobalMapCellObject startCell, MovingArmy owner)
    {
        _mapController = mapController;
        base.Init(startCell);
        Owner = owner;
    }

    public GlobalMapCell FindPlace(GlobalMapCell playersCell)
    {
        var place = Owner.FindCellToMove(playersCell);
        return place;
    }

    public bool AndGo(Action callback, GlobalMapCell place, float timeToMove)
    {
        var objPlace = _mapController.GetCellObjectByCell(place);
        if (objPlace != null)
        {
            MoveTo(timeToMove,objPlace, () =>
            {
                Owner.CurCell.CurMovingArmy = null;
                Owner.CurCell = place;
                Owner.CurCell.CurMovingArmy = Owner;
                callback();
            });
            return true;
        }
        else
        {
            return false;
        }
    }
}

