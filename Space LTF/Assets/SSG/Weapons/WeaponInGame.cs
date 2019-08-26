using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public enum TargetType
{
    Enemy,
    Ally,
}

public enum WeaponType
{
    laser = 1,
    rocket = 2,
    impulse =3,
    casset = 4,
    subMine = 5,
    linerShot = 6,
    staticDamageMine = 7,
    staticSystemMine = 8,
    closeStrike = 9,
    distShot = 10,
    eimRocket = 11,
    castMine = 12,
    randomDamage = 13,
    engineLockSpell = 14,
    shieldOFfSpell = 15,
    throwAroundSpell = 16,
    nextFrame = 17,
    beam = 18,
    artilleryBullet = 19,
}

public class WeaponAffectionAdditionalParams
{
}



public abstract class WeaponInGame : IWeapon, IAffectable,  IAffectParameters
{
    //protected float _bulletTurnSpeed = 36f;

    //Params
    public Bullet bulletOrigin;
    private int _curPeriodShoots;
    public Transform ShootPos;

    public TargetType TargetType;

    //public WeaponInv WeaponData;
    //private float _reloadSec;
    //private float _aimRadius;
    protected float _radiusShoot;
    public float SetorAngle { get; set; }
    public float BulletSpeed { get; set; }
    public float ReloadSec { get; set; }

    public float _delayBetweenShootsSec;

    //public float SpeedModif = 1f;  
    public int ShootPerTime { get; set; }
    public int _level;

    protected float _bulletTurnSpeed;
    protected float _fixedDelta;
    private float _nextShootTime;
    private float _sectorCos;
    public string Name;
    public bool IsCrahed { get; private set; }
    private bool _inProcess;
    private bool _isRoundAng;
    private AudioSource Source;
    private AudioClip Clip;

    public event Action<WeaponInGame> OnShootStart;

    public event Action<WeaponInGame> OnShootEnd;

//    public event Action<WeaponInGame, bool> OnCrashed;
    private DebugAimingData DebugAimingData = null;
    public CreateBulletDelegate CreateBulletAction { get; set; }
    public WeaponInventoryAffectTarget AffectAction { get; set; }
    private Action<WeaponInGame, Vector3, Bullet> DestroyAction;
    private Action<ShipBase> ShootDoneAction;
                                                  
    public float CurOwnerSpeed
    {
        get { return Owner.CurSpeed; }
    }

    public CurWeaponDamage CurrentDamage { get; private set; }
    public float AimRadius { get; set; }

    public ShipBase Owner { get; private set; }

    public TeamIndex TeamIndex => Owner.TeamIndex;


    public Vector3 GetShootPos
    {
        get { return ShootPos.transform.position; }
    }

    public WeaponInGame(WeaponInv weaponInv)
    {
        Name = weaponInv.Name;
        BulletSpeed = weaponInv.BulletSpeed;
        _fixedDelta = weaponInv.fixedDelta;
        ShootDoneAction = weaponInv.ShootDone;
        DestroyAction = weaponInv.BulletDestroyed;
        _level = weaponInv.Level;
        _bulletTurnSpeed = weaponInv._bulletTurnSpeed;
        AffectAction = new WeaponInventoryAffectTarget(Affect);
        _isRoundAng = weaponInv.isRoundAng;
        CreateBulletAction = weaponInv.BulletCreate;
        ShootPerTime = weaponInv.ShootPerTime;
        _delayBetweenShootsSec = weaponInv.delayBetweenShootsSec;
        AimRadius = weaponInv.AimRadius;
        _radiusShoot = weaponInv._radiusShoot;
        SetorAngle = weaponInv.sectorAngle;
        ReloadSec = weaponInv.ReloadSec;
        CurrentDamage = weaponInv.CurrentDamage;
        bulletOrigin = DataBaseController.Instance.GetBullet(weaponInv.WeaponType);
        Clip = DataBaseController.Instance.AudioDataBase.ShotWeapon(weaponInv.WeaponType);
    }

    public void Init(ShipBase owner)
    {
        Owner = owner;
        //        sDistanceShoot = distanceShoot*distanceShoot;
    }


    public void SetBulletCreateAction(CreateBulletDelegate bulletCreate)
    {
        CreateBulletAction = bulletCreate;
    }

    public void SetCost(float sectorCos)
    {
        _sectorCos = sectorCos;
    }

    public void CrashReload(bool val)
    {
//        if (val)
//        {
////            _reloadCoef = 2f;
//        }
//        else
//        {
////            _reloadCoef = 1f;
//        }
        IsCrahed = val;
//        if (OnCrashed != null)
//        {
//            OnCrashed(this, val);
//        }
    }

    public void Affect(ShipParameters shipParameters, ShipBase target, Bullet bullet,
        DamageDoneDelegate callback, WeaponAffectionAdditionalParams additional)
    {
        shipParameters.Damage(CurrentDamage.ShieldDamage, CurrentDamage.BodyDamage,callback, target);
    }

    protected bool IsAimedStraight(ShipPersonalInfo target, ShipBase owner, Vector3 shootPos,
        float bulletSpeed, float posibleDelta)
    {
        if (target.ShipLink.CurSpeed < 0.001f)
        {
            if (IsInSector(target.DirNorm))
            {
                return true;
            }
        }

        var p1 = target.ShipLink.PredictionPos();
        var p2 = target.ShipLink.Position;

        var shooterPredictionPos = owner.PredictionPos();
        var segmentShip = new SegmentPoints(owner.Position, shooterPredictionPos);
        var segmentTarget = new SegmentPoints(target.ShipLink.Position, target.ShipLink.PredictionPos());
        if (AIUtility.IsParalel(segmentShip, segmentTarget))
        {
#if UNITY_EDITOR
            if (!IsInSector(target.DirNorm))
            {
                Debug.LogError("can't aim not in sector IsParalel");
            }
#endif
            return true;
        }


        var crossPointData = AIUtility.IsAimedStraightFindCrossPoint(p1, p2, shootPos, shooterPredictionPos, false);
        if (crossPointData.HasValue)
        {
            bool canShoot = AIUtility.IsAimedStraightBaseOnCrossPoint(crossPointData.Value, bulletSpeed,
                target.ShipLink.CurSpeed, posibleDelta);
            var dirNorm = Utils.NormalizeFastSelf(crossPointData.Value.CrossPoint - shootPos);
            if (canShoot)
            {
#if UNITY_EDITOR
                if (!IsInSector(target.DirNorm))
                {
                    Debug.LogError("can't aim not in sector IsAimedStraightBaseOnCrossPoint");
                }
#endif
                return IsInSector(target.DirNorm);
//                return IsInSector(dirNorm);
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (IsInSector(target.DirNorm))
            {
                var d = AIUtility.IsAimedStraightByProjectionPoint(p2, shootPos,
                    shooterPredictionPos, true);
                if (d)
                {
                    d = AIUtility.IsAimedStraight4(target.ShipLink.LookDirection, owner.LookDirection);
                }
#if UNITY_EDITOR
                if (!IsInSector(target.DirNorm))
                {
                    Debug.LogError("can't aim not in sector IsAimedStraightByProjectionPoint");
                }
#endif
                return d;
            }
            else
            {
                return false;
            }
        }
    }

    protected void Shoot(ShipBase target)
    {
//        Debug.Log("Shoot! " + Time.time);
//        var pos = target.Position;
        _curPeriodShoots = 0;
        if (OnShootStart != null)
        {
            OnShootStart(this);
        }

        _inProcess = true;
        ShootDir(target);
        for (var i = 1; i < ShootPerTime; i++)
        {
            var timer = MainController.Instance.BattleTimerManager.MakeTimer(_delayBetweenShootsSec * i);
            timer.OnTimer += () =>
            {
                if (!Owner.IsDead)
                {
                    ShootDir(target);
                }
            };
        }

        if (ReloadSec < _delayBetweenShootsSec * (ShootPerTime - 1))
        {
            Debug.LogError("wrong weapons settings. ReloadSec is less than delayBetweenShootsSec");
            ReloadSec = _delayBetweenShootsSec * (ShootPerTime - 1) + 0.1f;
        }
    }

    public float PercentLoad()
    {
        var v = (_nextShootTime - Time.time) / ReloadSec;
        if (v >= 1f)
        {
            return 1f;
        }

        return v;
    }

    protected virtual void ShootDir(ShipBase target)
    {
        if (Source != null)
        {
            Source.PlayOneShot(Clip);
        }
        _curPeriodShoots++;
        BulletCreateByDir(target, Owner.LookDirection);
        if (_curPeriodShoots >= ShootPerTime)
        {
            ShootDoneAction(Owner);
            _nextShootTime = Time.time + ReloadSec;
            _inProcess = false;
            if (OnShootEnd != null)
            {
                OnShootEnd(this);
            }
        }
    }

    public virtual void BulletCreateByDir(ShipBase target, Vector3 dir)
    {
        CreateBulletWithModif(target);
    }

    protected void CreateBulletWithModif(ShipBase target)
    {
        CreateBulletAction(new BulletTarget(target), bulletOrigin, this, ShootPos.position,
            new BulleStartParameters(BulletSpeed, _bulletTurnSpeed, _radiusShoot, _radiusShoot));
    }

    protected void CreateBulletWithModif(Vector3 target)
    {
        CreateBulletAction(new BulletTarget(target), bulletOrigin, this, ShootPos.position,
            new BulleStartParameters(BulletSpeed, _bulletTurnSpeed, _radiusShoot, _radiusShoot));
    }

    public bool IsInRadius(float dist)
    {
        var isInRadius = dist < AimRadius;
//        Debug.Log("isInRadius:" + isInRadius + "   curDist:" + dist);
        return isInRadius;
    }

    public bool IsInSector(Vector3 dir)
    {
        if (_isRoundAng)
        {
            return true;
        }

        var isInSector = Utils.IsAngLessNormazied(dir, Owner.LookDirection, _sectorCos);
//        Debug.Log("isInSector:" + isInSector);
        return isInSector;
    }

    public virtual void ApplyToShip(ShipParameters shipParameters, ShipBase shipBase, Bullet bullet)
    {
        AffectTotal(shipParameters, shipBase, bullet, new WeaponAffectionAdditionalParams());
    }

    public void DamageDoneCallback(float healthdelta, float shielddelta, ShipBase damageAppliyer)
    {
        Owner.ShipInventory.LastBattleData.AddDamage(healthdelta, shielddelta);
        if (damageAppliyer != null)
        {
#if UNITY_EDITOR
            if (damageAppliyer.IsDead == Owner)
            {
                Debug.LogError($"Strange things. I wanna kill my self??? {Owner.Id}_{Owner.name}");
            }
#endif
            if (damageAppliyer.IsDead)
            {
                Owner.ShipInventory.LastBattleData.AddKill();
            }
        }

    }

    public void ReloadNow()
    {
        _nextShootTime = 0f;
    }

    public abstract bool IsAimed(ShipPersonalInfo target);
    

    public void UpgradeWithModul(BaseModulInv modul)
    {
        var support = modul as BaseSupportModul;
        if (support != null)
        {
            support.ChangeBullet(this);
            support.ChangeTargetAffect(this);
            support.ChangeParams(this);
        }
    }

    public bool TryShoot(Vector3 dir, ShipBase target)
    {
        if (!IsLoaded() || _inProcess)
        {
            return false;
        }

        Shoot(target);
        return true;
    }

    public bool IsLoaded()
    {
        if (IsCrahed)
        {
            return false;
        }

        return _nextShootTime < Time.time;
    }

    public void SetTransform(WeaponPlace transform)
    {
//        Clip = DataBaseController.Instance.AudioDataBase.GetShot(we)
        Source = transform.Source;
            ShootPos = transform.BulletOut;
    }

    public Vector3 CurPosition
    {
        get { return ShootPos.position; }
    }

    public int Level
    {
        get { return _level; }
    }
    public void AddAffectTargtAction(AffectTargetDelegate affectTarget)
    {
        AffectAction.Add(affectTarget);
    }

    public void CacheAngCos()
    {
        _sectorCos = Mathf.Cos(SetorAngle * Mathf.Deg2Rad / 2f);
    }

    public void AffectTotal(ShipParameters shipParameters, ShipBase target, Bullet bullet,
        WeaponAffectionAdditionalParams additional)
    {
        AffectAction.Main(shipParameters, target, bullet, DamageDoneCallback, additional);
        foreach (var affectTargetDelegate in AffectAction.Additional)
        {
            affectTargetDelegate(shipParameters, target, bullet, DamageDoneCallback, additional);
        }
    }


    public virtual void BulletDestroyed(Vector3 position, Bullet bullet)
    {
        DestroyAction(this, position, bullet);
    }

    public void GizmosDraw()
    {
        if (DebugAimingData != null)
        {
            DebugAimingData.GizmosDraw();
        }
    }

    public void Dispose()
    {
        OnShootEnd = null;
        OnShootStart = null;
    }

}