using JetBrains.Annotations;


public class RepairDropneAI : AffectMyShipAISpell<RepairDronesSpell>
{
    public RepairDropneAI([NotNull] RepairDronesSpell spell, Commander commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }

}

