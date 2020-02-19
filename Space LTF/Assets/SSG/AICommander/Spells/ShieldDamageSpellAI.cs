using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;


public class ShieldDamageSpellAI : BaseAISpell<ShieldOffSpell>
{                                                
    public ShieldDamageSpellAI([NotNull] ShieldOffSpell spell, ShipControlCenter commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }        
}

