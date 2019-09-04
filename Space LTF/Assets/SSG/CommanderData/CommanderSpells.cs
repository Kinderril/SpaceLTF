using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CommanderSpells
{
    private Commander _commander;
    private const float DistToBlock = 10f;
    public List<SpellInGame> AllSpells = new List<SpellInGame>();


    public CommanderSpells(Commander commander)
    {
        _commander = commander;
    }


    public void AddPriorityTarget()
    {
        ShipBase mainShip = _commander.MainShip;
        var priority = new CommanderSpellPriorityTarget();
        var spellInGame = new SpellInGame(priority,()=> mainShip.Position, mainShip.TeamIndex, mainShip, 1,
            Namings.PriorityTarget, Library.PriorityTargetCostTime, Library.PriorityTargetCostCount, SpellType.priorityTarget,9999);
        AllSpells.Add(spellInGame);

        var priorityBait = new CommanderSpellPriorityBait();
        var baieSpell = new SpellInGame(priorityBait, () => mainShip.Position, mainShip.TeamIndex, mainShip, 1,
            Namings.FakePriorityTarget, Library.BaitPriorityTargetCostTime, Library.BaitPriorityTargetCostCount, SpellType.BaitPriorityTarget,9999);
        AllSpells.Add(baieSpell);
    }


    public bool TryCastspell(SpellInGame spell,Vector3 trg)
    {
        return spell.TryCast(_commander.CoinController, trg);
    }

    public void AddSpell(BaseSpellModulInv baseSpellModul, Transform modulPos)
    {

        ShipBase mainShip = _commander.MainShip;
        var spellInGame= new SpellInGame(baseSpellModul, () => modulPos.position,mainShip.TeamIndex,mainShip,1,
            baseSpellModul.Name, baseSpellModul.CostTime, baseSpellModul.CostCount,baseSpellModul.SpellType, baseSpellModul.BulleStartParameters.distanceShoot);
        AllSpells.Add(spellInGame);
    }
}

