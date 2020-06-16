using UnityEngine;
using System.Collections;

public class ShipDangerAnomalyPoint : IShipDangerPoint
{

    public Vector3 DirToAsteroidNorm { get; set; }
    public float DistToShip { get; set; }
    public Vector3 Position { get; set; }
    public float Rad { get; set; }



    public ShipDangerAnomalyPoint(Vector3 Position, float Rad)
    {
        this.Position = Position;
        this.Rad = Rad;
    }
}
