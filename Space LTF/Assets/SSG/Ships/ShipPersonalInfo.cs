using UnityEngine;


public class ShipPersonalInfo
{
    public ShipBase ShipLink;
    public CommanderShipEnemy CommanderShipEnemy { get; private set; }
    public Vector3 DirNorm { get; private set; }
    public float Dist { get; private set; }
    public float Rating { get; private set; }
    public DebugRating DebugRating { get; private set; }
    // public bool CanShoot { get; set; }
    private ShipBase _owner;

    public bool Visible => ShipLink.VisibilityData.Visible;

    public ShipPersonalInfo(ShipBase owner, ShipBase mover, CommanderShipEnemy commanderShipEnemy)
    {
        _owner = owner;
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

    public bool IsInFrontSector()
    {
        var isAng = Utils.IsAngLessNormazied(_owner.LookDirection, DirNorm, UtilsCos.COS_90_RAD);
        return isAng;
    }
    public bool IsInBack()
    {
        var isAng = Utils.FastDot(_owner.LookDirection, DirNorm) < 0;
        return isAng;
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

