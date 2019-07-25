using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WeaponInventoryAffectTarget
{
    public AffectTargetDelegate Main;
    public List<AffectTargetDelegate> Additional = new List<AffectTargetDelegate>();

    public WeaponInventoryAffectTarget(AffectTargetDelegate Main)
    {
        this.Main = Main;
    }

    public void Add(AffectTargetDelegate affectTarget)
    {
                                   Additional.Add(affectTarget);
    }
}
