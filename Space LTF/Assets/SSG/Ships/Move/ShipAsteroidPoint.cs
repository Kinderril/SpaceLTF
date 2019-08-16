using UnityEngine;
using System.Collections;

public class ShipAsteroidPoint
{
    private AIAsteroidPredata cellASteroid;

    public Vector3 DirToAsteroidNorm { get; set; }
    public float DistToShip { get; set; }
    public Vector3 Position => cellASteroid.Position;
    public float Rad => cellASteroid.Rad;



    public ShipAsteroidPoint(AIAsteroidPredata cellASteroid)
    {
        this.cellASteroid = cellASteroid;
    }
}
