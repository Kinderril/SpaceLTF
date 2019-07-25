using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CommanderRewardController
{
    public int CurCredit { get; private set; }
    public event Action<int> OnCreditChange;
    public event Action<ShipBase> OnTargetRewardChange;
    private Commander _commander;

    public CommanderRewardController(Commander commander)
    {
        _commander = commander;
        CurCredit = 1000;
    }

    public bool TryRewardObject(ShipBase selectedShip)
    {
        return false;
    }
    
}

