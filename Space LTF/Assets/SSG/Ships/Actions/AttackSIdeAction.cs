using JetBrains.Annotations;
using UnityEngine;


public class AttackSideAction : AttackAction
{
    private Vector3 TargetPos => Target.ShipLink.Position;

    private bool _controlReached = false;
    private Vector3 _controlPoint;

    public AttackSideAction([NotNull] ShipBase owner, [NotNull] ShipPersonalInfo target, Vector3 controlPoint)
        : base(owner, target, ActionType.attackSide)
    {
        _controlPoint = controlPoint;
    }

    public override void ManualUpdate()
    {
        _owner.WeaponsController.CheckWeaponFire(Target);
        if (Target.Dist < 13 || _owner.PathController.Complete(_controlPoint, 3f))
        {
            _controlReached = true;
        }

        // var isSameTarget = IsAmTarget();
        if (_controlReached)
        {
            MoveToTarget();
        }
        else
        {
            // _owner.SetTargetSpeed(1f);
            _owner.MoveByWay(_controlPoint);
        }
    }

    private bool IsAmTarget()
    {
        return _owner.AttackersData.CurAttacker == Target;
    }
    public static Vector3 FindControlPoint(Vector3 start, Vector3 end, Battlefield battlefield)
    {
        var center = (start + end) / 2;
        var dir = end - start;
        var dist = dir.magnitude;
        var dirToENd = Utils.NormalizeFastSelf(dir);
        var right = Utils.Rotate90(dirToENd, SideTurn.left);
        var left = Utils.Rotate90(dirToENd, SideTurn.right);
        var p1 = center + right * dist / 2f;
        var p2 = center + left * dist / 2f;

        var centerBattlefield = battlefield.CellController.Data.CenterZone;

        var sDistP1 = (p1 = centerBattlefield).magnitude;
        var sDistP2 = (p2 = centerBattlefield).magnitude;
        var maxRad = battlefield.CellController.Data.InsideRadius;
        bool p1Good = sDistP1 < maxRad;
        bool p2Good = sDistP2 < maxRad;
        if (p1Good && p2Good)
        {
            if (sDistP1 > sDistP2)
            {
                return p1;
            }
            return p2;
        }

        if (p1Good)
        {
            return p1;
        }
        return p2;
    }
    public override void DrawGizmos()
    {
        if (TargetPos != null)
        {
            if (_controlReached)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_owner.Position, Target.ShipLink.Position);
            }
            else
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(_owner.Position, _controlPoint);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_controlPoint, Target.ShipLink.Position);
            }
        }
    }
}

