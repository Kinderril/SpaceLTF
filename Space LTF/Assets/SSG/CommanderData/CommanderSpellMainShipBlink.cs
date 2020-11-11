using UnityEngine;

public class CommanderSpellMainShipBlink : ISpellToGame
{
    public BulleStartParameters BulleStartParameters { get; private set; }
    public WeaponInventoryAffectTarget AffectAction => new WeaponInventoryAffectTarget(AffectACtion, TargetType.Ally);
    private ShipBase _shipToMove;
    private Vector3 center;
    private float MaxRadiusSqrt;

    public CommanderSpellMainShipBlink(float rad, ShipBase shipToMove)
    {
        _shipToMove = shipToMove;
        BulleStartParameters = new BulleStartParameters(1, 1, rad, rad);
        var battle = BattleController.Instance;
        center = (battle.CellController.Max + battle.CellController.Min) / 2f;
        var maxRadius = battle.CellController.Data.InsideRadius;
        MaxRadiusSqrt = maxRadius * maxRadius;
    }

    private void AffectACtion(ShipParameters targetparameters, ShipBase target, Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
    }

    public CreateBulletDelegate CreateBulletAction => (target, origin, weapon, pos, parameters) =>
    {
        //ADD ACTION HERE
    };

    public ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        return true;

    }
    public BulletDestroyDelegate BulletDestroyDelegate { get; }

    public CastActionSpell CastSpell => (target, origin, weapon, shootpos, castDat) =>
    {

        var pos = target.Position;
        var distFromCenter = (pos - center).sqrMagnitude;
        if (distFromCenter < MaxRadiusSqrt)
        {
            CreateBulletAction(target, origin, weapon, shootpos, castDat.Bullestartparameters);
            EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.BlinkPlaceEffect, pos, 3f);
            EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.BlinkTargetEffect, _shipToMove.Position, 3f);
            // _shipToMove.Rotation = Quaternion.FromToRotation(Vector3.forward, dir);
            _shipToMove.Position = pos;
        }

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
    public bool ShowLine => true;
    public void ResetBulletCreateAtion()
    {
        

    }

    public SubUpdateShowCast SubUpdateShowCast { get; }

    public CanCastAtPoint CanCastAtPoint
    {
        get { return pos => true; }
    }

    public void SetBulletCreateAction(CreateBulletDelegate bulletCreate)
    {

    }

    public void DisposeAfterBattle()
    {
        
    }
}
