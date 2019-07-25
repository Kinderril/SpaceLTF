using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public  abstract class ShipData
{
    protected ShipBase _owner;

    public ShipData(ShipBase owner)
    {
        _owner = owner;
    }
}

