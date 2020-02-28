using JetBrains.Annotations;


public class ThrowAroundAI : BaseAISpell<ThrowAroundSpell>
{
    public ThrowAroundAI([NotNull] ThrowAroundSpell spell, ShipControlCenter commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }
    protected override bool CanCastByCount(int myArmyCount)
    {
        return myArmyCount > 1;
    }

}

