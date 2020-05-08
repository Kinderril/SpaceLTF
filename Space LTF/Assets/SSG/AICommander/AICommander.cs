using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;


public class AICommander : IAICommander
{
    protected BaseAISpell[] _spells;
    protected AICommanderMainShip _mainShip;

    protected override void CreateAllAiSpells()
    {
        var aiPlayer = _commander.Player as PlayerAI;
        if (aiPlayer != null)
        {
            _startAtStart = aiPlayer.DoBaseDefence();
        }
        _mainShip = new AICommanderMainShip(_shipControl);
        var spellTmp = new List<BaseAISpell>();
        foreach (var baseSpellModulInv in _shipControl.ShipInventory.SpellsModuls)
        {
            if (baseSpellModulInv != null)
            {
                var v = Create(baseSpellModulInv);
                if (v != null)
                {
                    spellTmp.Add(v);
                }
            }
        }
        _spells = spellTmp.ToArray();
        if (_spells.Length == 0)
        {
            Debug.LogError(
                $"AI commander have no spells to use spell modules:{_shipControl.ShipInventory.SpellsModuls}");
        }
        Debug.Log("AIcommander Inited. Spells: " + _spells.Length);

    }



    public override void ManualUpdate()
    {
        if (enable && Time.time - _createdTime > CAST_PERIOD)
        {
            var myArmyCount = _commander.Ships.Count;
            for (int i = 0; i < _spells.Length; i++)
            {
                var spell = _spells[i];
                spell.ManualUpdate();
                spell.PeriodlUpdate(myArmyCount);
            }

            if (!_startAtStart)
                _mainShip.UpdateMove();
        }
    }



    public AICommander(ShipControlCenter shipControl, Commander commanderOwner) 
        : base(shipControl, commanderOwner)
    {

    }
}