using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class CloseStrikeAction : BaseAction
{
    private ShipBase _attackShip;
    private bool _endStrike;
    private CloseStrikeModul _modul;
    private float _endEndStrike;

    public CloseStrikeAction([NotNull] ShipBase owner, ShipBase attackShip,CloseStrikeModul modul) 
        : base(owner,ActionType.closeStrikeAction)
    {
        _modul = modul;
        _attackShip = attackShip;
        _modul.StartStrike();
        _endEndStrike = Time.time + 1f;
//        _owner.ExternalForce.Init(6f, 2f, _owner.LookDirection);
//        FindWay();
    }

    public override void ManualUpdate()
    {
        Debug.Log("CloseStrikeAction ManualUpdate");
        _owner.SetTargetSpeed(1f);
        _modul.UpdateStrike(_attackShip);
        //        if (_moveWay != null)
        //            _owner.MoveByWay(_moveWay);
//        var dir = _attackShip.Position - _owner.Position;
//        if (dir.sqrMagnitude < 1)
//        {
//            StrikeEnemy();
//        }
    }

    private void StrikeEnemy()
    {
//        _endStrike = true;
//        _attackShip.ShipParameters.DoDamage(3f,DamageType.Both);
//        var dir = Utils.NormalizeFastSelf(_attackShip.Position - _owner.Position);
//        _attackShip.ExternalForce.Init(3f,1.3f, dir);
    }

    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
            new CauseAction("out bf", () => !_owner.InBattlefield),
            new CauseAction("_endEndStrike", () => _endEndStrike < Time.time)
        };
        return c;
    }

    public override void DrawGizmos()
    {

    }
}

