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
    private AICommanderMainShip _mainShip;
    private float _createdTime;

    public AICommander(Commander commander)
    {
        _createdTime = Time.time;
        _commander = commander;
        var spellTmp = new List<BaseAISpell>();
        enable = _commander.MainShip != null;
        if (enable)
        {
            _commander.MainShip.OnDeath += OnDeath;
            _mainShip = new AICommanderMainShip(_commander);
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

            _spells = spellTmp.ToArray();
            if (_spells.Length == 0)
            {
                Debug.LogError(
                    $"AI commander have no spells to use spell modules:{commander.MainShip.ShipInventory.SpellsModuls}");
            }
            Debug.Log("AIcommander Inited. Spells: " + _spells.Length);
        }


    }

    private void OnDeath(ShipBase obj)
    {
        Dispose();
        enable = false;
    }

    [CanBeNull]
    private BaseAISpell Create(BaseSpellModulInv baseSpellModul, Commander commander)
    {
        var mainShip = commander.MainShip;
        var spellInGame = new SpellInGame(baseSpellModul, () => mainShip.Position, mainShip.TeamIndex, mainShip, 1,
            baseSpellModul.Name, baseSpellModul.CostTime, baseSpellModul.CostCount, baseSpellModul.SpellType,
            baseSpellModul.BulleStartParameters.distanceShoot, baseSpellModul.Desc(), baseSpellModul.DiscCounter);


        switch (baseSpellModul.SpellType)
        {
            case SpellType.shildDamage:
                return new ShieldDamageSpellAI(baseSpellModul as ShieldOffSpell, commander, spellInGame);
            case SpellType.engineLock:
                return new EngineLockSpellAI(baseSpellModul as EngineLockSpell, commander, spellInGame);
            case SpellType.lineShot:
                return new LineShotSpellAI(baseSpellModul as LineShotSpell, commander, spellInGame);
            case SpellType.distShot:
                return new DistShotSpellAI(baseSpellModul as DistShotSpell, commander, spellInGame);
            case SpellType.mineField:
                return new MineFieldSpellAI(baseSpellModul as MineFieldSpell, commander, spellInGame);
            case SpellType.randomDamage:
                return new RandomDamageSpellAI(baseSpellModul as RandomDamageSpell, commander, spellInGame);
            case SpellType.artilleryPeriod:
                return new ArtilleryAI(baseSpellModul as ArtillerySpell, commander, spellInGame);
            case SpellType.repairDrones:
                break;
            case SpellType.throwAround:
                return new ThrowAroundAI(baseSpellModul as ThrowAroundSpell, commander, spellInGame);
        }

        return null;
    }

    public void ManualUpdate()
    {
        if (enable && Time.time - _createdTime > 4)
        {
            for (int i = 0; i < _spells.Length; i++)
            {
                var spell = _spells[i];
                spell.ManualUpdate();
                spell.PeriodlUpdate();
            }

            _mainShip.Update();
        }
    }

    public void Dispose()
    {
        if (enable)
        {
            _commander.MainShip.OnDeath -= OnDeath;
        }
    }
}