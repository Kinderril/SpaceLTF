using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BankingData
{
    public Vector3 TargetDir { get; private set; }
    public float Steps { get; private set; }
    public bool ImplementedXZ { get; private set; }

    public BankingData(Vector3 dir, float steps)
    {
        TargetDir = dir;
        Steps = steps;
        ImplementedXZ = true;
    }

    public void SetNewData(Vector3 dir, float steps)
    {
        TargetDir = dir;
        Steps = steps;
        ImplementedXZ = false;
    }


    public void CompleteXZ()
    {
        ImplementedXZ = true;

    }

}