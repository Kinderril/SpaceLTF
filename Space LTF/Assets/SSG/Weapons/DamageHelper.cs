using UnityEngine;
using System.Collections;

public class DamageHelper 
{

    public static void StandartDamageDoneCallback(ShipBase Owner, float healthdelta, float shielddelta, ShipBase damageAppliyer)
    {
//        GlobalEventDispatcher.ShipDamage(Owner, healthdelta, shielddelta, weaponType);
        Owner.ShipInventory.LastBattleData.AddDamage(healthdelta, shielddelta);
        if (damageAppliyer != null)
        {
            //#if UNITY_EDITOR
            //            if (damageAppliyer.IsDead == Owner)
            //                Debug.LogError(
            //                    $"Strange things. I wanna kill my self??? {Owner.Id}_{Owner.name}  side:{Owner.TeamIndex}  weap:{Name}");
            //#endif
            if (damageAppliyer.IsDead)
            {
                GlobalEventDispatcher.ShipDeath(damageAppliyer, Owner);
                Owner.ShipInventory.LastBattleData.AddKill();
            }
        }
    }
}
