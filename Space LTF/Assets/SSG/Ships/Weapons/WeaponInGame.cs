using System;
using UnityEngine;

public enum TargetType
{
    Enemy = 0,
    Ally = 1
}

public enum WeaponType
{
    laser = 1,
    rocket = 2,
    impulse = 3,
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
    fireDamageMine = 20,
    spellRerairDrone = 21,
    beamWaveStrike = 22,
    machineGun = 23,
    vacuumdSpell = 24,
    healBodySupport = 25,
    healShieldSupport = 26,
}

public class WeaponAffectionAdditionalParams
{
}


public abstract class WeaponInGame : IWeapon, IAffectable, IAffectParameters
{
    private const float TargetSpeed = 0.034f;
    private const float DistSpeed = 0.03f;
    private const float TooCloseDist = 1f;

    protected float _bulletTurnSpeed;
    private int _curPeriodShoots;

    public float _delayBetweenShootsSec;
    protected float _fixedDelta;
    private bool _inProcess;
    private readonly bool _isRoundAng;
    public int _level;
    // private bool _nextShootMorePower;
    private float _nextShootTime;
    protected float _radiusShoot;
    private float _sectorCos;

    private readonly WeaponType _weaponType;

    //Params
    public Bullet bulletOrigin;
    private readonly AudioClip Clip;


    // private readonly DebugAimingData DebugAimingData = null;
    private readonly Action<WeaponInGame, Vector3, Bullet> DestroyAction;
    public string Name;
    private readonly Action<ShipBase> ShootDoneAction;
    public Transform ShootPos;
    private AudioSource Source;

    public TargetType TargetType { get; private set; }
    private TestTargetPosition _testTargetPosition;
    public Vector3? PosToAim => _testTargetPosition.PosToAim;

    public WeaponInGame(WeaponInv weaponInv)
    {
        TargetType = weaponInv.TargetType;
        _testTargetPosition = new TestTargetPosition();
        _weaponType = weaponInv.WeaponType;
        Name = weaponInv.Name;
        BulletSpeed = weaponInv.BulletSpeed;
        _fixedDelta = weaponInv.fixedDelta;
        ShootDoneAction = weaponInv.ShootDone;
        DestroyAction = weaponInv.BulletDestroyed;
        _level = weaponInv.Level;
        _bulletTurnSpeed = weaponInv._bulletTurnSpeed;
        AffectAction = new WeaponInventoryAffectTarget(AffectBulletOnShip, weaponInv.TargetType);
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

    public bool IsCrahed { get; private set; }


    public Vector3 GetShootPos => ShootPos.transform.position;

    public CreateBulletDelegate CreateBulletAction { get; set; }
    public WeaponInventoryAffectTarget AffectAction { get; set; }


    public void SetBulletCreateAction(CreateBulletDelegate bulletCreate)
    {
        CreateBulletAction = bulletCreate;
    }

    public float SetorAngle { get; set; }
    public float BulletSpeed { get; set; }
    public float ReloadSec { get; set; }

    //public float SpeedModif = 1f;  
    public int ShootPerTime { get; set; }
    public float AimRadius { get; set; }

    public float CurOwnerSpeed => Owner.CurSpeed;

    public CurWeaponDamage CurrentDamage { get; }

    public ShipBase Owner { get; private set; }

    public TeamIndex TeamIndex => Owner.TeamIndex;

    public virtual void BulletCreateByDir(ShipBase target, Vector3 dir)
    {
        if (target == null)
        {
            CreateBulletWithModif(dir, true);
        }
        else
        {
            CreateBulletWithModif(target);
        }
    }

    public virtual void ApplyToShip(ShipParameters shipParameters, ShipBase shipBase, Bullet bullet)
    {
        AffectTotal(shipParameters, shipBase, bullet, new WeaponAffectionAdditionalParams());
    }

    public void DamageDoneCallback(float healthdelta, float shielddelta, ShipBase damageAppliyer)
    {
        GlobalEventDispatcher.ShipDamage(Owner, healthdelta, shielddelta, _weaponType);
        Owner.ShipInventory.LastBattleData.AddDamage(healthdelta, shielddelta);
        if (damageAppliyer != null)
        {
#if UNITY_EDITOR
            if (damageAppliyer.IsDead == Owner)
                Debug.LogError(
                    $"Strange things. I wanna kill my self??? {Owner.Id}_{Owner.name}  side:{Owner.TeamIndex}  weap:{Name}");
#endif
            if (damageAppliyer.IsDead)
            {
                GlobalEventDispatcher.ShipDeath(damageAppliyer, Owner);
                Owner.ShipInventory.LastBattleData.AddKill();
            }
        }
    }

    public Vector3 CurPosition => ShootPos.position;

    public int Level => _level;


    public virtual void BulletDestroyed(Vector3 position, Bullet bullet)
    {
        DestroyAction(this, position, bullet);
    }

    public event Action<WeaponInGame> OnShootStart;

    public event Action<WeaponInGame> OnShootEnd;

    public void Init(ShipBase owner)
    {
        Owner = owner;
        //        sDistanceShoot = distanceShoot*distanceShoot;
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

    public abstract void AffectBulletOnShip(ShipParameters shipParameters, ShipBase target, Bullet bullet,
        DamageDoneDelegate callback, WeaponAffectionAdditionalParams additional);

    // public void ChargeWeaponsForNextShoot()
    // {
    //     // _nextShootMorePower = true;
    // }

    protected bool IsAimedStraight(IShipData targInfo, ShipBase owner, Vector3 shootStartPos, float distShoot)
    {
        float distCoef = 1f;
        var dist = targInfo.Dist;
        if (dist > TooCloseDist)
        {
            distCoef = dist * DistSpeed;
        }

        float offsetCoef;
        if (targInfo.ShipLink.DamageData.IsEngineWorks)
        {
            offsetCoef = TargetSpeed * distCoef;
        }
        else
        {
            offsetCoef = 0f;
        }

        _testTargetPosition.TestTarget(targInfo.ShipLink.Position, targInfo.ShipLink.LookDirection,
            targInfo.ShipLink.LookRight, shootStartPos, Owner.LookDirection, distShoot, offsetCoef);

        return _testTargetPosition.ShallShoot;
    }

    protected bool IsAimedStraight2(ShipPersonalInfo target, ShipBase owner, Vector3 shootPos,
        float bulletSpeed, float posibleDelta)
    {
        if (target.ShipLink.CurSpeed < 0.001f)
            if (IsInSector(target.DirNorm))
                return true;

        var p1 = target.ShipLink.PredictionPos();
        var p2 = target.ShipLink.Position;

        var shooterPredictionPos = owner.PredictionPos();
        var segmentShip = new SegmentPoints(owner.Position, shooterPredictionPos);
        var segmentTarget = new SegmentPoints(target.ShipLink.Position, target.ShipLink.PredictionPos());
        if (AIUtility.IsParalel(segmentShip, segmentTarget))
        {
#if UNITY_EDITOR
            if (!IsInSector(target.DirNorm)) Debug.LogError("can't aim not in sector IsParalel");
#endif
            return true;
        }


        var crossPointData = AIUtility.IsAimedStraightFindCrossPoint(p1, p2, shootPos, shooterPredictionPos);
        if (crossPointData.HasValue)
        {
            var canShoot = AIUtility.IsAimedStraightBaseOnCrossPoint(crossPointData.Value, bulletSpeed,
                target.ShipLink.CurSpeed, posibleDelta);
            var dirNorm = Utils.NormalizeFastSelf(crossPointData.Value.CrossPoint - shootPos);
            if (canShoot)
            {
#if UNITY_EDITOR
                if (!IsInSector(target.DirNorm))
                    Debug.LogError("can't aim not in sector IsAimedStraightBaseOnCrossPoint");
#endif
                return IsInSector(target.DirNorm);
                //                return IsInSector(dirNorm);
            }

            return false;
        }

        if (IsInSector(target.DirNorm))
        {
            var d = AIUtility.IsAimedStraightByProjectionPoint(p2, shootPos,
                shooterPredictionPos, true);
            if (d) d = AIUtility.IsAimedStraight4(target.ShipLink.LookDirection, owner.LookDirection);
#if UNITY_EDITOR
            if (!IsInSector(target.DirNorm)) Debug.LogError("can't aim not in sector IsAimedStraightByProjectionPoint");
#endif
            return d;
        }

        return false;
    }

    protected void Shoot(ShipBase target)
    {
        //        Debug.Log("Shoot! " + Time.time);
        //        var pos = target.Position;
        _curPeriodShoots = 0;
        if (OnShootStart != null)
            OnShootStart(this);

        _inProcess = true;
        CutTargetShootDir(target);
        for (var i = 1; i < ShootPerTime; i++)
        {
            var timer = MainController.Instance.BattleTimerManager.MakeTimer(_delayBetweenShootsSec * i);
            timer.OnTimer += () =>
            {
                if (!Owner.IsDead)
                    CutTargetShootDir(target);
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
        if (v >= 1f) return 1f;

        return v;
    }

    protected void ShootDir(ShipBase target)
    {
        if (Source != null)
            Source.PlayOneShot(Clip);
        _curPeriodShoots++;
        BulletCreateByDir(target, Owner.LookDirection);
        if (_curPeriodShoots >= ShootPerTime)
        {
            ShootDoneAction(Owner);
            Unload();
            _inProcess = false;
            if (OnShootEnd != null)
                OnShootEnd(this);
        }
    }

    protected virtual void CutTargetShootDir(ShipBase target)
    {
        ShootDir(target);
    }

    protected void CreateBulletWithModif(ShipBase target)
    {
        CreateBulletAction(new BulletTarget(target), bulletOrigin, this, ShootPos.position,
            new BulleStartParameters(BulletSpeed, _bulletTurnSpeed, _radiusShoot, _radiusShoot));
    }

    protected void CreateBulletWithModif(Vector3 direction, bool isDir)
    {
        Vector3 trg;
        if (isDir)
        {
            trg = ShootPos.position + direction;
        }
        else
        {
            trg = direction;
        }

        CreateBulletAction(new BulletTarget(trg), bulletOrigin, this, ShootPos.position,
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
        if (_isRoundAng) return true;

        var isInSector = Utils.IsAngLessNormazied(dir, Owner.LookDirection, _sectorCos);
        //        Debug.Log("isInSector:" + isInSector);
        return isInSector;
    }

    public void ReloadNow()
    {
        _nextShootTime = 0f;
    }

    public abstract bool IsAimed(IShipData target);


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
        if (!IsLoaded() || _inProcess) return false;

        Shoot(target);
        return true;
    }

    public bool IsLoaded()
    {
        if (IsCrahed) return false;
        return _nextShootTime < Time.time;
    }

    public bool IsLoaded(float posibleUnloadSec, out bool fullLoad)
    {
        if (IsCrahed)
        {
            fullLoad = false;
            return false;
        }

        var delta = _nextShootTime - Time.time;
        fullLoad = delta < 0f;
        return delta < posibleUnloadSec;
    }

    public void SetTransform(WeaponPlace transform)
    {
        Source = transform.Source;
        ShootPos = transform.BulletOut;
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
            affectTargetDelegate(shipParameters, target, bullet, DamageDoneCallback, additional);
    }

    public void GizmosDraw()
    {
        if (_testTargetPosition != null)
            _testTargetPosition.OnDrawGizmos();
    }

    public void Dispose()
    {
        OnShootEnd = null;
        OnShootStart = null;
    }
    public void IncreaseReload(float reloadCoefDifWeapons)
    {
        ReloadSec = reloadCoefDifWeapons * ReloadSec;
    }

    public void Unload()
    {
        _nextShootTime = Time.time + ReloadSec;
    }

    public void DropAimPos()
    {
        _testTargetPosition.DropAimPos();
    }

}