using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ShipPersonalInfo
{
    public ShipBase ShipLink;
    public CommanderShipEnemy CommanderShipEnemy { get; private set; }
    public Vector3 DirNorm { get; private set; }
    public float Dist { get; private set; }
    public float Rating { get; private set; }
    public DebugRating DebugRating { get; private set; }
    public bool CanShoot { get; set; }

    public bool Visible
    {
        get
        {
            return ShipLink.VisibilityData.Visible;
        }
    }

    public ShipPersonalInfo(ShipBase mover, CommanderShipEnemy commanderShipEnemy)
    {
        CommanderShipEnemy = commanderShipEnemy;
        ShipLink = mover;
    }

    public void SetParams(Vector3 dir, float dist)
    {
        Dist = dist;
        DirNorm = Utils.NormalizeFastSelf(dir);
    }

    public void SetRaing(float total)
    {
        Rating = total;
    }

    public void SetDebugRaing(DebugRating debugRating)
    {
        if (DebugRating == null)
        {
            DebugRating = debugRating;
        }
        else
        {
            DebugRating.UpdateData(debugRating);
        }
    }
}

