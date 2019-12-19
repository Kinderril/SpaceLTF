using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class DefenceAction : BaseAction
{
    public const float CloseToMainShipSQRT = 1f;
    public const float CloseToMainShipSQRT2 = CloseToMainShipSQRT*1.2f;
    public const float maxDist = 10f;
    public const float offsetDist = maxDist/2f;
    public const float MaxSdist = maxDist * maxDist;
    private float _nextCheckEnemies = 0;
    
    private float _endDefence = 0;
    private ShipBase Target;

    public DefenceAction([NotNull] ShipBase owner, ShipBase target) 
        : base(owner,ActionType.defence)
    {
        Target = target;
        FindWay();
        _endDefence = Time.time + 20f;
    }

    private Vector3 GetRandomPointToMove()
    {
        Vector3 p1 = Target.Position + Target.LookRight * offsetDist;
        Vector3 p2 = Target.Position + Target.LookLeft * offsetDist;
        var sDist1 = (p1 - _owner.Position).sqrMagnitude;
        var sDist2 = (p2 - _owner.Position).sqrMagnitude;
        if (sDist1 > sDist2)
        {
            return p1;
        }
        return p2;
    }

    public override void ManualUpdate()
    {
        // _owner.SetTargetSpeed(1f);
        if (_targetPoint != null)
        {
            if (!_owner.PathController.Complete(_targetPoint.Value))
            {
                _owner.MoveByWay(_targetPoint.Value);
            }
            else
            {
                FindWay();
            }
        }
        else
        {
            FindWay();
        }
    }

    private void FindWay()
    {
        _targetPoint = GetRandomPointToMove();
    }


    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
            new CauseAction("out bf", () => !_owner.InBattlefield),
            new CauseAction("end def time", () => _endDefence < Time.time),
            new CauseAction("target dead", () =>Target.IsDead),
            new CauseAction("enemies in raduis", () =>
            {
                if (_nextCheckEnemies < Time.time)
                {
                    _nextCheckEnemies = Time.time + 1f;
                    if (CheckEnemiesInRadius())
                    {
                        return true;
                    }
                }
                return false;
            })
        };
        return c;
    }
    

    private bool CheckEnemiesInRadius()
    {
        foreach (var shipPersonalInfo in _owner.Enemies)
        {
            var sd = (shipPersonalInfo.Key.Position - Target.Position).sqrMagnitude;
            if (sd < MaxSdist)
            {
                return true;
            }
        }
        return false;
    }

    public override void DrawGizmos()
    {
        var d = 0.2f;
//        Gizmos.DrawSphere(Target.Position, d);
        DrawUtils.DebugCircle(Target.Position,Vector3.up, Color.blue, maxDist);
//        Gizmos.DrawSphere(Target.Position+Vector3.up*d,d);
    }
}

