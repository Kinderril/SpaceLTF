using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MoveByWayAction : AbstractAttackAction
{
    private const float SDIST_CLOSE = 1f;
    protected bool _isShootEnd;
    protected bool _isEndWay;
    private readonly float _minAttackDist;
//    private readonly float _minAttackDistToEnd;
//    private readonly float _minAttackDistToStart;
//    protected float _nextRecalTime;
//    protected float _nextCheckTwist;
//    protected float _nextCheckRam;
//    protected float _nextCheckTurn;
    private List<Vector3> _points;
    private Vector3 _curTarget;
    private int _lastIndex = 0;

    public MoveByWayAction([NotNull] ShipBase owner,List<Vector3> points) 
        : base(owner, ActionType.moveByWay)
    {
        _points = points;
        _lastIndex = 1;
        _curTarget = _points[_lastIndex];
        _minAttackDist = float.MaxValue;
        foreach (var weapon in owner.WeaponsController.GelAllWeapons())
        {
            weapon.OnShootEnd += OnShootEnd;
            if (weapon.AimRadius < _minAttackDist)
                _minAttackDist = weapon.AimRadius;
        }

//        _minAttackDistToStart = _minAttackDist * 1.3f;
//        _minAttackDistToEnd = _minAttackDist * 1.7f;
        _isShootEnd = false;

        var beziePoints = BezieUtils.GetBeziePoints(_owner.LookDirection, points);
        _owner.WayDrawler.DoDraw(beziePoints);
//        Debug.LogError("Start move by action");
    }

    private void UpdatePoints()
    {
        _lastIndex++;
        if (_lastIndex < _points.Count)
        {
            _curTarget = _points[_lastIndex];
        }
        else
        {
//            Debug.LogError("END move by action");
            _isEndWay = true;
        }
    }

    public override void DrawGizmos()
    {
        for (int i = _lastIndex; i < _points.Count - 1; i++)
        {
            var p1 = _points[i];
            var p2 = _points[i + 1];
            Gizmos.color = Color.red;
            Gizmos.DrawLine(p1,p2);

        }
    }

    public override void ManualUpdate()
    {
        var direction4 = _curTarget - _owner.Position;
        var speed = _owner.ApplyRotation(direction4, false);
        _owner.SetTargetSpeed(speed);
//        _owner.MoveByWay(_curTarget);
        var sDist = (_owner.Position - _curTarget).sqrMagnitude;
        if (sDist < SDIST_CLOSE)
        {
            UpdatePoints();
        }
        foreach (var enemy in _owner.Enemies)
        {
            _owner.WeaponsController.CheckWeaponFire(enemy.Value);
        }
    }

    protected override void Dispose()
    {
        _owner.WayDrawler.Clear();
        base.Dispose();
    }

    protected void OnShootEnd(WeaponInGame obj)
    {
        _isShootEnd = true;
    }

    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
//            new CauseAction("out bf", () => !_owner.InBattlefield),
            new CauseAction("way complete", () => _isEndWay),
//            new CauseAction("is Shoot End", () => _isShootEnd),
        };
        return c;
    }
}