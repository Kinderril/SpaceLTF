using JetBrains.Annotations;


public class RechargeShieldAI : AffectMyShipAISpell<RechargeShieldSpell>
{
    public RechargeShieldAI([NotNull] RechargeShieldSpell spell, ShipControlCenter commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }

}

