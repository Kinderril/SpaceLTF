using UnityEngine;
using System.Collections;

public interface IShipDangerPoint 
{

    Vector3 DirToAsteroidNorm { get; set; }
    float DistToShip { get; set; }
    Vector3 Position { get; }
    float Rad { get; }
}
