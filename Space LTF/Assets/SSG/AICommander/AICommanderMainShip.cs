using UnityEngine;
using System.Collections;
using System.Linq;

public class AICommanderMainShip
{
    private ShipControlCenter _mainShipBase;
//    private Commander _commander;
    private float _nextTimeMove;
    private Vector3 _lastTarget;
    public AICommanderMainShip(ShipControlCenter commander)
    {
//        _commander = commander;
        _mainShipBase = commander;
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
        var enemies = _mainShipBase.Enemies.Keys.ToList();
        var rndEnemy = enemies.RandomElement();
        return rndEnemy.Position;
//        int p = 0;
//        Vector3 totalPos = Vector3.zero;
//        foreach (var ship in _commander.Ships)
//        {
//            if (ship.Value.ShipParameters.StartParams.ShipType != ShipType.Base)
//            {
//                totalPos += ship.Value.Position;
//                p++;
//            }
//        }
//
//        var midPos = totalPos / p;
//        return midPos;
    }

}
