using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class ShipWayController : ShipData
{
    public Vector3 Target;

    public ShipWayController([NotNull] ShipBase owner) 
        : base(owner)
    {

    }
}

