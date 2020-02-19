using JetBrains.Annotations;


public class HookShotSpellAI : BaseAISpell<HookShotSpell>
{
    public HookShotSpellAI([NotNull] HookShotSpell spell, ShipControlCenter commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }

}

