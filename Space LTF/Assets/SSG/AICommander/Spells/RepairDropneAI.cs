using JetBrains.Annotations;


public class RepairDropneAI : AffectMyShipAISpell<RepairDronesSpell>
{
    public RepairDropneAI([NotNull] RepairDronesSpell spell, ShipControlCenter commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }

}

