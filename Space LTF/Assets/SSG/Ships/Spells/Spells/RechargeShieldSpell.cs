using System;
using UnityEngine;


[System.Serializable]
public class RechargeShieldSpell : BaseSpellModulInv
{
    public const float MINES_DIST = 7f;
    private const float rad = 1f;

    private const float _sDistToShoot = 4 * 4;
    private bool _lastCheckIsOk = false;
    [field: NonSerialized]
    private ShipBase _lastClosest;

    private float HealPercent => Library.CHARGE_SHIP_SHIELD_HEAL_PERCENT + Level * 0.08f;
    public RechargeShieldSpell()
        : base(SpellType.rechargeShield, 2, 10,
             new BulleStartParameters(15f, 46f, MINES_DIST, MINES_DIST), false)
    {

    }
    protected override CreateBulletDelegate createBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        if (_lastClosest != null)
        {
            MainAffect(_lastClosest.ShipParameters, _lastClosest, null, null, null);
        }
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon,
        Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        //        var dir = (target.Position - weapon.CurPosition);
        //        Commander commnder;
        //        if (weapon.TeamIndex == TeamIndex.green)
        //        {
        //            commnder = BattleController.Instance.GreenCommander;
        //        }
        //        else
        //        {
        //            commnder = BattleController.Instance.RedCommander;
        //        }
        //
        //        ShipBase closestShip = commnder.GetClosestShip(target.Position,false);
        //
        //
        //        Bullet.Create(origin, weapon, dir, weapon.CurPosition, closestShip,  BulleStartParameters);
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        var ship = target;
        var maxShield = ship.ShipParameters.ShieldParameters.MaxShield;
        var countToHeal = maxShield * HealPercent;
        ship.Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.HealSheild);
        ship.ShipParameters.ShieldParameters.HealShield(countToHeal);
    }
    public override bool ShowLine => false;
    public override float ShowCircle => rad;
    public override Bullet GetBulletPrefab()
    {
        return null;
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
        var closestsShip = BattleController.Instance.ClosestShipToPos(pos, teamIndex, out float sDist);
        if (sDist < _sDistToShoot && closestsShip != null)
        {
            _lastCheckIsOk = true;
            objectToShow.gameObject.SetActive(true);
            objectToShow.transform.position = closestsShip.Position;
            _lastClosest = closestsShip;
            //            Debug.LogError($"Set last close {_lastClosest}");
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
        return new SpellDamageData(rad);
    }
    public override string Desc()
    {
        return String.Format(Namings.RechargeSheildSpell, Utils.FloatToChance(HealPercent));
    }
}

