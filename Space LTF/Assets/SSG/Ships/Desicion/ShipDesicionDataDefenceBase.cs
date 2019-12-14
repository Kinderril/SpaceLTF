using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class ShipDesicionDataDefenceBase : ShipDesicionDataBase
{
    private ShipBase _shipToDefence;

    private SpellZoneVisualCircle _radiusEffect;

    public ShipDesicionDataDefenceBase(ShipBase owner,ShipBase shipToDefence,PilotTactic tactic)
        :base(owner, tactic)
    {
        _shipToDefence = shipToDefence;
        var re = DataBaseController.Instance.SpellDataBase.RadiusAttackEffect;
        _radiusEffect = DataBaseController.GetItem(re);
        _radiusEffect.SetSize(_defDist);
    }


    protected override ActionType DoAttackAction(ShipBase ship)
    {
        return DoOrWait(ActionType.attack, ship);

    }

    protected override ActionType OptionalTask(out ShipBase ship)
    {
        var mineModul = GetMineModul();
        if (mineModul != null && mineModul.IsReady())
        {
            ship = null;
            return ActionType.mineField;
        }
        ship = _shipToDefence;
        return ActionType.defence;
    }

    public override string GetName()
    {
        return "Base defence";
    }

    protected override bool HaveEnemyInDangerZone(out ShipBase ship)
    {
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
        if (_radiusEffect != null)
        GameObject.Destroy(_radiusEffect.gameObject);
    }
}

