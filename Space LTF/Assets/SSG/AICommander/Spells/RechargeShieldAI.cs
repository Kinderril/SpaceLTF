using JetBrains.Annotations;


public class RechargeShieldAI : AffectMyShipAISpell<RechargeShieldSpell>
{
    public RechargeShieldAI([NotNull] RechargeShieldSpell spell, Commander commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }

}

