using System.Collections.Generic;
using UnityEngine;

public class CommanderSpellMainShipBlink : ISpellToGame
{
    public BulleStartParameters BulleStartParameters { get; private set; }
    public WeaponInventoryAffectTarget AffectAction => new WeaponInventoryAffectTarget(AffectACtion, TargetType.Ally);
    private ShipControlCenter _shipToMove;
    // private Vector3 center;
    private Vector3 _lastDir;
    // private float MaxRadiusSqrt;
    private float angResetTime;
    private float _castStart;
    private float _lastPower;
    private HashSet<ShipPersonalInfo> _hitted = new HashSet<ShipPersonalInfo>();

    public CommanderSpellMainShipBlink(float rad, ShipBase shipToMove)
    {
        _shipToMove = shipToMove as ShipControlCenter;
        BulleStartParameters = new BulleStartParameters(1, 1, rad, rad);
        var battle = BattleController.Instance;
        // center = (battle.CellController.Max + battle.CellController.Min) / 2f;
        var maxRadius = battle.CellController.Data.InsideRadius;
        // MaxRadiusSqrt = maxRadius * maxRadius;
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
        _shipToMove.RamBoostEffect?.Play();
        _hitted.Clear();
        _lastPower = 0f;
        _castStart = angResetTime = Time.time;
    };

    private SpellDamageData _localSpellDamageData = new SpellDamageData();

    public SpellDamageData RadiusAOE => _localSpellDamageData;

    public Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.nextFrame);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    public float PowerInc()
    {
        return 1f;
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

    public UpdateCastDelegate UpdateCast => PeriodCast;
    public EndCastDelegateSpell EndCastPeriod => EndCast;

    private void EndCast()
    {
        _shipToMove.RamBoostEffect?.Stop();
    }

    private void PeriodCast(Vector3 trgpos, BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, CastSpellData castdata)
    {
        if (_shipToMove == null)
        {
            return;
        }

        var delta = Time.time - angResetTime;

        var pp =25 * Mathf.Pow(delta * .6f, 1.5f);
        var pos = _shipToMove.Position;
        var dir = trgpos - pos;
        var sDits = dir.sqrMagnitude;
        if (sDits > 1)
        {
            ShipBoostRam.CheckEnemies(_shipToMove,_hitted,4f);
            var coef = (40 - pp* 0.9f) * 0.04f;
            // Debug.LogError($"coefd:{coef}  pp:{pp}");
            if (coef > 0)
            {
                _lastDir = dir;
                _shipToMove.ApplyRotation(dir * coef, true);
                var ang = Utils.IsAngLessNormazied(Utils.NormalizeFastSelf(dir), _shipToMove.LookDirection,
                    UtilsCos.COS_45_RAD);
                if (!ang)
                {
                    angResetTime = Time.time;
                }
                var power = ang ? pp : 2f;
                _lastPower = power;
            }

            // Debug.LogError($"is ang:{ang}  power:{power}");   
            _shipToMove.ExternalForce.Init(_lastPower, 1, _lastDir);

        }


    }

}
