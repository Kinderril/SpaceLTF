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
    public float BankSpeed { get; private set; }
    public const float BANK_SPEED = 0.45f;

    public BankingData(Vector3 dir, float steps)
    {
        BankSpeed = BANK_SPEED;
        TargetDir = dir;
        Steps = steps;
        ImplementedXZ = true;
    }

    public void SetNewData(Vector3 dir, float steps)
    {
        TargetDir = dir;
        Steps = steps;
        ImplementedXZ = false;
        BankSpeed = BANK_SPEED;
    }  

    public void SetNewData(Vector3 dir, float steps,float bankSpeed)
    {
        TargetDir = dir;
        Steps = steps;
        ImplementedXZ = false;
        BankSpeed = bankSpeed;
    }


    public void CompleteXZ()
    {
        ImplementedXZ = true;

    }

}