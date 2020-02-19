using JetBrains.Annotations;


public class VacuumSpellAI : BaseAISpell<VacuumSpell>
{
    public VacuumSpellAI([NotNull] VacuumSpell spell, ShipControlCenter commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }

}

