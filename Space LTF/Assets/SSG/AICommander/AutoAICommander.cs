using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;


public class AutoAICommander : IAICommander
{
    public AutoAICommander(ShipControlCenter shipControl, Commander commanderOwner)
        : base(shipControl, commanderOwner)
    {

    }

    protected override void CreateAllAiSpells()
    {
        foreach (var baseSpellModulInv in _shipControl.ShipInventory.SpellsModuls)
        {
            if (baseSpellModulInv != null)
            {
                var v = Create(baseSpellModulInv);

            }
        }

    }

    public override void ManualUpdate()
    {
        if (enable && Time.time - _createdTime > CAST_PERIOD)
        {
//            var myArmyCount = _commander.Ships.Count;
//            for (int i = 0; i < _spells.Length; i++)
//            {
//                var spell = _spells[i];
//                spell.ManualUpdate();
//                spell.PeriodlUpdate(myArmyCount);
//            }

        }
    }



}