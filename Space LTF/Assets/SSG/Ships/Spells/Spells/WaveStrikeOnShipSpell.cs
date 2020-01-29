using System;
using UnityEngine;


[System.Serializable]
public class WaveStrikeOnShipSpell : BaseSpellModulInv
{
    public const float BULLET_SHOOT_DIST = 14f;
    private const float _sDistToShoot = 4 * 4;
    private const float BASE_DAMAGE = 5;
    private bool _lastCheckIsOk = false;

    [field: NonSerialized]
    private ShipBase _lastClosest;

    public float Damage => BASE_DAMAGE + Level * 2;

    public WaveStrikeOnShipSpell()
        : base(SpellType.roundWave, 2, 10,
             new BulleStartParameters(15f, 46f, BULLET_SHOOT_DIST, BULLET_SHOOT_DIST), false)
    {

    }
    protected override CreateBulletDelegate createBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        //        var closestsShip = BattleController.Instance.ClosestShipToPos(shootPos, weapon.TeamIndex, out float sDist);
        if (_lastClosest != null)
        {
            var indToSearch = BattleController.OppositeIndex(weapon.TeamIndex);
            var nearShips = BattleController.Instance.GetAllShipsInRadius(_lastClosest.Position, indToSearch, BULLET_SHOOT_DIST);
            //            Debug.LogError($"WaveStrikeOnShipSpell ships in raius {nearShips.Count} ,  {BULLET_SHOOT_DIST}  indToSearch:{indToSearch.ToString()}");
            foreach (var nearShip in nearShips)
            {
                MainCreateBullet(new BulletTarget(nearShip), origin, weapon, _lastClosest.Position, bullestartparameters);
            }
        }
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon,
        Vector3 shootpos, BulleStartParameters bullestartparameters)
    {

        if (target.target != null)
        {
            Bullet.Create(origin, weapon, target.Position - shootpos, shootpos, target.target, bullestartparameters);
        }
        else
        {
            Debug.LogError($"WaveStrikeOnShipSpell error. Try to start beam bullet without target");
        }
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        target.ShipParameters.Damage(Damage, Damage, damagedone, _lastClosest);
    }

    public override bool ShowLine => false;
    public override float ShowCircle => BULLET_SHOOT_DIST;
    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.beamWaveStrike);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    protected override void CastAction(Vector3 pos)
    {
    }

    public override SubUpdateShowCast SubUpdateShowCast => ShowOnShip;

    public override CanCastAtPoint CanCastAtPoint
    {
        get { return pos => _lastCheckIsOk; }
    }

    protected void ShowOnShip(Vector3 pos, TeamIndex teamIndex, GameObject objectToShow)
    {
        var lastClosest = BattleController.Instance.ClosestShipToPos(pos, teamIndex, out float sDist);
        if (sDist < _sDistToShoot && lastClosest != null)
        {
            _lastCheckIsOk = true;
            objectToShow.gameObject.SetActive(true);
            objectToShow.transform.position = lastClosest.Position;
            _lastClosest = lastClosest;
        }
        else
        {
            _lastClosest = null;
            _lastCheckIsOk = false;
            objectToShow.gameObject.SetActive(false);
        }
    }



    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData();
    }
    public override string Desc()
    {
        return String.Format(Namings.WaveStrikeSpell, Damage, Damage);
    }
}

