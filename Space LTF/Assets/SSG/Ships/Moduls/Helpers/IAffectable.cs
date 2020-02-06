using UnityEngine;

public class BulletTarget
{
    public ShipBase target;
    public Vector3 Position;
    public bool IsDir { get; private set; }

    public BulletTarget(ShipBase target)
    {
        this.target = target;
        Position = target.Position;
    }
    public BulletTarget(Vector3 pos)
    {
        this.Position = pos;
    }
}

[System.Serializable]
public delegate void AffectTargetDelegate(ShipParameters targetParameters, ShipBase target, Bullet bullet,
    DamageDoneDelegate damageDone, WeaponAffectionAdditionalParams additional);

[System.Serializable]
public delegate void CreateBulletDelegate(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos,
    BulleStartParameters bulleStartParameters);

[System.Serializable]
public delegate void BulletDestroyDelegate(Bullet origin, IWeapon weapon, AICell cell);

public interface IAffectable
{

    WeaponInventoryAffectTarget AffectAction { get; }
    CreateBulletDelegate CreateBulletAction { get; }
    void SetBulletCreateAction(CreateBulletDelegate bulletCreate);
}

public interface IAffectParameters
{

    CurWeaponDamage CurrentDamage { get; }
    float AimRadius { get; set; }
    float SetorAngle { get; set; }
    float BulletSpeed { get; set; }
    float ReloadSec { get; set; }
    int ShootPerTime { get; set; }
}
