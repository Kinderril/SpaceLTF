using System;

public delegate void ShipShootDelegate(ShipBase shooter, ShipBase target);

public static class GlobalEventDispatcher
{
    public static event Action<ShipBase, ShipBase> OnShipDeath;
    public static event Action<ShipBase, float, float, WeaponType> OnShipDamage;
    public static event Action<ShipConfig> OnWinBattle;
    public static event Action<ActionModulInGame> OnSellModul;
    public static event Action<WeaponInv> OnUpgradeWeapon;
    public static event Action<SectorCellContainer> OnCellDataChanged;
    public static ShipShootDelegate OnShipShootDelegate;

    public static void ShipDeath(ShipBase target, ShipBase killer)
    {
        if (OnShipDeath != null && target != null && killer != null)
        {
            OnShipDeath(target, killer);
        }
    }
    public static void SellModul(ActionModulInGame s)
    {
        if (OnSellModul != null)
        {
            OnSellModul(s);
        }
    }
    public static void ShipDamage(ShipBase s, float shield, float body, WeaponType weaponType)
    {
        if (OnShipDamage != null)
        {
            OnShipDamage(s, shield, body, weaponType);
        }
    }

    public static void WinBattle(ShipConfig redCommanderFirstShipConfig)
    {
        if (OnWinBattle != null)
        {
            OnWinBattle(redCommanderFirstShipConfig);
        }
    }

    public static void UpgradeWeapon(WeaponInv weaponInv)
    {
        if (OnUpgradeWeapon != null)
        {
            OnUpgradeWeapon(weaponInv);
        }
    }

    public static void ShipShoot(ShipBase shooter, ShipBase target)
    {
        OnShipShootDelegate?.Invoke(shooter, target);
    }

    public static void CellDataChanged(SectorCellContainer sectorCellContainer)
    {

        OnCellDataChanged?.Invoke(sectorCellContainer);

    }
}

