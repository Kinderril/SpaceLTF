using JetBrains.Annotations;


public class ShieldDamageSpellAI : BaseAISpell<ShieldOffSpell>
{
    public ShieldDamageSpellAI([NotNull] ShieldOffSpell spell, ShipControlCenter commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }
    protected override bool CanCastByCount(int myArmyCount)
    {
        return myArmyCount > 1;
    }
}

