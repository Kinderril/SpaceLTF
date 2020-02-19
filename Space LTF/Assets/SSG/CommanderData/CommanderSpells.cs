using System.Collections.Generic;
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


    public void AddMainShipBlink()
    {
        var Radius = 10 + _commander.Player.Parameters.EnginePower.Level * 5;
        var delay = 20 - _commander.Player.Parameters.EnginePower.Level * 2;

        ShipBase mainShip = _commander.MainShip;
        var priority = new CommanderSpellMainShipBlink(Radius, mainShip);
        var spellInGame = new SpellInGame(priority, () => mainShip.Position, mainShip.TeamIndex, mainShip, 1,
            Namings.Tag("MainShipBlinkName"), 0, 0, SpellType.mainShipBlink, Radius, Namings.Tag("MainShipBlinkDesc"), (pos, distPos) => distPos, delay);
        AllSpells.Add(spellInGame);
    }

    // public void AddPriorityTarget()
    // {
    //     ShipBase mainShip = _commander.MainShip;
    //     var priority = new CommanderSpellPriorityTarget();
    //     var spellInGame = new SpellInGame(priority, () => mainShip.Position, mainShip.TeamIndex, mainShip, 1,
    //         Namings.Tag("PriorityTarget"), Library.PriorityTargetCostTime, Library.PriorityTargetCostCount, SpellType.priorityTarget, 9999, Namings.Tag("PriorityTargetDesc"), (pos, distPos) => distPos, 1f);
    //     AllSpells.Add(spellInGame);
    //
    //     var priorityBait = new CommanderSpellPriorityBait();
    //     var baieSpell = new SpellInGame(priorityBait, () => mainShip.Position, mainShip.TeamIndex, mainShip, 1,
    //         Namings.Tag("BaitPriorityTarget"), Library.BaitPriorityTargetCostTime, Library.BaitPriorityTargetCostCount, SpellType.BaitPriorityTarget, 9999, Namings.Tag("BaitPriorityTargetDesc"), (pos, distPos) => distPos, 1f);
    //     AllSpells.Add(baieSpell);
    // }

    public bool TryCastspell(SpellInGame spell, Vector3 trg)
    {
        return spell.TryCast(trg);
    }

    public void AddSpell(BaseSpellModulInv baseSpellModul, Transform modulPos)
    {

        ShipBase mainShip = _commander.MainShip;
        var spellInGame = new SpellInGame(baseSpellModul, () => modulPos.position, mainShip.TeamIndex, mainShip, 1,
            baseSpellModul.Name, baseSpellModul.CostTime, baseSpellModul.CostCount, baseSpellModul.SpellType,
            baseSpellModul.BulleStartParameters.distanceShoot, baseSpellModul.DescFull(), baseSpellModul.DiscCounter, 1f);
        AllSpells.Add(spellInGame);
    }
}

