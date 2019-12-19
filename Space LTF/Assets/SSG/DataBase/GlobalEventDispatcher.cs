using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class GlobalEventDispatcher
{
    public static event Action<ShipBase, ShipBase> OnShipDeath;
    public static event Action<ShipBase,float, float, WeaponType> OnShipDamage;
    public static event Action<ShipConfig> OnWinBattle;
    public static event Action<BaseModul> OnSellModul;
    public static event Action<WeaponInv> OnUpgradeWeapon;

    public static void ShipDeath(ShipBase target,ShipBase killer)
    {
        if (OnShipDeath != null && target != null && killer != null)
        {
            OnShipDeath(target, killer);
        }
    }    
    public static void SellModul(BaseModul s)
    {
        if (OnSellModul != null)
        {
            OnSellModul(s);
        }
    }
    public static void ShipDamage(ShipBase s,float shield,float body,WeaponType weaponType)
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
}

