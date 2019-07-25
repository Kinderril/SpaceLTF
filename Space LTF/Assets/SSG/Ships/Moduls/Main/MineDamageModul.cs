using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


[System.Serializable]
public class MineDamageModul : MineAbstractModul
{
    public MineDamageModul([NotNull] BaseModulInv baseModulInv) : base(baseModulInv)
    {

        CurrentDamage = new CurWeaponDamage(2 + Level, 3 + Level);
    }

    protected override Bullet GetPrefab()
    {
        return DataBaseController.Instance.GetBullet(WeaponType.staticDamageMine);
    }

    public override void ApplyToShip(ShipParameters shipParameters, ShipBase shipBase, Bullet bullet)
    {

        shipParameters.Damage(CurrentDamage.ShieldDamage, CurrentDamage.BodyDamage, DamageDoneCallback, shipBase);
    }

    public override void BulletDestroyed(Vector3 position, Bullet bullet)
    {

        Debug.Log($"Mine damage Destroyd {Time.time}");
    }

}

