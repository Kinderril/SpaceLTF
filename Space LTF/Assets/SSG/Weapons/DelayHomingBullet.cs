using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class DelayHomingBullet : LinearBullet
{
    public float FirstPartTime = 2f;
    protected float _deathTime;
    protected float _moveTimeEnd;
    private bool moveState = true;
    private bool _targetIsDead;
    public float HomingTimeSec = 8f;
    public float HomigRadius = 12f;
    public float SimpleTurnSpeed = 52f;
    public float HomingPeriodSec = 3f;
    private float _endHomingPeriod;

    private bool _isLeft;
    //    public BaseEffectAbsorber StayEffect;

    public override BulletType GetType => BulletType.delayHoming;

    public override void Init()
    {
        _isActive = false;
        moveState = true;
        base.Init();
        _isLeft = MyExtensions.IsTrueEqual();
//        _deathTime = DestroyPeriod + Time.time;
//        _moveTimeEnd = Time.time + 
    }

    public override void LateInit()
    {
        moveState = true;
        _isActive = false;      
        _moveTimeEnd = Time.time + FirstPartTime;
        base.LateInit();
        SimpleTurnSpeed = MyExtensions.GreateRandom(SimpleTurnSpeed) * MyExtensions.RandomSing();
//        if (StayEffect != null)
//        {
//            StayEffect.gameObject.SetActive(false);
//        }
    }

    protected override void ManualUpdate()
    {
        if (moveState)
        {
            UpdateMove();
        }
        else
        {
            UpdateHoming();
        }
    }


    private void UpdateMove()
    {
        _curTime += Time.deltaTime;
        var p = _curTime / _moveLifeTime;
        DoVisual(p);
        RotateSimple();
        if (_moveTimeEnd < Time.time)
        {
            ActivateHoming();
        }

    }

    private void RotateSimple()
    {


//        var ang = Vector3.Angle(dir, LookDirection);
//        var turnSpeed = TurnSpeed();
        var angPerFrameTurn = (SimpleTurnSpeed * Time.deltaTime);
//        var steps = ang / angPerFrameTurn;

        Vector3 lerpRes;
        if (_isLeft)
        {
            lerpRes = Utils.RotateOnAngUp(LookDirection, angPerFrameTurn);
        }
        else
        {
            lerpRes = Utils.RotateOnAngUp(LookDirection, -angPerFrameTurn);
        }



        Rotation = Quaternion.FromToRotation(Vector3.forward, lerpRes);
    }

    private void ActivateHoming()
    {
        Target = FindTarget(Position);
        if (Target == null)
        {
            Death();
        }
        else
        {
            TrailEffect.Play();
            TrailEffect.StartEmmision();
            _homing = true;
            _endHomingPeriod = Time.time + HomingPeriodSec;
            _isActive = true;
            moveState = false;
            _deathTime = Time.time + HomingTimeSec;
            Target.OnDeath += OnDeathTarget;
        }
    }

    [CanBeNull]
    private ShipBase FindTarget(Vector3 position)
    {
        var opIndex = BattleController.OppositeIndex(Weapon.TeamIndex);
        var all = BattleController.Instance.GetAllShipsInRadius(position, opIndex, HomigRadius);
        if (all.Count > 0)
        {
            return all.RandomElement();
        }
        else
        {
            return null;
        }

    }

    private void OnDeathTarget(ShipBase obj)
    {
        _targetIsDead = true;
        obj.OnDeath -= OnDeathTarget;
    }

    protected override void OnDestroyAction()
    {
        if (Target != null)
        {
            Target.OnDeath -= OnDeathTarget;
        }
    }
    private void UpdateHoming()
    {
        SetTargetSpeed(1f);
        if (!_targetIsDead && _endHomingPeriod > Time.time)
        {
            Approach(Target);
        }
        EngineUpdate();
        ApplyMove();
        TimeEndCheck();
    }
//
//    protected override float TurnSpeed()
//    {
//        renderer
//    }

    private void TimeEndCheck()
    {
        if (Time.time > _deathTime)
        {
            Death();
        }
    }
}

