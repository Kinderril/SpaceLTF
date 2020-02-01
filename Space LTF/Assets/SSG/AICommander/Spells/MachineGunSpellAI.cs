using JetBrains.Annotations;


public class MachineGunSpellAI : BaseAISpell<MachineGunSpell>
{
    public MachineGunSpellAI([NotNull] MachineGunSpell spell, Commander commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }
}

