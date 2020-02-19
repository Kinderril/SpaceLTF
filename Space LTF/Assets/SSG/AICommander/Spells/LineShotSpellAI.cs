using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;


public class LineShotSpellAI : BaseAISpell<LineShotSpell>
{

    public LineShotSpellAI([NotNull] LineShotSpell spell, ShipControlCenter commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }




}

