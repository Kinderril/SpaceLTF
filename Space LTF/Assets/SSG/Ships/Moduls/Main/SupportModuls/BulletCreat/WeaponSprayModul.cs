using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponSprayModul : BaseSupportModul
{
    private const float ANG_1 = 8f;
    private const float ANG_2 = 12f;
    private const float RELOAD = 2.5f;


    protected override bool BulletImplement => true;

    protected override CreateBulletDelegate BulletCreate(CreateBulletDelegate standartDelegate)
    {
        if (Level == 1)
        {

            return BulletCreateSpray2;
        }
        else
        {

            return BulletCreateSpray3;
        }
    }

    private void BulletCreateSpray3(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters startParameters)
    {
    
        var dirToShoot = target.Position - shootPos;
        var isHoming = origin is HomingBullet;
        if (isHoming)
        {
            var b0 = Bullet.Create(origin, weapon, dirToShoot, shootPos, target.target, startParameters);
            for (int i = 0; i < 2; i++)
            {
                var timer = MainController.Instance.BattleTimerManager.MakeTimer(0.4f);
                timer.OnTimer += () =>
                {
                    var b1 = Bullet.Create(origin, weapon, dirToShoot, shootPos, target.target, startParameters);
                };
            }

        }
        else
        {
            var b0 = Bullet.Create(origin, weapon, dirToShoot, shootPos, target.target, startParameters);
            var half = ANG_2 / 2f;

            var r1 = Utils.RotateOnAngUp(dirToShoot, -half);
            var r2 = Utils.RotateOnAngUp(dirToShoot, half);

            var b1 = Bullet.Create(origin, weapon, r1, shootPos, target.target, startParameters);
            var b2 = Bullet.Create(origin, weapon, r2, shootPos, target.target, startParameters);
        }
    }

    private void BulletCreateSpray2(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters startParameters)
    {
         var dirToShoot = target.Position - shootPos;

         var isHoming = origin is HomingBullet;
         if (isHoming)
         {
             var b0 = Bullet.Create(origin, weapon, dirToShoot, shootPos, target.target, startParameters);
            var timer = MainController.Instance.BattleTimerManager.MakeTimer(0.4f);
            timer.OnTimer += () =>
            {
                var b1 = Bullet.Create(origin, weapon, dirToShoot, shootPos, target.target, startParameters);
            };

        }
         else
         {
             var half = ANG_1 / 2f;

             var r1 = Utils.RotateOnAngUp(dirToShoot, -half);
             var r2 = Utils.RotateOnAngUp(dirToShoot, half);


             var b1 = Bullet.Create(origin, weapon, r1, shootPos, target.target, startParameters);
             var b2 = Bullet.Create(origin, weapon, r2, shootPos, target.target, startParameters);
         }
    }
    public override string DescSupport()
    {
        return $"Shoot with 3 bullets instead on one. Increase reload time by {Utils.FloatToChance(RELOAD)}%.";
    }
    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.ReloadSec *= RELOAD;
    }

    public WeaponSprayModul(int level)
        : base(SimpleModulType.WeaponSpray, level)
    {
    }
}
