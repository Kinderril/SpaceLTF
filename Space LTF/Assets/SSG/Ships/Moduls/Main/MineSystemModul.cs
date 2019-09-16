using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


[System.Serializable]
public class MineSystemModul : MineAbstractModul
{
    public MineSystemModul([NotNull] BaseModulInv baseModulInv) : base(baseModulInv)
    {

    }

    protected override Bullet GetPrefab()
    {
        return DataBaseController.Instance.GetBullet(WeaponType.staticSystemMine);
    }

    public override void ApplyToShip(ShipParameters shipParameters, ShipBase shipBase, Bullet bullet)
    {
        if (MyExtensions.IsTrue01(.7f))
        {
            shipBase.EngineStop.Stop(5f + Level * 2);
        }
    }

    public override void BulletDestroyed(Vector3 position, Bullet bullet)
    {
                 Debug.Log($"Mine system Destroyd {Time.time}");

    }

}

