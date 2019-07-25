using UnityEngine;
using System.Collections;

public class WeaponUIParams : IAffectParameters
{
    public WeaponUIParams(CurWeaponDamage dmg, float AimRadius, float SetorAngle, float BulletSpeed, float ReloadSec)
    {
        this.CurrentDamage = dmg;
        this.AimRadius = AimRadius;
        this.SetorAngle = SetorAngle;
        this.BulletSpeed = BulletSpeed;
        this.ReloadSec = ReloadSec;
    }

    public CurWeaponDamage CurrentDamage { get; }
    public float AimRadius { get; set; }
    public float SetorAngle { get; set; }
    public float BulletSpeed { get; set; }
    public float ReloadSec { get; set; }
}
