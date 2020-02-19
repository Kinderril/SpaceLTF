﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;


public class EngineLockSpellAI : BaseAISpell<EngineLockSpell>
{
    public EngineLockSpellAI([NotNull] EngineLockSpell spell, ShipControlCenter commander, SpellInGame spellData)
        : base(spellData, spell, commander)
    {

    }   

}

