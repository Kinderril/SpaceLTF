using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpecOpsMovingArmy : MovingArmy
{
    public SpecOpsMovingArmy(GlobalMapCell startCell, Action<MovingArmy> destroyCallback) 
        : base(startCell, destroyCallback)
    {

    }

    public GlobalMapCell FindCellToMove(GlobalMapCell playersCell, HashSet<GlobalMapCell> posibleCells)
    {
        if (_noStepNext)
        {
            _noStepNext = false;
            return null;
        }

        if (playersCell == CurCell)
        {
            return null;
        }
        var ways = CurCell.GetCurrentPosibleWays();
        var ststus = MainController.Instance.MainPlayer.ReputationData.GetStatus(_player.Army.BaseShipConfig);
        var posibleWays = ways.Where(x => !(x is GlobalMapNothing) && x.CurMovingArmy == null).ToList();
        if (posibleWays.Count == 0)
        {
            return null;
        }

        switch (ststus)
        {
            default:
            case EReputationStatus.friend:
            case EReputationStatus.neutral:
                var selectedWay = posibleWays.RandomElement();
                if (posibleCells.Contains(selectedWay))
                    return selectedWay;
                break;

            case EReputationStatus.negative:
            case EReputationStatus.enemy:
                int minDelta = 999;
                GlobalMapCell cellToGo = posibleWays[0];
                foreach (var way in posibleWays)
                {
                    var a = Mathf.Abs(way.indX - playersCell.indX);
                    var b = Mathf.Abs(way.indZ - playersCell.indZ);
                    var c = a + b;
                    if (c < minDelta)
                    {
                        minDelta = c;
                        cellToGo = way;
                    }
                }
                if (posibleCells.Contains(cellToGo))
                    return cellToGo;
                break;
        }

        return null;

    }

}
