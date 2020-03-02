using UnityEngine;

public interface IShipData
{
    ShipBase ShipLink { get; }
    Vector3 DirNorm { get; }
    float Dist { get; }
    float Rating { get; }
    DebugRating DebugRating { get; }
    bool Visible { get; }
    bool IsInFrontSector();
    bool IsInBack();
}