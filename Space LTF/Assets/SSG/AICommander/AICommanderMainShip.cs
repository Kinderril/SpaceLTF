using UnityEngine;
using System.Collections;

public class AICommanderMainShip
{
    private ShipBase _mainShipBase;
    private Commander _commander;
    private float _nextTimeMove;
    private Vector3 _lastTarget;
    public AICommanderMainShip(Commander commander)
    {
        _commander = commander;
        _mainShipBase = commander.MainShip;
    }

    public void Update()
    {
         if (_nextTimeMove < Time.time)
         {
             _nextTimeMove = Time.time + 7f;
             var midPos = MidPos();
             var sDelta = (_lastTarget - midPos).sqrMagnitude;
             if (sDelta > 4)
             {
                 MoveTo(midPos);
             }
         }
    }

    private void MoveTo(Vector3 midPos)
    {
        _mainShipBase.GoToPointAction(midPos,false);
    }

    private Vector3 MidPos()
    {
        Vector3 totalPos = Vector3.zero;
        int p = 0;
        foreach (var ship in _commander.Ships)
        {
            if (ship.Value.ShipParameters.StartParams.ShipType != ShipType.Base)
            {
                totalPos += ship.Value.Position;
                p++;
            }
        }

        var midPos = totalPos / p;
        return midPos;
    }

}
