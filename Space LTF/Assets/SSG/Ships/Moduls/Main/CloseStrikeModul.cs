using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public class CloseStrikeModul : BaseModul,IWeapon
{
//    private bool isCharged;
    private TimerManager.ITimer _timer;
    private BattleController _battleController;
    private Bullet _bulletOrigin;
    private bool _striking = false;
    private const float ANG_OFFSET = 8f;
    private const float BULLET_SPEED = 5f;
    private const float BULLET_TURN_SPEED = .2f;
    private const float DIST_SHOT = 4f;

    public CloseStrikeModul(BaseModulInv baseModulInv) 
        : base(baseModulInv)
    {
        CurrentDamage = new CurWeaponDamage(1 + Level, 1 + Level);
        _bulletOrigin = DataBaseController.Instance.GetBullet(WeaponType.closeStrike);
        if (_bulletOrigin == null)
        {
            Debug.LogError("Can't fin bullet for close strike!!!");
        }
        Period = 20f;
    }

    public void BulletCreateByDir(ShipBase target, Vector3 dir)
    {

        var bsp = new BulleStartParameters(BULLET_SPEED, BULLET_TURN_SPEED, DIST_SHOT, DIST_SHOT);
        Bullet.Create(_bulletOrigin, this, dir, _owner.Position, null, bsp);
    }

    public int Level
    {
        get { return ModulData.Level; }
    }

    public ShipBase Owner
    {
        get { return _owner; }
    }

    public void ApplyToShip(ShipParameters shipParameters, ShipBase shipBase, Bullet bullet)
    {
        shipParameters.Damage(CurrentDamage.ShieldDamage, CurrentDamage.BodyDamage, DamageDoneCallback,shipBase);
    }

    public float CurOwnerSpeed
    {
        get { return 0001f; }
    }

    public CurWeaponDamage CurrentDamage { get; private set; }

    public Vector3 CurPosition
    {
        get { return _owner.Position; }
    }

    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
//        Debug.Log("Apply isReady closestShip");
        if (_timer == null || !_timer.IsActive)
        {
            _timer = MainController.Instance.BattleTimerManager.MakeTimer(1f, true);
            _timer.OnTimer += OnTimer;
        }
        _battleController = BattleController.Instance;
        base.Apply(Parameters,owner);
    }


    private void OnTimer()
    {
        bool isReady = IsReady();
//        Debug.Log("OnTimer");
        if (isReady)
        {
//            Debug.Log("OnTimer isReady");
            var closestShip = _battleController.ClosestShipToPos(_owner.Position, BattleController.OppositeIndex(_owner.TeamIndex));
            if (closestShip != null)
            {
//                Debug.Log("OnTimer isReady closestShip");
                if (_owner.Enemies[closestShip].Dist < 2)
                {
//                    Debug.Log("Close strike StrikeToShip close");
                    UpdateTime();
                    StrikeToShip(closestShip);
                }
            }
        }
    }

    public void StartStrike()
    {
        //Force self
        _owner.ExternalForce.Init(12f,0.6f,_owner.LookDirection);
        _striking = true;
//        Debug.Log("CForce selfse");
    }

    public bool WantStrikeNow(out ShipBase closestShip)
    {
        closestShip = null;
        if (_striking)
        {
            return false;
        }
        if (!IsReady())
        {
            return false;
        }
        closestShip = BattleController.Instance.ClosestShipToPos(_owner.Position, BattleController.OppositeIndex(_owner.TeamIndex));
        if (closestShip != null)
        {
            var s = _owner.Enemies[closestShip];
            if (s.Dist < 5 && s.Dist > 2)
            {
                var dot = Vector3.Dot(closestShip.LookDirection, _owner.LookDirection);
                if (dot > 0)//двжение в одну сторону
                {
                    var dd = _owner.Position - closestShip.Position;
                    var dot2 = Vector3.Dot(dd, _owner.LookDirection);
                    if (dot2 < 0)//цель передомной
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void StrikeShip(ShipBase closestShip)
    {
//        Debug.Log("Close StrikeShip HIT");
        StrikeToShip(closestShip);
        closestShip.GetHit(this,null);
        _striking = false;
    }

    private void StrikeToShip(ShipBase ship)
    {

//        Debug.Log("StrikeToShip 1  " + _bulletOrigin);
        var dir = Vector3.Dot(_owner.LookLeft, _owner.Enemies[ship].DirNorm) > 0 ? _owner.LookLeft : _owner.LookRight;
        var dir1 = Utils.RotateOnAngUp(dir, -ANG_OFFSET);
        var dir2 = Utils.RotateOnAngUp(dir, ANG_OFFSET);


//        CreateBulletWithModif(baseEndPoint1_1);
//        CreateBulletWithModif(baseEndPoint2_1);

        BulletCreateByDir(null, dir);
        BulletCreateByDir(null, dir1);
        BulletCreateByDir(null, dir2);


    }

    public override void Dispose()
    {
        if (_timer != null)
        {
            _timer.Stop();
        }
        base.Dispose();
    }

    public bool ShallEndAction()
    {
        return _striking;
    }

    public void UpdateStrike(ShipBase attackShip)
    {
//        Debug.Log("UpdateStrike");
        if (_striking)
        {
            var data = _owner.Enemies[attackShip];
            if (data.Dist < 1f)
            {
                StrikeShip(attackShip);
            }
        }
    }

    public override void Delete()
    {
        base.Delete();
    }

    public void BulletDestroyed(Vector3 position, Bullet bullet)
    {
        
    }

    public TeamIndex TeamIndex
    {
        get { return _owner.TeamIndex; }
    }


    public void DamageDoneCallback(float healthdelta, float shielddelta,ShipBase damageAppliyer)
    {

        _owner.ShipInventory.LastBattleData.AddDamage(healthdelta, shielddelta);
    }
}

