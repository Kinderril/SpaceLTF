using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class MineAbstractModul : BaseModul , IWeapon
{
    private const float DELAY_BASE = 20f;
    private const float DELAY_DELTA = 1.5f;
    private BattleController _battleController;
    private TimerManager.ITimer _timer;
    private Bullet _mineBulletPrefab;
    private float _timeCome;

    public MineAbstractModul(BaseModulInv baseModulInv) 
        : base(baseModulInv)
    {
        CurrentDamage = new CurWeaponDamage(1,1);
        _battleController = BattleController.Instance;
        Period = DELAY_BASE - ModulData.Level * DELAY_DELTA;
        _mineBulletPrefab = GetPrefab();
#if UNITY_EDITOR
        var collider = _mineBulletPrefab.GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("mine prefab have no collider");
        }
#endif
    }

    public float CurOwnerSpeed
    {
        get { return 0f; }
    }

    public CurWeaponDamage CurrentDamage { get; protected set; }

    public Vector3 CurPosition
    {
        get { return _owner.Position; }
    }

    public void BulletCreate(ShipBase target,Vector3 dir)
    {
        Debug.Log($"Mine abstract BulletCreate {Time.time}");
        Bullet.Create(_mineBulletPrefab, this, -_owner.LookDirection, _owner.Position, null,
            new BulleStartParameters(0.01f, 0f, 1f, 1f));
    }

    public int Level
    {
        get { return ModulData.Level; }
    }

    protected abstract Bullet GetPrefab();
    
    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
        if (_timer == null || !_timer.IsActive)
        {
            _timer = MainController.Instance.TimerManager.MakeTimer(1f, true);
            _timer.OnTimer += OnTimer;
        }
        //        _owner.WeaponsController.OnWeaponShootStart += OnWeaponShootStart;
        base.Apply(Parameters,owner);
    }
    

    public ShipBase Owner
    {
        get { return _owner; }
    }

    public abstract void ApplyToShip(ShipParameters shipParameters, ShipBase shipBase, Bullet bullet);

    private void OnTimer()
    {
        bool isReady = IsReady();
        if (isReady)
        {
            var closestShip = _battleController.ClosestShipToPos(_owner.Position, BattleController.OppositeIndex(_owner.TeamIndex));
            if (closestShip != null)
            {
                var s = _owner.Enemies[closestShip];
                if (s.Dist < 10 && s.Dist > 3)
                {
                    var dot = Vector3.Dot(closestShip.LookDirection, _owner.LookDirection);
                    if (dot > 0)//двжение в одну сторону
                    {
                        var dd = _owner.Position - closestShip.Position;
                        var dot2 = Vector3.Dot(dd, _owner.LookDirection);
                        if (dot2 > 0)//цель за расстановщиком
                        {
                            SetMine();
                        }
                    }
                }
            }
        }
    }

    public void SetMine()
    {
        UpdateTime();
        BulletCreate(null, _owner.LookDirection);
    }

    public override void UpdateBattle()
    {
        if (_timeCome < Time.time)
        {
            _timeCome = Time.time + 3f;
            bool isReady = IsReady();
            if (isReady)
            {
                SetMine();
            }
        }
    }

    public override void Dispose()
    {
        if (_timer != null)
        {
            _timer.Stop();
        }
        //        _owner.WeaponsController.OnWeaponShootStart -= OnWeaponShootStart;
        base.Dispose();
    }

    public override void Delete()
    {
//        _owner.WeaponsController.OnWeaponShootStart -= OnWeaponShootStart;
        base.Delete();
    }

    public abstract void BulletDestroyed(Vector3 position, Bullet bullet);

    public TeamIndex TeamIndex
    {
        get { return _owner.TeamIndex; }
    }

    public void DamageDoneCallback(float healthdelta, float shielddelta, ShipBase damageAppliyer)
    {
        _owner.ShipInventory.LastBattleData.AddDamage(healthdelta,shielddelta);
    }
}

