using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class AICommander
{
    private Commander _commander;
    private bool enable = false;
    private BaseAISpell[] _spells;

    public AICommander(Commander commander)
    {
        _commander = commander;
        var spellTmp = new List<BaseAISpell>();
        enable = _commander.MainShip != null;
        if (enable)
        {
            foreach (var baseSpellModulInv in commander.MainShip.ShipInventory.SpellsModuls)
            {
                if (baseSpellModulInv != null)
                {
                    var v = Create(baseSpellModulInv, commander);
                    if (v != null)
                    {
                        spellTmp.Add(v);
                    }
                }
            }
        }
        _spells = spellTmp.ToArray();
        Debug.Log("AIcommander Inited. Spells: " +_spells.Length);
    }

    [CanBeNull]
    private BaseAISpell Create(BaseSpellModulInv baseSpellModulInv,Commander commander)
    {
        switch (baseSpellModulInv.SpellType)
        {
            case SpellType.shildDamage:
                return new ShieldDamageSpellAI(baseSpellModulInv as ShieldOffSpell, commander);
            case SpellType.engineLock:
                return new EngineLockSpellAI(baseSpellModulInv as EngineLockSpell, commander);
            case SpellType.lineShot:
                return new LineShotSpellAI(baseSpellModulInv as LineShotSpell, commander);
            case SpellType.distShot:
                return new DistShotSpellAI(baseSpellModulInv as DistShotSpell, commander);
            case SpellType.mineField:
                return new MineFieldSpellAI(baseSpellModulInv as MineFieldSpell, commander);
            case SpellType.randomDamage:
                return new RandomDamageSpellAI(baseSpellModulInv as RandomDamageSpell, commander);
                break;
            case SpellType.throwAround:
                break;
//            case SpellType.allToBase:
//                break;
//            case SpellType.invisibility:
//                break;
//            case SpellType.spaceWall:
//                break;
        }
        return null;
    }

    public void ManualUpdate()
    {
        if (enable)
        {
            for (int i = 0; i < _spells.Length; i++)
            {
                var spell = _spells[i];
                spell.ManualUpdate();
                spell.PeriodlUpdate();
            }
        }
    }
}

