using JetBrains.Annotations;
using System.Linq;
using UnityEngine;


public class EvadeAction : BaseAction
{
    private AICell _clouds = null;

    private Vector3 target;
    private Vector3 side;
    private Vector3 pos;

    private float _nextPosibleRecalc;

    public EvadeAction([NotNull] ShipBase owner)
        : base(owner, ActionType.evade)
    {
        int dist;
        var cloudsCell =
            _owner.CellController.Data.FindClosestCellByType(_owner.Cell, CellType.Clouds, false, out dist);
        if (dist < 3)
        {
            if (cloudsCell != null)
            {
                _clouds = cloudsCell;
            }
            else
            {
                //                cloudsCell = _owner.CellController.Data.FindClosestCellByType(_owner.Cell, CellType.Free, out dist);
            }
        }
    }

    public override void ManualUpdate()
    {
        if (_clouds != null)
        {
            // _owner.SetTargetSpeed(1f);
            pos = _clouds.Center;
            _targetPoint = pos;
            _owner.MoveByWay(pos);
            return;
        }

        var danger = _owner.Locator.DangerEnemy;
        var acceleration = false;
        if (danger != null)
        {
            if (_owner.MaxSpeed() > danger.ShipLink.MaxSpeed())
            {
                //GO STRAIGHT
                // _owner.SetTargetSpeed(1f);
                pos = _owner.Position + _owner.LookDirection * 10;
                var cell = _owner.CellController.GetCell(pos);
                if (cell.IsFree())
                {
                    _targetPoint = pos;
                    _owner.MoveByWay(pos);
                    return;
                }
            }

            if (_owner.ShipParameters.TurnSpeed > danger.ShipLink.ShipParameters.TurnSpeed)
            {
                //DO TURN
                side = GetSideDir(danger);
                // _owner.SetTargetSpeed(1f);
                pos = _owner.Position + side * 7;
                var cell = _owner.CellController.GetCell(pos);
                if (cell.IsFree())
                {
                    _targetPoint = pos;
                    _owner.MoveByWay(pos);
                    return;
                }
            }
            side = GetSideDir(danger);
            pos = _owner.Position + side * 7;
            _targetPoint = pos;
            _owner.MoveByWay(pos);
            return;
        }

        if (_nextPosibleRecalc < Time.time)
        {
            _nextPosibleRecalc = Time.time + 4f;
            Vector3 sumVectors = Vector3.zero;
            var enemiesPos = _owner.Enemies.Where(x => x.Key.ShipParameters.StartParams.ShipType != ShipType.Base)
                .ToList();
            if (enemiesPos.Count > 0)
            {

                foreach (var shipPersonalInfo in enemiesPos)
                {
                    sumVectors += shipPersonalInfo.Key.Position;
                }

                var enemiesCenter = sumVectors / enemiesPos.Count;
                var cellC = _owner.CellController;
                var fieldCenter = (cellC.Min + cellC.Max) / 2f;
                var dir = enemiesCenter - fieldCenter;
                var norm = Utils.NormalizeFastSelf(dir);
                pos = fieldCenter - norm * (AICellDataRaound.SafeRadius - 1);
            }
            else
            {
                pos = _owner.Position;
            }


            _targetPoint = pos;
        }

        _owner.MoveByWay(pos);


    }

    public override Vector3? GetTargetToArrow()
    {
        return _targetPoint;
    }

    private Vector3 GetSideDir(ShipPersonalInfo danger)
    {
        Vector3 side;
        var sideDot = Vector3.Dot(_owner.LookLeft, danger.DirNorm) > 0;
        side = sideDot ? _owner.LookLeft : _owner.LookRight;
        return side;
    }


    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
//            new CauseAction("_owner dead", () => !_owner.InBattlefield),
            new CauseAction("out bf", () => !_owner.InBattlefield),
            new CauseAction("no danger",
                () => _owner.DesicionData.GlobalTactics == EGlobalTactics.Fight &&
                      _owner.Locator.DangerEnemy == null),
            new CauseAction("no danger ship link",
                () => _owner.DesicionData.GlobalTactics == EGlobalTactics.Fight &&
                      _owner.Locator.DangerEnemy.ShipLink == null),
            new CauseAction("weapon load",
                () => _owner.DesicionData.GlobalTactics == EGlobalTactics.Fight &&
                      _owner.WeaponsController.AnyWeaponIsLoaded())
//            new CauseAction("path complete", () => _owner.PathController.Complete(_targetPoint.Value)),
        };
        return c;
    }

    public override void DrawGizmos()
    {
    }
}