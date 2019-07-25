using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponNoDeathModul : BaseSupportModul
{
    private const float RELOAD = 1.8f;


    protected override bool BulletImplement => true;

    protected override CreateBulletDelegate BulletCreate(CreateBulletDelegate standartDelegate)
    {
        return BulletWithNoDeath;
    }
    private void BulletWithNoDeath(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters startParameters)
    {
        var dirToShoot = target.Position - shootPos;
        var b0 = Bullet.Create(origin, weapon, dirToShoot, shootPos, target.target, startParameters);
        b0.DeathOnHit = false;
    }


    public override string DescSupport()
    {
        return $"Bullet don't destroy when hit target. Increase reload time by {Utils.FloatToChance(RELOAD)}%.";
    }
    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.ReloadSec *= RELOAD;
    }

    public WeaponNoDeathModul(int level)
        : base(SimpleModulType.WeaponNoBulletDeath, level)
    {
    }
}
