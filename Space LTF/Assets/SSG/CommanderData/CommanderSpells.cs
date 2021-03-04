using System.Collections.Generic;
using UnityEngine;


public class CommanderSpells
{
    public const float COMMANDER_BLINK_BASE_PERIOD = 2;
    public const float COMMANDER_BLINK_LEVEL_PERIOD = 2;
    public const float COMMANDER_BLINK_BASE_DIST = 7;
    public const float COMMANDER_BLINK_LEVEL_DIST = 3;

    private Commander _commander;
    private const float DistToBlock = 10f;
    public List<SpellInGame> AllSpells = new List<SpellInGame>();


    public CommanderSpells(Commander commander)
    {
        _commander = commander;
    }

    public void Dispose()
    {
        foreach (var spellInGame in AllSpells)
        {
            spellInGame.Dispose();
        }
        AllSpells.Clear();
    }

    public void AddMainShipBlink()
    {
        var radius = COMMANDER_BLINK_BASE_DIST + _commander.Player.Parameters.EnginePower.Level * COMMANDER_BLINK_LEVEL_DIST;
        var delay = COMMANDER_BLINK_BASE_PERIOD - _commander.Player.Parameters.EnginePower.Level * COMMANDER_BLINK_LEVEL_PERIOD;

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
            Namings.Tag("MainShipBlinkName"),  15, SpellType.mainShipBlink, Namings.Tag("MainShipBlinkDesc"),
            posCutter, delay, new CurWeaponDamage(0,0));
        AllSpells.Add(spellInGame);
    }
    public bool TryCastspell(SpellInGame spell, Vector3 trg)
    {
        return spell.TryCast(trg);
    }

    public void AddSpell(BaseSpellModulInv baseSpellModul, Transform modulPos,List<BaseModulInv> moduls)
    {

        ShipBase mainShip = _commander.MainShip;
        var spellInGame = new SpellInGame(baseSpellModul, () => modulPos.position, mainShip.TeamIndex, mainShip, 1,
            baseSpellModul.Name, baseSpellModul.CostTime, baseSpellModul.SpellType,
             baseSpellModul.DescFull(),
            baseSpellModul.DiscCounter, 1f, baseSpellModul.CurrentDamage);
        foreach (var modul in moduls)
        {
            var support = modul as BaseSupportModul;
            if (support != null)
            {
//                Debug.LogError($"Apply to spell:{baseSpellModul.Name}  modul:{modul.Name}");
                support.ChangeBullet(spellInGame);
                support.ChangeTargetAffect(spellInGame);
                support.ChangeParams(spellInGame);
                spellInGame.AddInfoForTooltip(support.DescSupport());
            }
        }

        AllSpells.Add(spellInGame);
    }
}

