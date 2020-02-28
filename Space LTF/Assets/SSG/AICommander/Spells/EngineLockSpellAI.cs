using JetBrains.Annotations;


public class EngineLockSpellAI : BaseAISpell<EngineLockSpell>
{
    public EngineLockSpellAI([NotNull] EngineLockSpell spell, ShipControlCenter commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }

    protected override bool CanCastByCount(int myArmyCount)
    {
        return myArmyCount > 1;
    }
}

