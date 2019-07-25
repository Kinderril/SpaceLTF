using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;


public class MineFieldSpellAI : BaseAISpell<MineFieldSpell>
{
    private const float ShootDistSqrt = 45*45;

    public MineFieldSpellAI([NotNull] MineFieldSpell spell, Commander commander) 
        : base(spell, commander)
    {

    }

    protected override void PeriodInnerUpdate()
    {
        if (CanCast())
        {
            Vector3 trg;
            if (IsEnemyClose(out trg))
            {
                TryUse(trg);
            }
            else
            {
                FindCells();
            }
        }
    }

    private void FindCells()
    {
        var cc = _commander.Battlefield.CellController;
        var halfX = cc.Data.MaxIx/2;
        var halfZ = cc.Data.MaxIz/2;
        var cellCenter = cc.Data.GetCell(halfX, halfZ);
        var goodCell = cc.Data.FindClosestCellByType(cellCenter, CellType.Free);
        TryUse(goodCell.Center);
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


    private void TryUse(Vector3 v)
    {

        _spell.TryCast(_commander.CoinController, v);
    }
}

