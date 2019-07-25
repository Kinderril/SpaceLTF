using UnityEngine;
using System.Collections;

public class CommanderSpellPriorityTarget : ISpellToGame
{
    public BulleStartParameters BulleStartParameters =>new BulleStartParameters(1,1,1,1);
    public WeaponInventoryAffectTarget AffectAction => new WeaponInventoryAffectTarget(AffectACtion);

    private void AffectACtion(ShipParameters targetparameters, ShipBase target, Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
//                  Debug.LogError("AFFECT PRIORITY!!");
        bullet.Weapon.Owner.Commander.SetPriorityTarget(target);
    }

    public CreateBulletDelegate CreateBulletAction => (target, origin, weapon, pos, parameters) =>
    {
        var b0 = Bullet.Create(origin, weapon, Vector3.up, target.Position, target.target, BulleStartParameters);
    };

    public SpellDamageData RadiusAOE()
    {
        return new SpellDamageData();
    }

    public Bullet GetBulletPrefab()
    {      
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.nextFrame);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }
}
