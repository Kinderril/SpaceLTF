using JetBrains.Annotations;
using UnityEngine;


public class MineFieldSpellAI : BaseAISpell<MineFieldSpell>
{
    private const float ShootDistSqrt = 45 * 45;

    public MineFieldSpellAI([NotNull] MineFieldSpell spell, ShipControlCenter commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }

    protected override void PeriodInnerUpdate(int myArmyCount)
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

        var cc = BattleController.Instance.Battlefield.CellController;
        var halfX = cc.Data.MaxIx / 2;
        var halfZ = cc.Data.MaxIz / 2;
        var cellCenter = cc.Data.GetCell(halfX, halfZ);
        var goodCell = cc.Data.FindClosestCellByType(cellCenter, CellType.Free);
        TryUse(goodCell.Center);
    }

    private bool IsEnemyClose(out Vector3 trg)
    {
        float sDist;
        var ship = BattleController.Instance.ClosestShipToPos(_owner.Position, oIndex, out sDist);
        if (sDist < ShootDistSqrt)
        {
            trg = ship.Position;
            return true;
        }
        trg = Vector3.zero;
        return false;
    }

}

