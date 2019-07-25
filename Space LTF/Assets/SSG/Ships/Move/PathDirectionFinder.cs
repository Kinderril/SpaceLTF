using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

class CrossData
{
    public Vector3 cross;
    public AICellSegment cellSegment;

    public CrossData(
        Vector3 cross,
        AICellSegment cellSegment)
    {
        this.cellSegment = cellSegment;
        this.cross = cross;
    }
}

public static class PathDirectionFinder
{
    public static Vector3 TryFindDirection(CellController cellController,Vector3 lookDirection,AICell currentCell, Vector3 targetPoint, Vector3 startPosition,
         out bool goodDir, PathDebugData debugData)
    {
        debugData.Reset();
        debugData.StartPoint = startPosition;
        debugData.TargetPoint = targetPoint;
        debugData.c0 = currentCell;
        if (!currentCell.IsFree())
        {
            var dir = NotFreeGetDirection(cellController, lookDirection, currentCell, startPosition, out goodDir, debugData);
            return dir;
        }
        else
        {
            var targetDir2 = targetPoint - startPosition;
            var normDir = Utils.NormalizeFastSelf(targetDir2);
            //        CellPoint resultCorner;
            var getCross = GetCross(currentCell, targetPoint, startPosition);
            if (getCross != null)
            {
                var controlPoint = GetControlPoint(getCross, normDir);
                debugData.CrossPoint = getCross.cross;
                var pointToGo = CheckingCells(getCross.cross, controlPoint, startPosition, currentCell, debugData);
                if (pointToGo != null)
                {
                    debugData.PointToGo = pointToGo;
                    goodDir = false;
                    var targetDir = pointToGo.Value - startPosition;
                    debugData.ResultDirection = targetDir;
                    return targetDir;

                }

            }
            goodDir = Utils.IsAngLessNormazied(normDir, lookDirection, UtilsCos.COS_2_RAD);
            debugData.ResultDirection = normDir;
            return normDir;
        }
    }

    private static Vector3 NotFreeGetDirection(CellController cellController,Vector3 lookDirection, AICell currentCell,
       Vector3 startPosition, out bool goodDir, PathDebugData debugData)
    {
        var freeCell = cellController.Data.FindClosestCellByType(currentCell, CellType.Free);

        var targetDir2 = freeCell.Center - startPosition;
        var normDir = Utils.NormalizeFastSelf(targetDir2);
        goodDir = Utils.IsAngLessNormazied(normDir, lookDirection, UtilsCos.COS_2_RAD);
        debugData.ResultDirection = normDir;
        return normDir;
//        var xI = currentCell.Xindex;
//        var zI = currentCell.Zindex;
//        if (Mathf.Abs(lookDirection.z) < Mathf.Abs(lookDirection.x))
//        {
//            //TEST by X
//            if (lookDirection.x < 0)
//            {
//                var cell = cellController.Data.GetCell(xI - 1, 0);
//            }
//            else
//            {
//                
//            }
//        }
//        var senToTest = new SegmentPoints(startPosition,startPosition + lookDirection* currentCell.Side*5);
//        for (int i = 0; i < currentCell.Borders.Length; i++)
//        {
//            var b = currentCell.Borders[i];
//            var cross = AIUtility.GetCrossPoint(senToTest, b);
//            if (cross.HasValue)
//            {
//                break;
//            }
//        }

    }

    private static CellPoint GetControlPoint(CrossData data, Vector3 dir)
    {
        var di1 = data.cellSegment.A() - data.cross;
        if (Vector3.Dot(dir, di1) > 0)
        {
            return data.cellSegment.ACell();
        }
        return data.cellSegment.BCell();
    }

    [CanBeNull]
    private static CrossData GetCross(AICell currentCell, Vector3 targetPoint, Vector3 startPosition)
    {
        Vector3? cross;
        var segmentTest = new SegmentPoints(startPosition,targetPoint);
        cross = AIUtility.GetCrossPoint(currentCell.Border1, segmentTest);
        if (cross.HasValue)
        {
            return new CrossData(cross.Value, currentCell.Border1);
        }
        cross = AIUtility.GetCrossPoint(currentCell.Border2, segmentTest);
        if (cross.HasValue)
        {
            return new CrossData(cross.Value, currentCell.Border2);
        }
        cross = AIUtility.GetCrossPoint(currentCell.Border3, segmentTest);
        if (cross.HasValue)
        {
            return new CrossData(cross.Value, currentCell.Border3);
        }
        cross = AIUtility.GetCrossPoint(currentCell.Border4, segmentTest);
        if (cross.HasValue)
        {
            return new CrossData(cross.Value, currentCell.Border4);
        }
        return null;
    }

    private static Vector3? CheckingCells(Vector3 cross, CellPoint pointClosest, Vector3 startPosition, AICell curCell, PathDebugData debugData)
    {
        //        AICellData cellData = _owner.CellController.Data;
        var cells = pointClosest.CellsByCell(curCell);
        AICell c1;
        AICell c2;
        AICell c3;
        CellPoint a = pointClosest;
        CellPoint b;
        CellPoint d;
        CellPoint e;
        var delta2 = cross - pointClosest.Position;
        //        var delta = cross - startPosition;
        if (Mathf.Abs(delta2.z) > Mathf.Abs(delta2.x))
        {
            //V1
            c1 = cells[0];
            c2 = cells[1];
            c3 = cells[2];
            if (cross.x > startPosition.x)
            {
                //V11
                b = pointClosest.ConnectedPoints[2];
                e = pointClosest.ConnectedPoints[0];
                if (cross.z > startPosition.z)
                {
                    //V111
                    d = pointClosest.ConnectedPoints[1];
                }
                else
                {
                    //V112
                    d = pointClosest.ConnectedPoints[3];
                }

            }
            else
            {
                //V12
                b = pointClosest.ConnectedPoints[0];
                e = pointClosest.ConnectedPoints[2];
                if (cross.z > startPosition.z)
                {
                    //V121
                    d = pointClosest.ConnectedPoints[1];
                }
                else
                {
                    //V122
                    d = pointClosest.ConnectedPoints[3];
                }
            }

        }
        else
        {
            c1 = cells[1];
            c2 = cells[0];
            c3 = cells[2];
            //V2
            if (cross.x > startPosition.x)
            {
                //V21
                d = pointClosest.ConnectedPoints[0];
                if (cross.z > startPosition.z)
                {
                    //V211
                    b = pointClosest.ConnectedPoints[3];
                    e = pointClosest.ConnectedPoints[1];
                }
                else
                {
                    //V212
                    b = pointClosest.ConnectedPoints[1];
                    e = pointClosest.ConnectedPoints[3];
                }
            }
            else
            {
                //V22
                d = pointClosest.ConnectedPoints[2];
                if (cross.z > startPosition.z)
                {
                    //V221
                    b = pointClosest.ConnectedPoints[3];
                    e = pointClosest.ConnectedPoints[1];
                }
                else
                {
                    //v222   
                    b = pointClosest.ConnectedPoints[1];
                    e = pointClosest.ConnectedPoints[3];
                }
            }
        }
        return Directions(c1, c2, c3, a, b, d, e, debugData);
    }

    private static Vector3? Directions(AICell c1, AICell c2, AICell c3,
        CellPoint a, CellPoint b, CellPoint d, CellPoint e, PathDebugData debugData)
    {
        debugData.c1 = c1;
        debugData.c2 = c2;
        debugData.c3 = c3;
        debugData.a = a;
        debugData.b = b;
        debugData.d = d;
        debugData.e = e;
        if (c1.IsFree())
        {
            if (c3.IsFree())
            {
                return null;
            }
            else
            {
                return e.Position;
            }
        }
        else
        {
            if (c2.IsFree())
            {
                if (c3.IsFree())
                {
                    return a.Position;
                }
                else
                {
                    return d.Position;
                }
            }
            else
            {
                return b.Position;
            }
        }

    }


}

