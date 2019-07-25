using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BeamBullet : Bullet
{
    private bool _targetIsDead;
    private bool _canActivate;
    protected float _deathTime;
    private float _baseDist = 10;
//    public GameObject LineObject;
    public BaseEffectAbsorber ProcessEvent;

    public override void Init()
    {
        _canActivate = false;
        ProcessEvent.gameObject.SetActive(false);
        base.Init();
    }


    public override void LateInit()
    {
        _deathTime = Time.time + 2f;
        _targetIsDead = false;
        base.LateInit();
        _canActivate = true;
        Target.OnDeath += OnDeathTarget;
//        Debug.LogError($"Beam start {Time.time}");
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
            if (_canActivate)
            {
                ProcessEvent.Play();
                _canActivate = false;
                ProcessEvent.gameObject.SetActive(true);
            }
            MoveTo(Target.Position, Weapon.CurPosition); 
        }
    }

    private void MoveTo(Vector3 target, Vector3 @from)
    {

        ProcessEvent.UpdatePositions(from, target);
    }

    private bool TimeEndCheck()
    {
        if (Time.time > _deathTime)
        {
            Target.GetHit(Weapon,this);
            Death();
            return true;
        }

        if (_targetIsDead)
        {
            Death();
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

    public override void Death()
    {
        ProcessEvent.gameObject.SetActive(false);
        _canActivate = false;
        //        Debug.LogError($"Beam dead {Time.time}");
        base.Death();
    }
}

