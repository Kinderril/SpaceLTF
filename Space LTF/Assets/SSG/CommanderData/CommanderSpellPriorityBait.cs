using UnityEngine;
using System.Collections;

public class CommanderSpellPriorityBait : ISpellToGame
{
    public BulleStartParameters BulleStartParameters =>new BulleStartParameters(1,1,1,1);
    public WeaponInventoryAffectTarget AffectAction => new WeaponInventoryAffectTarget(AffectACtion);

    private void AffectACtion(ShipParameters targetparameters, ShipBase target, Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {

    }

    public CreateBulletDelegate CreateBulletAction => (target, origin, weapon, pos, parameters) =>
    {
        var oppositeIndex = BattleController.OppositeIndex(weapon.TeamIndex);
        var baitCommander =  BattleController.Instance.GetCommander(oppositeIndex);
        var ship = BattleController.Instance.ClosestShipToPos(target.Position, weapon.TeamIndex);
        baitCommander.SetPriorityTarget(ship,true);
    };

    public CastActionSpell CastSpell => (target, origin, weapon, shootpos, bullestartparameters) =>
    {
        CreateBulletAction(target, origin, weapon, shootpos, bullestartparameters);
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

    public float ShowCircle => 1;
    public bool ShowLine => false;
}
