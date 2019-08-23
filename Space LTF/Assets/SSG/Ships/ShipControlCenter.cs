using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;


public class ShipControlCenter : ShipBase
{
    private Vector3? CurTarget;
    private float _nextClosePoint;
    private float dist2flow = 2f;
//    private List<Vector3> listOfDirs = new List<Vector3>();
    private Vector3 StartPos;

    protected override void DesicionDataInit()
    {
        DesicionData = new ControlCenterDesicionData(this);
    }

//    protected override void EngineUpdate()
//    {
//        _curSpeed = 0f;
//    }

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

            if (CurAction != null)
            {
                CurAction.ShallEndUpdate2();
            }
            CurAction.ManualUpdate();
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
        Gizmos.DrawWireSphere(StartPos,dist2flow);
    }
}

