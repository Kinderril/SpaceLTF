using System;
using UnityEngine;


[System.Serializable]
public class MachineGunSpell : BaseSpellModulInv
{
    private const float DIST_SHOT = 25f;
    // private const float baseDamage = 4;
    private const float rad = 1f;


    private float DmgHull => 3 + Level * 2;
    private float DmgShield => 1 + (int)(Level * 1.5f);
    public int BulletsCount => 4;

    public MachineGunSpell()
        : base(SpellType.machineGun, 1, 45, new BulleStartParameters(14f, 36f, DIST_SHOT, DIST_SHOT), false)
    {
    }

    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData();
    }

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var battle = BattleController.Instance;
        var offset = rad / 2;
        for (int i = 0; i < BulletsCount; i++)
        {
            var timer =
                MainController.Instance.BattleTimerManager.MakeTimer(i * 0.15f);
            timer.OnTimer += () =>
            {
                if (battle.State == BattleState.process)
                {
                    var xx = MyExtensions.Random(-offset, offset);
                    var zz = MyExtensions.Random(-offset, offset);

                    var nTargte = new BulletTarget(target.Position + new Vector3(xx, 0, zz));
                    MainCreateBullet(nTargte, origin, weapon, weapon.CurPosition, bullestartparameters);
                }
            };
        }
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        ActionShip(target, DmgHull, DmgShield, damagedone);
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {

        var startPos = weapon.CurPosition;
        var dir = Utils.NormalizeFastSelf(target.Position - startPos);
        Bullet.Create(origin, weapon, dir, startPos,
            null, bullestartparameters);
    }

    protected override CreateBulletDelegate createBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;

    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.machineGun);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    public override bool ShowLine => true;
    public override float ShowCircle => rad;
    //    public override bool S => false;

    protected override void CastAction(Vector3 pos)
    {
    }


    private static void ActionShip(ShipBase shipBase, float bodyDamage, float shieldDamage, DamageDoneDelegate damageDoneCallback)
    {
        shipBase.ShipParameters.Damage(shieldDamage, bodyDamage, damageDoneCallback, shipBase);
    }
    public override string Desc()
    {
        return String.Format(Namings.Tag("MachineGunSpellDesc"), BulletsCount, DmgShield, DmgHull);
    }
}

