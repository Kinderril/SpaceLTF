using System.Collections.Generic;
using UnityEngine;

public class WeaponAimedType
{
    public WeaponInGame[] WeaponsToShoot;
    public WeaponInGame WeaponToAim;
    private List<WeaponInGame> preList = new List<WeaponInGame>();

    public WeaponAimedType(WeaponInGame firstWeapon)
    {
        WeaponToAim = firstWeapon;
        preList.Add(firstWeapon);
    }

    public void TryShoot(ShipPersonalInfo target, Vector3 ownerLokkDir)
    {
        bool subCanFire = false;
        switch (WeaponToAim.TargetType)
        {
            case TargetType.Enemy:
                subCanFire = CheckWeaponAimed(WeaponToAim, target);
                break;

            case TargetType.Ally:
                subCanFire = CheckWeaponAimed(WeaponToAim, target);
                break;
        }

        if (subCanFire)
        {
            for (int i = 0; i < WeaponsToShoot.Length; i++)
            {
                var toShoot = WeaponsToShoot[i];
                toShoot.TryShoot(ownerLokkDir, target.ShipLink);
            }
        }
    }

    private bool CheckWeaponAimed(WeaponInGame weapon, ShipPersonalInfo shipInfo)
    {
        shipInfo.CanShoot = weapon.IsInRadius(shipInfo.Dist);// && weapon.IsInSector(shipInfo.DirNorm);
        if (shipInfo.CanShoot)
        {
            var isAimed = weapon.IsAimed(shipInfo);
            if (isAimed)
            {
                return true;
            }
        }
        return false;
    }

    public void AddWeapon(WeaponInGame weaponInGame)
    {

        preList.Add(weaponInGame);
    }

    public void Cache()
    {
        WeaponsToShoot = preList.ToArray();
        preList = null;

    }
}