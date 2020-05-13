using System;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;


public class AutoAICommander : IAICommander
{
    private CommanderSpells _commanderSpells;
    public event Action<bool, int> OnSpellActivated;

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
                AutoSpellContainer spell = new AutoSpellContainer(_shipControl, spellModulInv,OnSpellAutoActivated);
                _spells.Add(spellModulInv,spell);

            }
        }

    }

    private void OnSpellAutoActivated(int shipId, bool val)
    {
        OnSpellActivated?.Invoke(val,shipId);
    }        

    public override void ManualUpdate()
    {
        if (enable && Time.time - _createdTime > CAST_PERIOD && Time.timeScale > 0.0001f)
        {
            foreach (var autoSpellContainer in _spells)
            {
                if (autoSpellContainer.Value.IsActive)
                {
                    autoSpellContainer.Value.PeriodlUpdate();
                }
            }
        }
    }


    public AutoSpellContainer GetAutoSpell(SpellInGame baseSpellModul)
    {
        return _spells[baseSpellModul];
    }

    public void ActivateAiSpell(SpellInGame spellSelected, ShipBase getClosest)
    {
        var setTarget = _spells[spellSelected];
        setTarget.SetActive(true,getClosest);
    }

    public void Dispose()
    {
        foreach (var autoSpellContainer in _spells)
        {
            autoSpellContainer.Value.Dispose();
        }
        _spells.Clear();
    }
}