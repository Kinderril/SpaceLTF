using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public class ArtillerySpell : BaseSpellModulInv
{
    private const float DIST_SHOT = 25f;
    private const float rad = 5f;

    public ArtillerySpell(int costCount, int costTime)
        : base(SpellType.artilleryPeriod, costCount, costTime,
            MainCreateBullet, MainAffect, new BulleStartParameters(2.7f, 36f, DIST_SHOT, DIST_SHOT), false)
    {

    }

    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData(rad);
    }

    private static void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        ActionShip(target, bullet.Position, damagedone);
    }

    private static void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var battle = BattleController.Instance;
        for (int i = 0; i < 10; i++)
        {
            var timer =
                MainController.Instance.TimerManager.MakeTimer(i * 0.5f);
            timer.OnTimer += () => {
                if (battle.State == BattleState.process)
                {
                    var b = Bullet.Create(origin, weapon, Vector3.down, shootpos + Vector3.up * DIST_SHOT,
                        null, bullestartparameters);
                }
            };
        }


    }

    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.throwAroundSpell);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    protected override void CastAction(Vector3 pos)
    {
    }


    private static void ActionShip(ShipBase shipBase,Vector3 fromPos,DamageDoneDelegate damageDoneCallback)
    {
        shipBase.ShipParameters.Damage(2,2, damageDoneCallback,shipBase);
    }

}

