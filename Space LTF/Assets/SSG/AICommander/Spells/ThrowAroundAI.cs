using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;


public class ThrowAroundAI : BaseAISpell<ThrowAroundSpell>
{
    public ThrowAroundAI([NotNull] ThrowAroundSpell spell, Commander commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }   

}

