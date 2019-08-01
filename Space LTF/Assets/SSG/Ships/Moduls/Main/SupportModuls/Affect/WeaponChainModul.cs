using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponChainModul : BaseSupportModul
{

    private const float SEARCH_RAD = 4f;
    private const float SEARCH_PER_LVL = 3f;
    private const float decrase = 0.75f;
    public WeaponChainModul(int level)
        : base(SimpleModulType.WeaponChain, level)
    {
    }

    protected override bool AffectTargetImplement
    {
        get { return true; }
    }

    protected void AffectTargetDelegate(ShipParameters paramsTargte, ShipBase ship, Bullet bullet, DamageDoneDelegate doneDelegate, WeaponAffectionAdditionalParams additional)
    {
        var shipsInRad = BattleController.Instance.GetAllShipsInRadius(ship.Position,
            BattleController.OppositeIndex(bullet.Weapon.TeamIndex), SEARCH_RAD + Level* SEARCH_PER_LVL);
        shipsInRad.Remove(ship);
        if (shipsInRad.Count > 0 && shipsInRad != null)
        {
            var rndSHip = shipsInRad.RandomElement();
            if (rndSHip != null)
            {                             
                bullet.Weapon.BulletCreateByDir(rndSHip,ship.LookDirection);
            }
        }
    }
    public override string DescSupport()
    {
        return ($"After hit can reflect to another target. Decrease damage by {Utils.FloatToChance(decrase)}.");
    }

    protected override WeaponInventoryAffectTarget AffectTarget(WeaponInventoryAffectTarget affections)
    {
        affections.Add(AffectTargetDelegate);
        return base.AffectTarget(affections);
    }
    public override void ChangeParams(IAffectParameters weapon)
    {
      
        weapon.CurrentDamage.BodyDamage *= decrase;
        weapon.CurrentDamage.ShieldDamage *= decrase;
        
    }
}
