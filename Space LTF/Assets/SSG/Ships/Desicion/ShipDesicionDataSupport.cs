using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class ShipDesicionDataSupport : ShipDesicionDataBase
{
    public const float distToAttack = 10f;
    private ShipBase _supportedShip;
    private SpellZoneVisualCircle _radiusEffect;

    public ShipDesicionDataSupport(ShipBase owner, PilotTactic tactic)
        : base(owner, tactic)
    {

        var re = DataBaseController.Instance.SpellDataBase.RadiusAttackEffect;
        _radiusEffect = DataBaseController.GetItem(re);
        _radiusEffect.SetSize(distToAttack);
    }

    public override string GetName()
    {
        return "Support";
    }

    protected override ActionType DoAttackAction(ShipBase ship)
    {
        return DoOrWait(ActionType.attack, ship);
    }

    protected override ActionType OptionalTask(out ShipBase ship)
    {
        ship = _supportedShip;
        return ActionType.defence;
    }

    protected override bool HaveEnemyInDangerZone(out ShipBase ship)
    {
        if (HaveEnemyClose(out ship, distToAttack - 2))
        {
            return true;
        }
        if (HaveEnemyClose(out ship, _supportedShip, distToAttack))
        {
            return true;
        }
        ship = null;
        return false;
    }

    public override void Select(bool val)
    {
        base.Select(val);
        _radiusEffect.gameObject.SetActive(val);
    }

    public override void DrawUpdate()
    {
        _radiusEffect.transform.position = _owner.Position;
    }

    public override void Dispose()
    {
        GameObject.Destroy(_radiusEffect.gameObject);
    }

}

