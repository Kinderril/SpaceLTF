using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class StayBullet : LinearBullet
{
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
        moveState = true;
        base.Init();
    }

    public override void LateInit()
    {
        moveState = true;
        _isActive = false;  
        base.LateInit();
        _moveTimeEnd = Time.time + _moveLifeTime;
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

