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
}

public class WeaponInventoryParameters
{
    public float shieldDamage;
    public float bodyDamage;
    public float sectorAngle;
    public float turnSpeed;
    public float reloadSec;
    public float delayBetweenShootsSec;
    public int shootPerTime;
    public float _bulletSpeed;
    public float AimRadius;


    public WeaponInventoryParameters(int shieldDamage, int bodyDamage, float sectorAngle, float reloadSec,
        float delayBetweenShootsSec,
        int shootPerTime, float _bulletSpeed, float AimRadius, float turnSpeed = 0f)
    {
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

    private float _shieldDamage = 0f;
    private float _bodyDamage = 0f;

    private float _bulletSpeed = 0.3f;
    public float _aimRadius;
    public int Level;
    public float _radiusShoot;
    public float sectorAngle;
    public float delayBetweenShootsSec;
    public int shootPerTime;
    public WeaponType WeaponType;
    public bool isRoundAng;
    public readonly float fixedDelta;
    private CurWeaponDamage _currentDamage;
    private CurWeaponDamage _currentDamage1;
    public string Name { get; set; }

    public float SetorAngle { get; set; }
    //    float IAffectParameters.BulletSpeed { get; set; }

    public float BulletSpeed
    {
        get => _bulletSpeed;
        set => _bulletSpeed = value;
    }

    public float ReloadSec { get; set; }

    public CurWeaponDamage CurrentDamage => new CurWeaponDamage(ShieldDamage, BodyDamage);


    [field: NonSerialized] public event Action<WeaponInv> OnUpgrade;

    //protected Dictionary<int, Dictionary<WeaponUpdageType, WeaponUpgradeData>> _levelUpDependences;

    private float ShieldDamage
    {
        get { return _shieldDamage + Level - 1; }
    }

    private float BodyDamage
    {
        get { return _bodyDamage + Level - 1; }
    }

    public ItemType ItemType
    {
        get { return ItemType.weapon; }
    }

    public IInventory CurrentInventory { get; set; }

    public WeaponInv(WeaponInventoryParameters parameters,
        WeaponType WeaponType, int Level)
    {
        _shieldDamage = parameters.shieldDamage;
        _bodyDamage = parameters.bodyDamage;
        this.shootPerTime = parameters.shootPerTime;
        this._bulletSpeed = parameters._bulletSpeed;
        this._bulletTurnSpeed = parameters.turnSpeed;
        this.ReloadSec = parameters.reloadSec;
        this.delayBetweenShootsSec = parameters.delayBetweenShootsSec;
        this._aimRadius = parameters.AimRadius;
//        if (WeaponType == WeaponType.beam)
//        {
//            this.RadiusShoot = AimRadius;
//        }
//        else
//        {
//            this.RadiusShoot = AimRadius * 1.6f;
//        }
        this._radiusShoot = _aimRadius * 1.6f;
        this.sectorAngle = parameters.sectorAngle;
//        this.MaxCharges = MaxCharges;
//        this.RemainCharges = RemainCharges;
        this.WeaponType = WeaponType;
        this.Level = Level;
        isRoundAng = sectorAngle >= 360;

        if (delayBetweenShootsSec * shootPerTime > ReloadSec)
        {
            Debug.LogError("wrong shoot times: delayBetweenShootsSec:" + delayBetweenShootsSec +
                           " ReloadSec:" + ReloadSec +
                           "  shootPerTime:" + shootPerTime);
        }

        Name = Namings.Weapon(WeaponType);
        if (shootPerTime > 1)
        {
            fixedDelta = MAX_DELTA * (shootPerTime / 2f);
        }
        else
        {
            fixedDelta = MAX_DELTA;
        }

        //_levelUpDependences = CreateLevelUpDependences();
    }

    public int CostValue
    {
        get { return MoneyConsts.BASE_WEAPON_MONEY_COST + MoneyConsts.LEVEL_WEAPON_MONEY_COST * (Level - 1); }
    }

    public string GetInfo()
    {
        return Name + " (" + Level + ")";
    }

    public string DamageInfo()
    {
        var dmg = CurrentDamage;
        return String.Format("Damage: Shield:{0}. Body:{1}", dmg.ShieldDamage, dmg.BodyDamage);
    }

    public string WideInfo()
    {
//        var curDesc = CurrentUpgradesDesc();
        var info = GetInfo() + "\n" + DamageInfo();
//        if (curDesc.Length > 1)
//        {
//            info = info + "\n" + curDesc;
//        }

        return info;
    }

//    CurWeaponDamage IAffectParameters.CurrentDamage => _currentDamage;

//    float IAffectParameters.AimRadius { get; set; }


    public float AimRadius
    {
        get { return _aimRadius; }
        set { _aimRadius = value; }
    }

    public void BulletCreate(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos,
        BulleStartParameters bulleStartParameters)
    {
        //        var dirToShoot = Owner.LookDirection;
        var dirToShoot = target.Position - shootPos;
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
            if (MoneyConsts.WeaponUpgrade.ContainsKey(Level))
            {
                var cost = MoneyConsts.WeaponUpgrade[Level];
                if (owner.MoneyData.HaveMoney(cost))
                {
                    var txt = String.Format( Namings.WANT_UPGRADE_WEAPON , Namings.Weapon(WeaponType));
                    WindowManager.Instance.ConfirmWindow.Init(() =>
                    {
                        owner.MoneyData.RemoveMoney(cost);
                        Upgrade();
                        //                        WindowManager.Instance.InfoWindow.Init(null, "Upgrade completed");
                    }, null, txt);
                }
                else
                {
                    WindowManager.Instance.NotEnoughtMoney(cost);
                }
            }
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null,  Namings.WeaponMaxLevel);
        }
    }

    public bool CanUpgrade()
    {
        return Level < Library.MAX_WEAPON_LVL;
    }


    public void Upgrade()
    {
        if (CanUpgrade())
        {
            Level++;
            if (OnUpgrade != null)
            {
                OnUpgrade(this);
            }
        }
    }
}