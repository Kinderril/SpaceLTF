using System.Collections.Generic;

[System.Serializable]
public class WeaponInventoryAffectTarget
{
    public AffectTargetDelegate Main;
    public List<AffectTargetDelegate> Additional = new List<AffectTargetDelegate>();
    public TargetType TargetType = TargetType.Ally;

    public WeaponInventoryAffectTarget(AffectTargetDelegate Main, TargetType targetType)
    {
        this.Main = Main;
        TargetType = targetType;
    }

    public void Add(AffectTargetDelegate affectTarget)
    {
        Additional.Add(affectTarget);
    }
}
