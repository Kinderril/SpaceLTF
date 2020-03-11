using System;

public class ShipDesicionDataAttack : ShipDesicionDataBase
{

    private ShipBase _shipToDefence;
    private bool _haveBaseToDefence;
    public ShipDesicionDataAttack(ShipBase owner, PilotTactic tactic)
        : base(owner, tactic)
    {
        _shipToDefence = owner.Commander.MainShip;
        _haveBaseToDefence = _shipToDefence != null;
        if (_shipToDefence != null)
        {
            _shipToDefence.OnDispose += OnDispose;
        }
    }

    private void OnDispose(ShipBase obj)
    {
        if (_shipToDefence != null)
        {
            _shipToDefence.OnDispose -= OnDispose;
        }
        _haveBaseToDefence = false;
    }

    public override bool HaveEnemyInDangerZoneDefenceBase(out ShipBase ship)
    {
        if (!_haveBaseToDefence)
        {
            ship = null;
            return false;
        }
        if (HaveEnemyClose(out ship, 4))
        {
            return true;
        }
        if (HaveEnemyClose(out ship, _shipToDefence, _defDist))
        {
            return true;
        }
        ship = null;
        return false;
    }
    public override bool HaveClosestDamagedFriend(out ShipBase ship)
    {
        float dist = Single.MaxValue;
        ship = null;
        bool haveVal = false;
        // _owner.WeaponsController.AnyDamagedWeaponIsLoaded()
        foreach (var shipTest in _owner.Commander.Ships)
        {
            var s = (shipTest.Value);
            var hpPercent = s.ShipParameters.CurHealth / s.ShipParameters.MaxHealth;
            var spPercent = s.ShipParameters.CurShiled / s.ShipParameters.MaxShield;
            bool hpWantHeal = hpPercent < .6f;
            bool spWantHeal = spPercent < .6f && s.ShipParameters.MaxShield > 0f;
            if ((hpWantHeal && _owner.WeaponsController.SupportWeaponsBuffPosibilities.BodyHeal)
                || (spWantHeal && _owner.WeaponsController.SupportWeaponsBuffPosibilities.ShieldHeal) ||
                _owner.WeaponsController.SupportWeaponsBuffPosibilities.Buff)
            {
                if (SideAttack == ESideAttack.BaseDefence && _haveBaseToDefence)
                {
                    float maxDistDefBase = 15 * 15;
                    var sDistTmp = (s.Position - _shipToDefence.Position).sqrMagnitude;
                    if (sDistTmp < dist && sDistTmp < maxDistDefBase)
                    {
                        dist = sDistTmp;
                        ship = s;
                        haveVal = true;
                    }
                }
                else
                {
                    var sDistTmp = (s.Position - _owner.Position).sqrMagnitude;
                    if (sDistTmp < dist)
                    {
                        dist = sDistTmp;
                        ship = s;
                        haveVal = true;
                    }
                }

            }
        }
        return haveVal;
    }

    protected override ActionType? DoAttackAction(ShipBase ship)
    {
        var attackOptional = AttackOrAttackSide(ship);
        var attack = DoOrWait(attackOptional, ship);

        if (attack.HasValue)
        {
            return attack.Value;
        }

        return null;
    }

    protected override ActionType OptionalTask(out ShipBase ship)
    {
        ship = null;
        return ActionType.afterAttack;
    }

    protected override bool HaveEnemyInDangerZone(out ShipBase ship)
    {
        if (_owner.Enemies.Count > 0)
        {
            var enemy = CalcBestEnemy(out var enemyRating, _owner.Enemies);
            ship = enemy.ShipLink;
            return true;
        }
        ship = null;
        return false;
    }

    public override void Dispose()
    {
        if (_haveBaseToDefence && _shipToDefence != null)
        {
            _shipToDefence.OnDispose -= OnDispose;
        }
        base.Dispose();
    }

    public override string GetName()
    {
        return "Attack";
    }

}

