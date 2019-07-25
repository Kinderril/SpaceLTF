using UnityEngine;

public enum HitCatcherType
{
    body,
    shield,
}

public class ShipHitCatcher : HitCatcher
{
    public ShipBase ShipBase;
    public HitCatcherType CatcherType = HitCatcherType.body;

    public TeamIndex TeamIndex
    {
        get { return ShipBase.TeamIndex; }
    }

    public override void GetHit(IWeapon weapon,Bullet bullet)
    {
        ShipBase.GetHit(weapon, bullet);
    }
}