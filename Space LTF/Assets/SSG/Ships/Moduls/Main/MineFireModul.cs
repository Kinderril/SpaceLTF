using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


[System.Serializable]
public class MineFireModul : MineAbstractModul
{
    public const float PERIOD_DAMAGE = 7f;

    public MineFireModul([NotNull] BaseModulInv baseModulInv) : base(baseModulInv)
    {

//        CurrentDamage = new CurWeaponDamage(2 + Level, 3 + Level);
    }

    protected override Bullet GetPrefab()
    {
        return DataBaseController.Instance.GetBullet(WeaponType.fireDamageMine);
    }

    public override void ApplyToShip(ShipParameters shipParameters, ShipBase shipBase, Bullet bullet)
    {
           shipBase.DamageData.ApplyEffect(ShipDamageType.fire, PERIOD_DAMAGE);
    }

    public override void BulletDestroyed(Vector3 position, Bullet bullet)
    {
        Debug.Log($"Fire Mine Destroyd {Time.time}");
    }

}

