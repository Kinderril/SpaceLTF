﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum BulletAffectType
{
    damage = 0,
    repair = 1,
}

public enum BulletDamageType
{
    physical,
    energy,
}

public enum BulletType
{
    linear = 0,
    homing = 1,
    stay = 2,
    beam = 3,
    upDown = 4,
    nextFrame = 5,
    delayHoming = 6,
}

[System.Serializable]
public class BulleStartParameters
{
    public BulleStartParameters(
        float bulletSpeed,
        float turnSpeet,
        float distanceShoot,
        float radiusShoot
    )
    {
        this.bulletSpeed = bulletSpeed;
        this.turnSpeed = turnSpeet;
        this.distanceShoot = distanceShoot;
        this.radiusShoot = radiusShoot;
    }

    public float bulletSpeed;
    public float turnSpeed;
    public float distanceShoot;
    public float radiusShoot;
}

public abstract class Bullet : MovingObject
{
    [SerializeField]
    public int ID;
    public GameObject Visual;
    protected Vector3 _endPos;
    protected Vector3 _startPos;
    protected float _maxSpeed = 0f;
    protected float _startTime;

    public float StartTime
    {
        get { return _startTime; }
    }

    protected float _turnSpeed;
    protected float _curTime;
    protected float _distanceShoot;
    public BulletAffectType AffectTypeHit;
    //    private float _endTime;
    public IWeapon Weapon { get; private set; }
    public bool IsAcive { get;private set; }

    public ShipBase Target;
    protected bool _homing = false;
    public bool EffectOnHit = true;
    public bool DeathOnHit = true;
    public bool _hitted = false;
    public bool _isActive = false;
    public BulletDamageType DamageType = BulletDamageType.energy;

    public WeaponType WeaponType;
    public BaseEffectAbsorber HitEffect;
    public BaseEffectAbsorber TrailEffect;

    protected override float TurnSpeed()
    {
        return _turnSpeed;
    }
    
    public override float MaxSpeed()
    {
        return _maxSpeed;
    }

    public Vector3 CurDir()
    {
        return _endPos - _startPos;
    }

    public virtual void LateInit()
    {
        transform.position = _startPos;
        base.Init();
        _hitted = false;
        IsAcive = true;
        if (TrailEffect != null)
        {
            TrailEffect.Play();
        }
        if (TrailEffect != null)
        {
            TrailEffect.StartEmmision();
        }

        if (HitEffect != null)
        {
            HitEffect.Stop();
        }
    }   

    public abstract BulletType GetType { get; }

    public static Bullet Create(Bullet origin, IWeapon weapon, Vector3 dir, Vector3 position,
        ShipBase target, BulleStartParameters bulleStartParameters)
    {
        //bulleStartParameters = weapon.ModifyParameters(bulleStartParameters);
        return Create(origin, weapon, dir, position, target, bulleStartParameters.bulletSpeed,
            bulleStartParameters.turnSpeed, bulleStartParameters.distanceShoot);
    }

    private static Bullet Create(Bullet origin, IWeapon weapon, Vector3 dir, Vector3 position,
        ShipBase target, float bulletSpeed, float turnSpeed, float distanceShoot)
    {
#if UNITY_EDITOR
        if (origin == null)
        {
            Debug.LogError("WTF!!!!  BUTTEL ORIGIN IS NULL!!!");
            return null;
        }
#endif
        var bullet = DataBaseController.Instance.Pool.GetBullet(origin.ID);
        switch (origin.GetType)
        {
            case BulletType.beam:
                bullet.InitBeam(weapon,  position,target, bulletSpeed);
                break;
            case BulletType.upDown:
                bullet.InitLinear(weapon, dir, position, distanceShoot, bulletSpeed);
                break;
            case BulletType.linear:
                bullet.InitLinear(weapon, dir, position, distanceShoot, bulletSpeed);
                break;
            case BulletType.homing:
                bullet.InitHoming(weapon, dir, position, target, bulletSpeed, turnSpeed, distanceShoot);
                break;
            case BulletType.stay:
                bullet.InitStay(weapon, dir, position, distanceShoot, bulletSpeed);
                break;
            case BulletType.nextFrame:
                bullet.InitNextFrame(weapon, dir, position, distanceShoot, bulletSpeed);
                break;
            case BulletType.delayHoming:
                bullet.InitDelayHoming(weapon, dir, position, distanceShoot, bulletSpeed, turnSpeed);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
//        var isLinear = origin is LinearBullet;
        bullet.LateInit();
        var par = BattleController.Instance.BulletContainer;
        bullet.transform.SetParent(par);
        BattleController.Instance.AddBullet(bullet);

        return bullet;
    }

    private void InitBeam(IWeapon weapon,  Vector3 position, ShipBase target, float bulletSpeed)
    {
        _curSpeed = weapon.CurOwnerSpeed / 2f;
        _startTime = Time.time;
//        _turnSpeed = turnSpeed;
        _curSpeed = 0f;
        _maxSpeed = bulletSpeed * (1 + (weapon.Level - 1) * 0.1f);
        _homing = true;
        Weapon = weapon;
        Target = target;
        _startPos = position;
        Position = position;
        _startTime = Time.time;
        _isActive = true;
        //        if (dir != Vector3.zero)
//        Rotation = Quaternion.LookRotation(dir);
    }

    private void InitStay(IWeapon weapon, Vector3 dir, Vector3 position, float distanceShoot, float bulletSpeed)
    {
        _curTime = 0;
        _curSpeed = _maxSpeed = bulletSpeed;
#if UNITY_EDITOR
        if (_curSpeed <= 0)
        {
            Debug.LogError("wrong bullet speed " + gameObject.name);
        }
#endif
        _homing = false;
        Weapon = weapon;
        _startTime = Time.time;
        _startPos = position;
        transform.position = _startPos;
        _endPos = _startPos + Utils.NormalizeFastSelf(dir) * distanceShoot;
        _distanceShoot = distanceShoot;
        Rotation = Quaternion.LookRotation(dir);

    }

    private void InitDelayHoming(IWeapon weapon, Vector3 dir, Vector3 position, float distanceShoot, float bulletSpeed, float turnSpeed)
    {
        _turnSpeed = turnSpeed;
        _curTime = 0;
        _curSpeed = _maxSpeed = bulletSpeed;
#if UNITY_EDITOR
        if (_curSpeed <= 0)
        {
            Debug.LogError("wrong bullet speed " + gameObject.name);
        }
#endif
        Weapon = weapon;
        _startTime = Time.time;
        _startPos = position;
        transform.position = _startPos;
        _endPos = _startPos + Utils.NormalizeFastSelf(dir) * distanceShoot;
        _distanceShoot = distanceShoot;
        Rotation = Quaternion.LookRotation(dir);

    }

    private void InitNextFrame(IWeapon weapon, Vector3 dir, Vector3 position, float distanceShoot, float bulletSpeed)
    {
        _curTime = 0;
        _curSpeed = _maxSpeed = bulletSpeed;
#if UNITY_EDITOR
        if (_curSpeed <= 0)
        {
            Debug.LogError("wrong bullet speed " + gameObject.name);
        }
#endif
        _homing = false;
        IsAcive = true;
        Weapon = weapon;
        _startTime = Time.time;
        _startPos = position;
        _endPos = _startPos + Utils.NormalizeFastSelf(dir) * distanceShoot;
        _distanceShoot = distanceShoot;

        Rotation = Quaternion.LookRotation(dir);

    }

    private void InitLinear(IWeapon weapon, Vector3 dir, Vector3 position, float distanceShoot, float bulletSpeed)
    {
        _curTime = 0;
        _curSpeed = _maxSpeed = bulletSpeed;
//        _turnSpeed = turnSpeed;
#if UNITY_EDITOR
        if (_curSpeed <= 0)
        {
            Debug.LogError("wrong bullet speed " + gameObject.name);
        }

        if (distanceShoot <= 0)
        {
            Debug.LogError("bullet wrong dist");
        }
        if (bulletSpeed <= 0)
        {
            Debug.LogError("bullet wrong bulletSpeed");
        }
#endif
        _homing = false;
        Weapon = weapon;
        _startTime = Time.time;
//        _endTime = _startTime + _lifeTimeSec;
        _startPos = position;
        _endPos = _startPos + Utils.NormalizeFastSelf(dir)*distanceShoot;
        _distanceShoot = distanceShoot;
        _isActive = true;
        Rotation = Quaternion.LookRotation(dir);
    }

    private void InitHoming(IWeapon weapon, Vector3 dir, Vector3 position, ShipBase target, float bulletSpeed, float turnSpeed, float distanceShoot)
    {
#if UNITY_EDITOR
        if (target == null)
        {
            Debug.LogError("Homing bullet without target");
            return;
        }
#endif
        _curSpeed = weapon.CurOwnerSpeed/2f;
        _startTime = Time.time;
        _turnSpeed = turnSpeed;
        _curSpeed = 0f;
        _maxSpeed = bulletSpeed * (1 + (weapon.Level-1)*0.1f);
        _homing = true;
        Weapon = weapon;
        Target = target;
        _startPos = position;
        Position = position;
        _startTime = Time.time;
        _isActive = true;
        //        if (dir != Vector3.zero)
        Rotation = Quaternion.LookRotation(dir);

    }

    protected virtual void ManualUpdate()
    {
    }

    void Update()
    {
        ManualUpdate();
    }


    protected virtual bool IsCatch(ShipHitCatcher s)
    {
        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        OnBulletHit(other);
    }


    private void OnBulletHit(Collider other)
    {
        if (!_isActive)
        {
            return;
        }
        if (!EffectOnHit)
        {
            return;
        }
        if (other.CompareTag(TagController.OBJECT_MOVER_TAG))
        {
            var ship = other.GetComponent<ShipHitCatcher>();

            if (ship != null && IsCatch(ship))
            {
                switch (AffectTypeHit)
                {
                    case BulletAffectType.repair:
                        if (ship.TeamIndex != Weapon.TeamIndex)
                        {
                            return;
                        }

                        if (ship.ShipBase == Weapon.Owner)
                        {
                            return;
                        }
//                        PlayHitEffect(other);
                        ship.GetHit(Weapon, this);
                        if (DeathOnHit)
                        {
                            Death();
                        }
                        break;
                    case BulletAffectType.damage:
                        if (ship.TeamIndex == Weapon.TeamIndex)
                        {
                            return;
                        }

//                        PlayHitEffect(other);
                        ship.GetHit(Weapon, this);
                        if (DeathOnHit)
                        {
                            Death();
                        }
                        break;
                }
            }
        }
    }

    public virtual void Death()
    {
        float delay = 1.7f;
        if (TrailEffect != null)
        {
            TrailEffect.StopEmmision();
            EffectController.Instance.LeaveEffect(TrailEffect, transform, delay);
        }

        if (!_hitted)
        {
            if (HitEffect != null)
            {
                HitEffect.Play();
                HitEffect.transform.position = transform.position;
                EffectController.Instance.LeaveEffect(HitEffect, transform, delay);
            }
        }
        IsAcive = false;
        Weapon.BulletDestroyed(transform.position, this);
        EndUse(delay);
//        GameObject.Destroy(gameObject);
    }


    protected override void DrawGizmos()
    {
        //        DrawUtils.GizmosArrow(transform.position, LookDirection * 2, Color.red);
    }
}

