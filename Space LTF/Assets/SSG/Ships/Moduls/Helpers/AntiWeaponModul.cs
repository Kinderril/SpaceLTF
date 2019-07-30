using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class AntiWeaponModul : TimerModul
{
    private const float RADIUS_BASE = 14;
    private float _radSqrt = 400;

//    private float _nextCheckTime;
    private BattleController _battleController;
    protected BulletDamageType _damageType;

    public AntiWeaponModul(BaseModulInv baseModulInv) 
        : base(baseModulInv)
    {
        var r = ModulData.Level + RADIUS_BASE;
        _radSqrt = r*r;
    }

    protected abstract BulletKiller GetEffect();
    protected abstract float destroyTime { get; }

    private void KillBullet(Bullet closestBullet)
    {
        var bulletKiller = DataBaseController.GetItem(GetEffect());
        bulletKiller.Init(_owner,closestBullet, destroyTime);
    }

    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
        _battleController = BattleController.Instance;
        base.Apply(Parameters,owner);
    }
    

    protected override void TimerAction()
    {
        var closestBullet = _battleController.ClosestBulletToPos(_owner.Position, _damageType,
            BattleController.OppositeIndex(_owner.TeamIndex));

        if (closestBullet != null && Time.time - closestBullet.StartTime > 0.05f)
        {
            var d = closestBullet.transform.position - _owner.Position;
            if (d.sqrMagnitude < _radSqrt)
            {
                UpdateTime();
                KillBullet(closestBullet);
            }
        }
    }
}

