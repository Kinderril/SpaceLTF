using System;
using UnityEngine;

[System.Serializable]
public class CurWeaponDamage
{
    public CurWeaponDamage(float ShieldDamage, float BodyDamage)
    {
        this.ShieldDamage = ShieldDamage;
        this.BodyDamage = BodyDamage;
    }

    public float ShieldDamage;
    public float BodyDamage;

    public CurWeaponDamage Copy()
    {
        return new CurWeaponDamage(ShieldDamage, BodyDamage);
    }
}

public class WeaponInventoryParameters
{
    public float shieldDamage;
    public float bodyDamage;
    public float shieldPreLevelDamage;
    public float bodyPreLevelDamage;
    public float sectorAngle;
    public float turnSpeed;
    public float reloadSec;
    public float delayBetweenShootsSec;
    public int shootPerTime;
    public float _bulletSpeed;
    public float AimRadius;
    public TargetType TargetType;


    public WeaponInventoryParameters(int shieldDamage, int bodyDamage, float shieldPreLevelDamage, float bodyPreLevelDamage, float sectorAngle, float reloadSec,
        float delayBetweenShootsSec,
        int shootPerTime, float _bulletSpeed, float AimRadius, float turnSpeed, TargetType targetType)
    {
        TargetType = targetType;
        this.shieldPreLevelDamage = shieldPreLevelDamage;
        this.bodyPreLevelDamage = bodyPreLevelDamage;
        this.AimRadius = AimRadius;
        this._bulletSpeed = _bulletSpeed;
        this.turnSpeed = turnSpeed;
        this.shootPerTime = shootPerTime;
        this.delayBetweenShootsSec = delayBetweenShootsSec;
        this.reloadSec = reloadSec;
        this.sectorAngle = sectorAngle;
        this.bodyDamage = bodyDamage;
        this.shieldDamage = shieldDamage;
    }
}

public enum UpgradesDesc
{
    taken,
    dismissed,
    future,
}

[System.Serializable]
public abstract class WeaponInv : IItemInv, IAffectParameters
{
    private const float MAX_DELTA = .2f;
    public float _bulletTurnSpeed = 36f;

    private float _shieldPreLevelDamage = 0f;
    private float _bodyPreLevelDamage = 0f;
    private float _shieldDamage = 0f;
    private float _bodyDamage = 0f;

    private float _bulletSpeed = 0.3f;
    public float _aimRadius;
    public int Level;
    public float _radiusShoot;
    public float sectorAngle;
    public float delayBetweenShootsSec;
    //    public int shootPerTime;
    public WeaponType WeaponType;
    public TargetType TargetType = TargetType.Enemy;
    public bool isRoundAng;
    public readonly float fixedDelta;
    // private CurWeaponDamage _currentDamage;
    // private CurWeaponDamage _currentDamage1;
    public string Name { get; set; }

    public float SetorAngle { get; set; }
    //    float IAffectParameters.BulletSpeed { get; set; }

    public float BulletSpeed
    {
        get => _bulletSpeed;
        set => _bulletSpeed = value;
    }

    public float ReloadSec { get; set; }
    public int ShootPerTime { get; set; }

    public CurWeaponDamage CurrentDamage => new CurWeaponDamage(ShieldDamage, BodyDamage);


    [field: NonSerialized] public event Action<WeaponInv> OnUpgrade;

    //protected Dictionary<int, Dictionary<WeaponUpdageType, WeaponUpgradeData>> _levelUpDependences;

    private float ShieldDamage => _shieldDamage + (Level - 1) * _shieldPreLevelDamage;

    private float BodyDamage => _bodyDamage + (Level - 1) * _bodyPreLevelDamage;

    public ItemType ItemType => ItemType.weapon;

    public IInventory CurrentInventory { get; set; }

    public WeaponInv(WeaponInventoryParameters parameters,
        WeaponType WeaponType, int Level)
    {
        _shieldPreLevelDamage = parameters.shieldPreLevelDamage;
        _shieldDamage = parameters.shieldDamage;
        _bodyPreLevelDamage = parameters.bodyPreLevelDamage;
        _bodyDamage = parameters.bodyDamage;
        this.ShootPerTime = parameters.shootPerTime;
        this._bulletSpeed = parameters._bulletSpeed;
        this._bulletTurnSpeed = parameters.turnSpeed;
        this.ReloadSec = parameters.reloadSec;
        this.delayBetweenShootsSec = parameters.delayBetweenShootsSec;
        this._aimRadius = parameters.AimRadius;
        TargetType = parameters.TargetType;
        this._radiusShoot = _aimRadius * 1.6f;
        this.sectorAngle = parameters.sectorAngle;
        //        this.MaxCharges = MaxCharges;
        //        this.RemainCharges = RemainCharges;
        this.WeaponType = WeaponType;
        this.Level = Level;
        isRoundAng = sectorAngle >= 360;

        if (delayBetweenShootsSec * ShootPerTime > ReloadSec)
        {
            Debug.LogError("wrong shoot times: delayBetweenShootsSec:" + delayBetweenShootsSec +
                           " ReloadSec:" + ReloadSec +
                           "  shootPerTime:" + ShootPerTime);
        }

        Name = Namings.Weapon(WeaponType);
        if (ShootPerTime > 1)
        {
            fixedDelta = MAX_DELTA * (ShootPerTime / 2f);
        }
        else
        {
            fixedDelta = MAX_DELTA;
        }

        //_levelUpDependences = CreateLevelUpDependences();
    }

    public int CostValue => MoneyConsts.BASE_WEAPON_MONEY_COST + MoneyConsts.LEVEL_WEAPON_MONEY_COST * (Level - 1);

    public int RequireLevel(int posibleLevel = -1)
    {
        if (posibleLevel > 0)
        {
            return RequireLevelByLevel(posibleLevel);
        }
        else
        {
            return RequireLevelByLevel(Level);
        }
    }

    public string GetInfo()
    {
        return Name + " (" + Level + ")";
    }

    public string DamageInfo()
    {
        var dmg = CurrentDamage;
        return Namings.Format("Damage: Shield:{0}. Body:{1}", dmg.ShieldDamage, dmg.BodyDamage);
    }

    private int RequireLevelByLevel(int lvl)
    {
        return 1 + (lvl - 1) * Library.WEAPON_REQUIRE_LEVEL_COEF;
    }

    public string WideInfo()
    {
        var info = GetInfo() + "\n" + DamageInfo();
        return info;
    }

    //    CurWeaponDamage IAffectParameters.CurrentDamage => _currentDamage;

    //    float IAffectParameters.AimRadius { get; set; }


    public float AimRadius
    {
        get { return _aimRadius; }
        set { _aimRadius = value; }
    }

    public virtual void BulletCreate(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos,
        BulleStartParameters bulleStartParameters)
    {
        //        var dirToShoot = Owner.LookDirection;

        var dirToShoot = target.IsDir ? target.Position : target.Position - shootPos;
        var b = Bullet.Create(origin, weapon, dirToShoot, shootPos, target.target, bulleStartParameters);
    }


    //    public virtual void Affect(ShipParameters shipParameters, ShipBase target, Bullet bullet, DamageDoneDelegate callback, WeaponAffectionAdditionalParams additional)
    //    {
    ////        var damage = CurrentDamage();
    //        shipParameters.Damage(damage.ShieldDamage, damage.BodyDamage, callback);
    //    }
    public abstract WeaponInGame CreateForBattle();

    public virtual void BulletDestroyed(IWeapon weapon, Vector3 position, Bullet bullet)
    {
    }

    public virtual void ShootDone(ShipBase ship)
    {
    }

    public void TryUpgrade()
    {
        var owner = CurrentInventory.Owner;
        if (CanUpgrade())
        {
            if (CurrentInventory.CanMoveToByLevel(this, Level + 1))
            {
                if (MoneyConsts.WeaponUpgrade.ContainsKey(Level))
                {
                    var cost = MoneyConsts.WeaponUpgrade[Level];
                    if (owner.MoneyData.HaveMoney(cost))
                    {
                        var txt = Namings.Format(Namings.Tag("WANT_UPGRADE_WEAPON"), Namings.Weapon(WeaponType));
                        WindowManager.Instance.ConfirmWindow.Init(() =>
                        {
                            WindowManager.Instance.UiAudioSource.PlayOneShot(DataBaseController.Instance.AudioDataBase.Upgrade);
                            owner.MoneyData.RemoveMoney(cost);
                            GlobalEventDispatcher.UpgradeWeapon(this);
                            Upgrade(true);
                            //                        WindowManager.Instance.InfoWindow.Init(null, "Upgrade completed");
                        }, null, txt);
                    }
                    else
                    {
                        WindowManager.Instance.NotEnoughtMoney(cost);
                    }
                }
                else
                {
                    Debug.LogError("don't have money consts to upgrade");
                }
            }
            else
            {
                if (CurrentInventory is ShipInventory shipInv)
                {
                    int pilotLevel = shipInv.PilotParameters.CurLevel;
                    int targetLevel = this.RequireLevel(Level + 1);
                    WindowManager.Instance.InfoWindow.Init(null, Namings.Format(Namings.Tag("CanCauseNoLevel"), pilotLevel, targetLevel));
                }
            }

        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null, Namings.Tag("WeaponMaxLevel"));
        }
    }

    public bool CanUpgrade()
    {
        return Level < Library.MAX_WEAPON_LVL;
    }


    public void Upgrade(bool byPlayer)
    {
        if (CanUpgrade())
        {
            Level++;
            if (byPlayer && Level == Library.MAX_WEAPON_LVL)
            {
                MainController.Instance.Statistics.AddMaxLevelWeapons();
            }
            if (OnUpgrade != null)
            {
                OnUpgrade(this);
            }
        }
    }
}