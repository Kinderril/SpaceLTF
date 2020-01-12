using UnityEngine;

public enum HitCatcherType
{
    body,
    shield,
}

[RequireComponent(typeof(Collider))]
public class ShipHitCatcher : HitCatcher
{
    public ShipBase ShipBase;
    public HitCatcherType CatcherType = HitCatcherType.body;

    void Awake()
    {
        gameObject.tag = TagController.OBJECT_MOVER_TAG;
        var collider = GetComponent<Collider>();
        collider.isTrigger = true;
#if UNITY_EDITOR
        if (ShipBase == null)
        {
            Debug.LogError($"{transform.parent.gameObject.name} have wrong ShipHitCatcher");
        }
#endif
    }

    public TeamIndex TeamIndex
    {
        get { return ShipBase.TeamIndex; }
    }

    public override void GetHit(IWeapon weapon,Bullet bullet)
    {
        ShipBase.GetHit(weapon, bullet);
    }
}