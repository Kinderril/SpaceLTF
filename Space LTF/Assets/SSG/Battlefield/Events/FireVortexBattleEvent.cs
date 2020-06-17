using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireVortexBattleEvent : SetterElementBattleEvent
{
    public FireVortexBattleEvent(BattleController battle, float insideRad) :
        base(battle, 1, 2,
            DataBaseController.Instance.DataStructPrefabs.FireVortexs, EBattlefildEventType.fireVortex, insideRad)
    {

    }

    public override void Init()
    {
        base.Init();

        foreach (var createdElement in _createdElements)
        {
            var pos = createdElement.transform.position;
            var aiCell = _battle.Battlefield.CellController.Data.GetCellByPos(pos);
            aiCell.AddDangerPoint(pos, 5f);
        }
    }
}
