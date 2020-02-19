using System;
using UnityEngine;


public class ShipControlCenter : ShipBase
{

    private Vector3? CurTarget;
    private float _nextClosePoint;
    private float dist2flow = 2f;
    private Vector3 StartPos;
    public TurretConnector Connector;
    public CommanderCoinController CoinController { get; private set; }

    public override void Init(TeamIndex teamIndex, ShipInventory shipInventory, ShipBornPosition pos, IPilotParameters pilotParams,
        Commander commander, Action<ShipBase> dealthCallback)
    {
        base.Init(teamIndex, shipInventory, pos, pilotParams, commander, dealthCallback);
        Connector.Init(this);
    }

    protected override void DesicionDataInit()
    {
        DesicionData = new ControlCenterDesicionData(this);
    }



    protected override void UpdateAction()
    {
        UpdateShieldRegen();
        if (CurAction != null)
        {
            if (CurAction.ShallEndByTime())
            {
                CurAction.EndAction("By time");
                return;
            }

            CurAction.ManualUpdate();
            if (CurAction != null)
            {
                CurAction.ShallEndUpdate2();
            }
            EngineUpdate();
            ApplyMove();
        }
    }

    public override Vector3 PredictionPos()
    {
        return Position;
    }

    protected override void GizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(StartPos, dist2flow);
    }



    public void SetCoinController(CommanderCoinController coinController)
    {
        CoinController = coinController;
        CoinController.Init(this);
    }

    public override void Dispose()
    {
        if (CoinController != null)
        {
            CoinController.Dispsoe();
        }
        base.Dispose();
    }
}

