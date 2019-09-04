using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponAOEModul : BaseSupportModul
{
    private const float RAD = 5f;
    private const float decrase = 0.75f;
    public WeaponAOEModul(int level)
        : base(SimpleModulType.WeaponAOE, level)
    {
    }

    protected override bool AffectTargetImplement => true;


    protected override WeaponInventoryAffectTarget AffectTarget(WeaponInventoryAffectTarget affections)
    {
        affections.Add(AffectTargetDelegate);
        return base.AffectTarget(affections);
    }
    public override string DescSupport()
    {
        return $"Damage all ships in radius {RAD}. Decrease damage by {Utils.FloatToChance(decrase)}%.";
    }
    protected void AffectTargetDelegate(ShipParameters paramsTargte, ShipBase target, Bullet bullet, DamageDoneDelegate doneDelegate,WeaponAffectionAdditionalParams additional)
    {
        var ships = BattleController.Instance.GetAllShipsInRadius(target.Position,
            BattleController.OppositeIndex(bullet.Weapon.TeamIndex), RAD);

        var dmg = bullet.Weapon.CurrentDamage;
        foreach (var ship in ships)
        {
            if (ship != target)
            {
                ship.ShipParameters.Damage(dmg.ShieldDamage, dmg.BodyDamage, doneDelegate, target);
            }
        }
    }

    public override void ChangeParams(IAffectParameters weapon)
    {

        weapon.CurrentDamage.BodyDamage *= decrase;
        weapon.CurrentDamage.ShieldDamage *= decrase;

    }
}
