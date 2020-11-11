using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public interface IWeapon
{
    TargetType TargetType { get; }
    TeamIndex TeamIndex { get; }
    Vector3 CurPosition { get; }
    ShipBase Owner { get; }
    int Level { get; }
    float CurOwnerSpeed { get; }
    CurWeaponDamage CurrentDamage { get; }

    void BulletCreateByDir(ShipBase target, Vector3 dir);
    void DamageDoneCallback(float healthdelta, float shielddelta,ShipBase damageAppliyer);
    void BulletDestroyed(Vector3 position, Bullet bullet);
    void ApplyToShip(ShipParameters shipParameters, ShipBase shipBase, Bullet bullet);

}


