using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;


public class DistShotSpellAI : BaseAISpell<DistShotSpell>
{
    public DistShotSpellAI([NotNull] DistShotSpell spell, Commander commander) 
        : base(spell, commander)
    {

    }       
}

