using JetBrains.Annotations;


public class MachineGunSpellAI : BaseAISpell<MachineGunSpell>
{
    public MachineGunSpellAI([NotNull] MachineGunSpell spell, ShipControlCenter commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }
}

