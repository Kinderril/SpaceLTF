using UnityEngine;


public class SameSidePersonalInfo : IShipData
{
    public ShipBase ShipLink { get; private set; }
    public Vector3 DirNorm => Utils.NormalizeFastSelf(ShipLink.Position - _requester.Position);
    public float Dist => (ShipLink.Position - _requester.Position).magnitude;
    public float Rating { get; private set; }
    public DebugRating DebugRating { get; }
    public bool Visible => true;
    private ShipBase _requester;

    public SameSidePersonalInfo(ShipBase infoShip, ShipBase requester)
    {
        _requester = requester;
        ShipLink = infoShip;

    }

    public bool IsInFrontSector()
    {
        return true;//TODO
    }

    public bool IsInBack()
    {
        return true;//TODO
    }
}
