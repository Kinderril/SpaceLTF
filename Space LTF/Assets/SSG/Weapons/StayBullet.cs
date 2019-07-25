using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class StayBullet : LinearBullet
{
//    public float DestroyPeriod = 1f;
//    public bool ActivateOnStart = false;
    public float StayTime = MineFieldSpell.MINES_PERIOD;
    protected float _deathTime;
    protected float _moveTimeEnd;
    private bool moveState = true;
    public BaseEffectAbsorber StayEffect;

    public override BulletType GetType
    {
        get { return BulletType.stay;}
    }

    public override void Init()
    {
        _isActive = false;
        base.Init();
        moveState = true;
//        _deathTime = DestroyPeriod + Time.time;
//        _moveTimeEnd = Time.time + 
    }

    public override void LateInit()
    {
        //        _isActive = ActivateOnStart;
        //        moveState = !ActivateOnStart;       
        _moveTimeEnd = Time.time + _moveLifeTime;
//        if (_isActive)            
//        {
//            _deathTime = StayTime + Time.time;
////            Debug.Log($"Inited  _deathTime: {_deathTime}.  Time:{Time.time}");
//        }
//        else
//        {
////            _deathTime = DestroyPeriod + Time.time;
//        }
        base.LateInit();
        if (StayEffect != null)
        {
            StayEffect.gameObject.SetActive(false);
        }
    }

    protected override void ManualUpdate()
    {
        if (moveState)
        {
            UpdateMove();
        }
        else
        {
            UpdateStay();
        }
    }


    private void UpdateMove()
    {
        _curTime += Time.deltaTime;
        var p = _curTime / _moveLifeTime;
        DoVisual(p);
        if (_moveTimeEnd < Time.time)
        {
            _isActive = true;
            moveState = false;
            _deathTime = Time.time + StayTime;
            if (TrailEffect != null)
            {
                TrailEffect.Stop();
            }
            if (StayEffect != null)
            {
                StayEffect.gameObject.SetActive(true);
            }
        }

    }
    private void UpdateStay()
    {
        if (_deathTime < Time.time)
        {
            Death();
        }
    }
}

