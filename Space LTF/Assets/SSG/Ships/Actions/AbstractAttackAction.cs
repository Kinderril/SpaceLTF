using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;


public  abstract class AbstractAttackAction : BaseAction
{
    public AbstractAttackAction([NotNull] ShipBase owner, ActionType actionType)
        : base(owner, actionType)
    {

    }
}

