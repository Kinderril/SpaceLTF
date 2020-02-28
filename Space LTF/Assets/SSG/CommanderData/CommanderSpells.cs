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
        var radius = 7 + _commander.Player.Parameters.EnginePower.Level * 3;
        var delay = 20 - _commander.Player.Parameters.EnginePower.Level * 2;

        ShipBase mainShip = _commander.MainShip;

        Vector3 posCutter(Vector3 maxdistpos, Vector3 targetdistpos)
        {
            var distFromShip = (targetdistpos - mainShip.Position).magnitude;
            if (distFromShip > radius)
            {
                return maxdistpos;
            }
            else
            {
                return targetdistpos;
            }
        }

        var priority = new CommanderSpellMainShipBlink(radius, mainShip);
        var spellInGame = new SpellInGame(priority, () => mainShip.Position, mainShip.TeamIndex, mainShip, 1,
            Namings.Tag("MainShipBlinkName"), 0, 0, SpellType.mainShipBlink, radius, Namings.Tag("MainShipBlinkDesc"),
            posCutter, delay);
        AllSpells.Add(spellInGame);
    }
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

