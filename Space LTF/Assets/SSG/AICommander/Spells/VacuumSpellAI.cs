using JetBrains.Annotations;


public class VacuumSpellAI : BaseAISpell<VacuumSpell>
{
    public VacuumSpellAI([NotNull] VacuumSpell spell, Commander commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }

}

