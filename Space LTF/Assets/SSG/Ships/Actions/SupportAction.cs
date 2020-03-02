using JetBrains.Annotations;
using UnityEngine;



public class SupportAction : AttackAction
{


    public SupportAction([NotNull] ShipBase owner, [NotNull] IShipData target,
        ActionType actionType = ActionType.support)
        : base(owner, target, actionType)
    {
        _isShootEnd = false;
    }


    protected override CauseAction[] GetEndCauses()
    {
        var c = new[]
        {
            new CauseAction("out bf", () => !_owner.InBattlefield),
            new CauseAction("is Shoot End", () => _isShootEnd),
            new CauseAction("invisible", () => !Target.Visible),
            new CauseAction("target null", () => Target == null),
            new CauseAction("another target close", AnotherTargetBetter),
            new CauseAction("weapon not load", () => _owner.WeaponsController.AllSupportWeaponNotLoad(0f)),
            new CauseAction("target is dead", () => Target.ShipLink.IsDead)
        };
        return c;
    }
    protected override void Dispose()
    {
        foreach (var weapon in _owner.WeaponsController.GelAllWeapons())
            weapon.OnShootEnd -= OnShootEnd;
    }
    protected override bool AnotherTargetBetter()
    {

        if (_nextRecalTime < Time.time)
        {
            _nextRecalTime = Time.time + MyExtensions.Random(0.8f, 1.2f);

            if (Target.Dist > 20)
            {
                var besstEnemy = _shipDesicionDataBase.CalcBestEnemy(_owner.Enemies);
                if (besstEnemy != Target)
                    return true;
            }
        }
        return false;
    }
}