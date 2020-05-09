using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;


public class AutoAICommander : IAICommander
{
    private CommanderSpells _commanderSpells;

    Dictionary<SpellInGame,AutoSpellContainer> _spells = new Dictionary<SpellInGame, AutoSpellContainer>();
    public AutoAICommander(Commander commanderOwner)
        : base(commanderOwner.MainShip, commanderOwner)
    {
        _commanderSpells = commanderOwner.SpellController;
        CreateAllAiSpells();
    }

    protected override void CreateAllAiSpells()
    {
        foreach (var spellModulInv in _commanderSpells.AllSpells)
        {
            if (spellModulInv != null)
            {
                AutoSpellContainer spell = new AutoSpellContainer(_shipControl, spellModulInv);
                _spells.Add(spellModulInv,spell);

            }
        }

    }

    public override void ManualUpdate()
    {
        if (enable && Time.time - _createdTime > CAST_PERIOD)
        {
            foreach (var autoSpellContainer in _spells)
            {
                if (autoSpellContainer.Value.IsActive)
                {
                    autoSpellContainer.Value.PeriodlUpdate();
                }
            }
//            var myArmyCount = _commander.Ships.Count;
//            for (int i = 0; i < _spells.Length; i++)
//            {
//                var spell = _spells[i];
//                spell.ManualUpdate();
//                spell.PeriodlUpdate(myArmyCount);
//            }

        }
    }


    public AutoSpellContainer GetAutoSpell(SpellInGame baseSpellModul)
    {
        return _spells[baseSpellModul];
    }
}