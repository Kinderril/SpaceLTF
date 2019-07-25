using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EndGameWaitAction : BaseAction
{
    public EndGameWaitAction(ShipBase owner) : base(owner, ActionType.waitEdnGame)
    {
    }

    public override void ManualUpdate()
    {
        _owner.SetTargetSpeed(0.01f);
    }

    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
            new CauseAction("out bf", () => _owner.InBattlefield),
        };
        return c;
    }
}

