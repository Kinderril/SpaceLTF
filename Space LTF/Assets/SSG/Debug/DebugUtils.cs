using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class DebugUtils
{
    public static void KillAllEnemies()
    {
        var cmd = BattleController.Instance.RedCommander;
        foreach (var shipsValue in cmd.Ships.Values)
        {
            shipsValue.ShipParameters.Damage(100, 100, (delta, shieldDelta, attacker) =>
                { },shipsValue);
            shipsValue.ShipParameters.Damage(100, 100, (delta, shieldDelta, attacker) => { },shipsValue);
        }
    }
}

