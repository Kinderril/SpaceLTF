using System;
using UnityEngine;





public class ShipPersonalInfo : IShipData, IDisposable
{
    public ShipBase ShipLink { get; }
    public Vector3 DirNorm { get; private set; }
    public float Dist { get; private set; }
    public float Rating { get; private set; }
    public bool IsDead { get; private set; }
    public DebugRating DebugRating { get; private set; }

    private ShipBase _owner;

    public bool Visible => ShipLink.VisibilityData.Visible;


    public ShipPersonalInfo(ShipBase owner, ShipBase mover)
    {
        IsDead = false;
        _owner = owner;
        ShipLink = mover;
        ShipLink.OnDeath += OnDeath;
    }

    private void OnDeath(ShipBase obj)
    {
        IsDead = true;
    }

    public void SetParams(Vector3 dir, float dist)
    {
        Dist = dist;
        DirNorm = Utils.NormalizeFastSelf(dir); //Направление ИЗ нас в ЦЕЛЬ
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

    public void Dispose()
    {
        if (ShipLink != null)
        {
            ShipLink.OnDeath -= OnDeath;
        }
    }
}

