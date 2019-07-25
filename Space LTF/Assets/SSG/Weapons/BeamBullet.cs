using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BeamBullet : Bullet
{
    private bool _targetIsDead;
    protected float _deathTime;
    private float _baseDist = 10;
//    public GameObject LineObject;
    public BaseEffectAbsorber ProcessEvent;

    public override void LateInit()
    {
        _deathTime = Time.time + 2f;
        _targetIsDead = false;
        ProcessEvent.Play();
        base.LateInit();
        Target.OnDeath += OnDeathTarget;
    }

    public override BulletType GetType => BulletType.beam;

    private void OnDeathTarget(ShipBase obj)
    {
        _targetIsDead = true;
        obj.OnDeath -= OnDeathTarget;
    }
    
    protected override void ManualUpdate()
    {
        if (!TimeEndCheck())
        {
            MoveTo(Target.Position, Weapon.CurPosition); 
        }
    }

    private void MoveTo(Vector3 target, Vector3 @from)
    {
        ProcessEvent.UpdatePositions(from, target);
    }

    private bool TimeEndCheck()
    {
        if (Time.time > _deathTime || !_targetIsDead)
        {
            Target.GetHit(Weapon,this);
            Death();
            return true;
        }
        return false;
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

