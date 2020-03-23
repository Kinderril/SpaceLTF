public class DamageHelper
{

    public static void StandartDamageDoneCallback(ShipBase Owner, float healthdelta, float shielddelta, ShipBase damageAppliyer)
    {
        var coef = damageAppliyer != null ? damageAppliyer.ExpCoef : 0f;
        Owner.ShipInventory.LastBattleData.AddDamage(healthdelta, shielddelta, coef);
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
