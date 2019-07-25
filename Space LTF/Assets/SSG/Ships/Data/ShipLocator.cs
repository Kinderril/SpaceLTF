using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public  class ShipLocator : ShipData
{
    private float _nextUpdateTime;
    public ShipPersonalInfo DangerEnemy = null;

    public ShipLocator([NotNull] ShipBase owner) : base(owner)
    {

    }

    public void ManualUpdate()
    {
        if (_nextUpdateTime < Time.time)
        {
            _nextUpdateTime = Time.time + 1f;
            Locator();
        }
    }

    private void Locator()
    {
        DangerEnemy = null;
        foreach (var shipPersonalInfo in _owner.Enemies)
        {
            if (shipPersonalInfo.Value.Dist < 12f)
            {
                var dot = Vector3.Dot(shipPersonalInfo.Key.LookDirection, shipPersonalInfo.Value.DirNorm);
                bool isBack = dot < 0;
//                bool isBack = dot < 0;
                if (isBack)
                {
                    var isAng = Utils.IsAngLessNormazied(-shipPersonalInfo.Key.LookDirection, shipPersonalInfo.Value.DirNorm,
                        UtilsCos.COS_50_RAD);
                    if (isAng)
                    {
                        DangerEnemy = shipPersonalInfo.Value;
                    }
                }
            }
        }
    }
}

