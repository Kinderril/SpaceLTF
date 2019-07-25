using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;


public class LineShotSpellAI : BaseAISpell<LineShotSpell>
{
    private const float ShootDistSqrt = 15*15;

    public LineShotSpellAI([NotNull] LineShotSpell spell, Commander commander) 
        : base(spell, commander)
    {

    }

    protected override void PeriodInnerUpdate()
    {
        Vector3 trg;
        if (CanCast())
        {
            if (IsEnemyClose(out trg))
            {
                TryUse(trg);
            }
        }
    }

    private bool IsEnemyClose(out Vector3 trg)
    {
        var oIndex = BattleController.OppositeIndex(_commander.TeamIndex);
        float sDist;
        var ship = BattleController.Instance.ClosestShipToPos(_commander.MainShip.Position, oIndex,out sDist);
        if (sDist < ShootDistSqrt)
        {
            trg = ship.Position;
            return true;
        }
        trg = Vector3.zero;
        return false;
    }


}

