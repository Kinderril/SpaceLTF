using UnityEngine;
using System.Collections;

public class TurretConnectorContainer : MovingObject
{
    public TurretConnector Connector;

    public override void Init()
    {
        Connector.Init(this);
        var d = new Vector3(MyExtensions.Random(-1f, 1f),0, MyExtensions.Random(-1f, 1f));
        LookDirection = Utils.NormalizeFastSelf(d);
        LookLeft = Utils.Rotate90(LookDirection, SideTurn.left);
        LookRight = Utils.Rotate90(LookDirection, SideTurn.right);
        base.Init();
    }


    protected override float TurnSpeed()
    {
        return 30f;
    }

    public override float MaxSpeed()
    {
        return 1;
    }

    public void ManualUpdate()
    {
        ApplyMove();
    }
}
