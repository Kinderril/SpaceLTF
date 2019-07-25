using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ShipBornPosition
{
    public Vector3 direction = Vector3.zero;
    public Vector3 position = Vector3.zero;

    public ShipBornPosition(Vector3 position, Vector3 direction)
    {
        this.position = position;
        this.direction = direction;
    }
}

