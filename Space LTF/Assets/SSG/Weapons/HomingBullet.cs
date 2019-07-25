using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class HomingBullet : Bullet
{
    private bool _targetIsDead;
    protected float _deathTime;

    public override void LateInit()
    {
        _deathTime = Time.time + 8f;
        _targetIsDead = false;
        base.LateInit();
        Target.OnDeath += OnDeathTarget;
    }

    public override BulletType GetType
    {
        get
        {
            return BulletType.homing;
        }
    }

    private void OnDeathTarget(ShipBase obj)
    {
        _targetIsDead = true;
        obj.OnDeath -= OnDeathTarget;
    }
    
    protected override void ManualUpdate()
    {
        SetTargetSpeed(1f);
        if (!_targetIsDead)
        {
            Approach(Target);
        }
        EngineUpdate();
        ApplyMove();
        TimeEndCheck();
    }

    private void TimeEndCheck()
    {
        if (Time.time > _deathTime)
        {
            Death();
        }
    }


    protected override void DrawGizmosSelected()
    {
        if (Target != null)
            Gizmos.DrawLine(Position,Target.Position);
        base.DrawGizmosSelected();
    }

    protected override bool IsCatch(ShipHitCatcher s)
    {
        if (s.CatcherType == HitCatcherType.shield)
        {
            return false;
        }
        return base.IsCatch(s);
    }

    protected override void OnDestroyAction()
    {
        if (Target != null)
        {
            Target.OnDeath -= OnDeathTarget;
        }
    }
}

