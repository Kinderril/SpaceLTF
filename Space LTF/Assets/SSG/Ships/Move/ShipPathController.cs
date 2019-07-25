using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



public class ShipPathController
{
    private float Dist = 5f;
    private bool _lastFrameBlocks;
    private int _maskLayer;
    private SideTurn turnSideTurn = SideTurn.left;
    public Vector3 Target { get; private set; }
    private ShipBase _owner;
    private Vector3 _lastTarget;

    private Vector3? blockPosition = null;
    private Vector3? targetCorner = null;
    private PathDebugData _debugData;

#if DEBUG
    private SideTurn _lastSideTurn = SideTurn.left;
    private int _switchTimes = 0;
#endif

    public ShipPathController(ShipBase owner,float dist)
    {
        Dist = dist;
        _maskLayer = 1 << LayerMask.NameToLayer("Border");
        _owner = owner;
        _debugData = new PathDebugData();
    }
    
    public Vector3 GetCurentDirection4(Vector3 target, out bool exactPoint, out bool goodDir,out Vector3? pointToGo,out float speed)
    {
        var targetDirection = GetCurentDirection4(target, out exactPoint, out goodDir, _debugData, out pointToGo);

        targetDirection = Utils.NormalizeFastSelf(targetDirection);
        var updatedDir = _owner.Cell.UpdatePosibleDirection(targetDirection,  _owner.EulerY);
        speed = _owner.Cell.CheckWantSlow(_owner.LookDirection,_owner.Position,  _owner.EulerY);

        return updatedDir;
    }

    public Vector3 GetCurentDirection4(ShipBase target, out bool exactPoint, out bool goodDir, out Vector3? pointToGo, out float speed)
    {
        var targetDir = target.Position - _owner.Position;
        var inFront = Vector3.Dot(targetDir, _owner.LookDirection) > 0;
        if (inFront)
        {
            var isCrossed = IsCellFreeByTarget(target.Position);
            if (isCrossed)
            {
//                var ptarget = target.PredictionPos();
//                return GetCurentDirection4(ptarget, out nextIsSame, out goodDir);
                return GetCurentDirection4(target.Position, out exactPoint, out goodDir, out pointToGo, out speed);
            }
            else
            {
                return GetCurentDirection4(target.Position, out exactPoint, out goodDir, out pointToGo, out speed);
            }
        }
        return GetCurentDirection4(target.Position, out exactPoint, out goodDir,  out pointToGo, out speed);

    }

    private Vector3 GetCurentDirection4(Vector3 target, out bool exactPoint, out bool goodDir,PathDebugData debugData,out Vector3? pointToGo)
    {
        debugData.Reset();
        _lastTarget = target;
//        var targetDir = target - _owner.Position;
        targetCorner = null;
        blockPosition = null;
        var dir = PathDirectionFinder.TryFindDirection(_owner.CellController,_owner.LookDirection,_owner.Cell, target, _owner.Position,  out goodDir,
            debugData);
#if DEBUG
//        var vv = Vector3.Angle(dir, _owner.LookDirection);
        var isLeft = Vector3.Dot(_owner.LookLeft, dir) > 0;
        var nextSideTurn = isLeft ? SideTurn.left : SideTurn.right;
        if (_lastSideTurn != nextSideTurn)
        {
            _switchTimes++;
            if (_switchTimes > 2)
            {
//                Debug.LogError("too many turns " + _owner.name);
            }
        }
        else
        {
            _switchTimes = 0;
        }

        _lastSideTurn = nextSideTurn;

#endif
        exactPoint = false;
        pointToGo = debugData.PointToGo;
        return dir;
    }

    
    private bool IsCellFreeByTarget(Vector3 target)
    {
//        var targetDir = target - _owner.Position;
//        var normDir = Utils.NormalizeFastSelf(targetDir);
        int xDir = 0;
        int zDir = 0;
        var targetCell = _owner.CellController.FindCell(target);
        var deltaX = Mathf.Abs(target.x - _owner.Position.x);
        var deltaZ = Mathf.Abs(target.z - _owner.Position.z);
        if (deltaX < deltaZ)
        {
            if (targetCell.Zindex > _owner.Cell.Zindex)
            {
                zDir = 1;
            }
            else if (targetCell.Zindex < _owner.Cell.Zindex)
            {
                zDir = -1;
            }
        }
        else
        {
            if (targetCell.Xindex > _owner.Cell.Xindex)
            {
                xDir = 1;
            }
            else if (targetCell.Xindex < _owner.Cell.Xindex)
            {
                xDir = -1;
            }
        }

        var xIn = _owner.Cell.Xindex + xDir;
        var zIn = _owner.Cell.Zindex + zDir;
        var testCell = _owner.CellController.Data.GetCell(xIn, zIn);
        return testCell.CellType == CellType.Free;
    }

    /*
    public Vector3 GetCurentDirection3(Vector3 target, out bool nextIsSame, out bool goodDir)
    {
        var targetDir = target - _owner.Position;

        var normDir = Utils.NormalizeFastSelf(targetDir);
        int xDir = 0;
        int zDir = 0;
        var targetCell = BattleController.Instance.CellController.FindCell(target);
        var deltaX = Mathf.Abs(target.x - _owner.Position.x);
        var deltaZ = Mathf.Abs(target.z - _owner.Position.z);
        if (deltaX < deltaZ)
        {
            if (targetCell.Zindex > _owner.Cell.Zindex)
            {
                zDir = 1;
            }
            else if (targetCell.Zindex < _owner.Cell.Zindex)
            {
                zDir = -1;
            }
        }
        else
        {
            if (targetCell.Xindex > _owner.Cell.Xindex)
            {
                xDir = 1;
            }
            else if (targetCell.Xindex < _owner.Cell.Xindex)
            {
                xDir = -1;
            }
        }

        var xIn = _owner.Cell.Xindex + xDir;
        var zIn = _owner.Cell.Zindex + zDir;
        var testCell = BattleController.Instance.CellController.Data.GetCell(xIn, zIn);
        if (testCell.CellType == CellType.Free)
        {
            goodDir = Utils.IsAngLessNormazied(normDir, _owner.LookDirection, UtilsCos.COS_2_RAD);
            nextIsSame = true;
            return normDir;
        }
        
        var from = _owner.Position;
        var hitPoint = testCell.GetHit(from, target);
        if (hitPoint.HasValue)
        {
            var controlPoint = ManeuverLib.ChooseControlPoint(testCell.GetPoints(), hitPoint.Value, target);
            _lastFrameBlocks = true;
            goodDir = false;
            nextIsSame = true;
            var dir = controlPoint - _owner.Position;
            return dir;
        }
        
        nextIsSame = true;
        goodDir = true;
        return _owner.LookDirection;
    }
    public Vector3 GetCurentDirection2(Vector3 target, out bool nextIsSame, out bool goodDir)
    {
        Target = target;
        var targetDir = target - _owner.Position;
        var normDir = Utils.NormalizeFastSelf(targetDir);
        nextIsSame = false;
        if (_owner.InAsteroidField)
        {
            goodDir = true;
            _lastFrameBlocks = false;

            return _owner.LookDirection;
        }
        RaycastHit hit;
        var isNowBlocks = Physics.Raycast(_owner.Position, _owner.LookDirection, out hit, Dist, _maskLayer);
        if (!isNowBlocks)
        {
            goodDir = Utils.IsAngLessNormazied(normDir, _owner.LookDirection, UtilsCos.COS_2_RAD);
            _lastFrameBlocks = false;
            return normDir;
        }

        var field = hit.collider.GetComponent<AsteroidField>();
        var controlPoint = ManeuverLib.ChooseControlPoint(field, hit.point, target);
        _lastFrameBlocks = true;
        goodDir = false;
        var dir = controlPoint - _owner.Position;
        return dir;
    }

    public Vector3 GetCurentDirection(Vector3 target, out bool nextIsSame, out bool goodDir)
    {
        Target = target;
        var targetDir = target - _owner.Position;
        nextIsSame = false;
        if (_owner.InAsteroidField)
        {
            goodDir = false;
            _lastFrameBlocks = false;
            var nextTargetDir2 = Utils.Rotate90(targetDir, turnSideTurn);
            return nextTargetDir2;
        }
        RaycastHit hit;
        var isNowBlocks = Physics.Raycast(_owner.Position, _owner.LookDirection, out hit, Dist, _maskLayer);
        if (!isNowBlocks)
        {
            goodDir = false;
            _lastFrameBlocks = false;
            return targetDir;
        }
        if (!_lastFrameBlocks)
        {
            var field = hit.collider.GetComponent<AsteroidField>();
            if (field == null)
            {
                if (MyExtensions.IsTrue01(.5f))
                {
                    turnSideTurn = SideTurn.left;
                }
                else
                {
                    turnSideTurn = SideTurn.right;
                }
            }
            else
            {
                var controlPoint = ManeuverLib.ChooseControlPoint(field, hit.point, target);
                turnSideTurn = ManeuverLib.GetTurnByDir(controlPoint, _owner.Position, _owner.LookDirection);
            }
        }
        _lastFrameBlocks = true;
        goodDir = false;
        var nextTargetDir = Utils.Rotate90(targetDir, turnSideTurn);
        return nextTargetDir;
    }
    */

    public bool Complete(Vector3 target,float checkDist = 0.5f)
    {
        var sDist = (target - _owner.Position).sqrMagnitude;
        if (sDist < checkDist)
        {
            return true;
        }
        return false;
    }

    public Vector3 Turn45()
    {
        var turnRad = _owner.MaxTurnRadius;
        Vector3 side;
        if (MyExtensions.IsTrue01(0.5f))
        {
            side = _owner.LookLeft;
        }
        else
        {
            side = _owner.LookRight;
        }
        var target = _owner.LookDirection * turnRad + side * turnRad + _owner.Position;
        return target;
    }

    public void OnDrawGizmosSelected()
    {
        _debugData.DrawGizmos();
//        Gizmos.color = _lastFrameBlocks ? Color.red : Color.green;
//        Gizmos.DrawRay(_owner.Position, _owner.LookDirection* Dist);
//        Gizmos.color = Color.white;
//        Gizmos.DrawLine(_owner.Position, _lastTarget);
    }
}

