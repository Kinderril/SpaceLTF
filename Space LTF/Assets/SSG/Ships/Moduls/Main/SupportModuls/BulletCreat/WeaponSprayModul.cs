using System;
using UnityEngine;

[Serializable]
public class WeaponSprayModul : BaseSupportModul
{
    private const float ANG_1 = 8f;
    private const float ANG_2 = 12f;
    private const float RELOAD = 3.2f;


    protected override bool BulletImplement => true;

    protected override CreateBulletDelegate BulletCreate(CreateBulletDelegate standartDelegate)
    {
        if (Level == 1)
        {
            return (target, origin, weapon, pos, parameters) => BulletCreateSpray2(target, origin, weapon, pos, parameters, standartDelegate);
        }
        else if (Level == 2)
        {
            return (target, origin, weapon, pos, parameters) => BulletCreateSpray3(target, origin, weapon, pos, parameters, standartDelegate);

        }

        return (target, origin, weapon, pos, parameters) => BulletCreateSpray4(target, origin, weapon, pos, parameters, standartDelegate);
    }

    private int BulletsCount()
    {
        if (Level == 1)
        {
            return 2;
        }
        else if (Level == 2)
        {
            return 3;

        }

        return 4;
    }

    private void BulletCreateSpray4(BulletTarget target, Bullet origin,
        IWeapon weapon, Vector3 shootPos, BulleStartParameters startParameters, CreateBulletDelegate standartDelegate)
    {

        var dirToShoot = target.Position - shootPos;
        var isHoming = origin is HomingBullet;
        if (isHoming)
        {
            standartDelegate(target, origin, weapon, shootPos, startParameters);
            for (int i = 0; i < 4; i++)
            {
                var timer = MainController.Instance.BattleTimerManager.MakeTimer(0.4f);
                timer.OnTimer += () =>
                {
                    standartDelegate(target, origin, weapon, shootPos, startParameters);
                };
            }

        }
        else
        {
            var full = ANG_2;
            var half = ANG_2 / 2f;

            var r1 = Utils.RotateOnAngUp(dirToShoot, -half);
            var r2 = Utils.RotateOnAngUp(dirToShoot, half);
            var r3 = Utils.RotateOnAngUp(dirToShoot, -full);
            var r4 = Utils.RotateOnAngUp(dirToShoot, full);

            standartDelegate(new BulletTarget(shootPos + r1), origin, weapon, shootPos, startParameters);
            standartDelegate(new BulletTarget(shootPos + r2), origin, weapon, shootPos, startParameters);
            standartDelegate(new BulletTarget(shootPos + r3), origin, weapon, shootPos, startParameters);
            standartDelegate(new BulletTarget(shootPos + r4), origin, weapon, shootPos, startParameters);
        }
    }
    private void BulletCreateSpray3(BulletTarget target, Bullet origin,
        IWeapon weapon, Vector3 shootPos, BulleStartParameters startParameters, CreateBulletDelegate standartDelegate)
    {

        var dirToShoot = target.Position - shootPos;
        var isHoming = origin is HomingBullet;
        if (isHoming)
        {
            standartDelegate(target, origin, weapon, shootPos, startParameters);
            for (int i = 0; i < 2; i++)
            {
                var timer = MainController.Instance.BattleTimerManager.MakeTimer(0.4f);
                timer.OnTimer += () =>
                {
                    standartDelegate(target, origin, weapon, shootPos, startParameters);
                };
            }

        }
        else
        {
            standartDelegate(new BulletTarget(shootPos + dirToShoot), origin, weapon, shootPos, startParameters);

            var half = ANG_2 / 2f;
            var r1 = Utils.RotateOnAngUp(dirToShoot, -half);
            var r2 = Utils.RotateOnAngUp(dirToShoot, half);

            standartDelegate(new BulletTarget(shootPos + r1), origin, weapon, shootPos, startParameters);
            standartDelegate(new BulletTarget(shootPos + r2), origin, weapon, shootPos, startParameters);
        }
    }

    private void BulletCreateSpray2(BulletTarget target, Bullet origin,
        IWeapon weapon, Vector3 shootPos, BulleStartParameters startParameters, CreateBulletDelegate standartDelegate)
    {
        var dirToShoot = target.Position - shootPos;

        var isHoming = origin is HomingBullet;
        if (isHoming)
        {
            standartDelegate(target, origin, weapon, shootPos, startParameters);
            var timer = MainController.Instance.BattleTimerManager.MakeTimer(0.4f);
            timer.OnTimer += () =>
            {
                standartDelegate(target, origin, weapon, shootPos, startParameters);
            };

        }
        else
        {
            var half = ANG_1 / 2f;

            var r1 = Utils.RotateOnAngUp(dirToShoot, -half);
            var r2 = Utils.RotateOnAngUp(dirToShoot, half);

            standartDelegate(new BulletTarget(shootPos + r1), origin, weapon, shootPos, startParameters);
            standartDelegate(new BulletTarget(shootPos + r2), origin, weapon, shootPos, startParameters);
        }
    }
    public override string DescSupport()
    {
        return Namings.Format(Namings.Tag("SprayModulDesc"), BulletsCount(), Utils.FloatToChance(RELOAD));
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
