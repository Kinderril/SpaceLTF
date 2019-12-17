using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


[System.Serializable]
public class BlinkModul : TimerModul
{

    public BlinkModul(BaseModulInv baseModulInv) 
        : base(baseModulInv)
    {
        Period = 15 - ModulData.Level * 3;
    }


    protected override float Delay()
    {
        return Period;
    }

    protected override void TimerAction()
    {
        ActionCheck(_owner.CurAction);
    }

    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
        base.Apply(Parameters,owner);
        _owner.OnActionChange += ActionChanged;
    }
    
    private void ActionCheck([CanBeNull]BaseAction act)
    {
        var curAction = act;
        if (curAction == null)
        {
            return;
        }
        if (curAction.ActionType == ActionType.attack)
        {
            var attackAction = curAction as AttackAction;
            if (attackAction != null)
            {
                CheckWithAttackAction(attackAction);
            }
        }

        if (curAction.ActionType == ActionType.evade || curAction.ActionType == ActionType.afterAttack)
        {
            if (_owner.Locator.DangerEnemy != null)
            {
                var trg = (_owner.Position + _owner.LookDirection * 5);
                var c = _owner.CellController.FindCell(trg);
                if (c.CellType == CellType.Free)
                {
                    BlinkTo(trg, _owner.LookDirection);
                }
            }
        }
    }

    private void ActionChanged(ShipBase arg1, BaseAction arg2)
    {
        ActionCheck(arg2);
    }

    private void CheckWithAttackAction(AttackAction attackAction)
    {

        var trg = attackAction.Target;
        var isInFront = trg.IsInFrontSector();
        var isClose = trg.Dist < 6 && trg.Dist > 2;
        if (!isInFront && isClose)
        {
            BlinkTo(trg.ShipLink.LookDirection * -5 + trg.ShipLink.Position, trg.ShipLink.LookDirection);
            Use();
        }
    }

    private void BlinkTo(Vector3 pos,Vector3 dir)
    {
        EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.BlinkPlaceEffect,pos,3f);
        EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.BlinkTargetEffect, _owner.transform, 3f);
        _owner.Rotation = Quaternion.FromToRotation(Vector3.forward, dir);
        _owner.Position = pos;
    }

    public override void Dispose()
    {
        _owner.OnActionChange -= ActionChanged;
        base.Dispose();
    }

    public override void Delete()
    {
        _owner.OnActionChange -= ActionChanged;
        base.Delete();
    }
}

