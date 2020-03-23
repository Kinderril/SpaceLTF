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

    public bool TryShoot(IShipData target, Vector3 ownerLokkDir)
    {
        bool subCanFire = false;
        WeaponToAim.DropAimPos();
        switch (WeaponToAim.TargetType)
        {
            case TargetType.Enemy:
                subCanFire = CheckWeaponAimed(WeaponToAim, target);
                break;

            case TargetType.Ally:
                subCanFire = CheckWeaponAimed(WeaponToAim, target);
                break;
        }
        // WeaponToAim.
        bool someWeaponSHoot = false;
        if (subCanFire)
        {
            for (int i = 0; i < WeaponsToShoot.Length; i++)
            {
                var toShoot = WeaponsToShoot[i];
                var isDone = toShoot.TryShoot(ownerLokkDir, target.ShipLink);
                if (isDone)
                {
                    someWeaponSHoot = true;
                }
            }
        }

        return someWeaponSHoot;
    }

    private bool CheckWeaponAimed(WeaponInGame weapon, IShipData shipInfo)
    {
        var isInRadius = weapon.IsInRadius(shipInfo.Dist);// && weapon.IsInSector(shipInfo.DirNorm);
        if (isInRadius)
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
        for (int i = 0; i < WeaponsToShoot.Length; i++)
        {
            if (i > 0)
            {
                WeaponsToShoot[i].DisableSound();
            }
        }
        preList = null;

    }
}