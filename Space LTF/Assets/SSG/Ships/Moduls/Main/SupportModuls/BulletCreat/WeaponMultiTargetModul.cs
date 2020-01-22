using System;
using UnityEngine;

[System.Serializable]
public class WeaponMultiTargetModul : BaseSupportModul
{
    private const float _decrase_Base = 0.4f;

    public float Decrase => _decrase_Base + 0.14f * Level;
    public WeaponMultiTargetModul(int level)
        : base(SimpleModulType.WeaponMultiTarget, level)
    {
    }

    // protected override bool AffectTargetImplement => true;
    protected override bool BulletImplement => true;

    protected override CreateBulletDelegate BulletCreate(CreateBulletDelegate standartDelegate)
    {
        return (target, origin, weapon, shootpos, bullestartparameters) =>
        {
            var shipsInRad = BattleController.Instance.GetAllShipsInRadius(weapon.CurPosition,
                BattleController.OppositeIndex(weapon.TeamIndex), bullestartparameters.radiusShoot);
            if (shipsInRad.Count > 0)
            {
                foreach (var shipBase in shipsInRad)
                {
                    var dir = Utils.NormalizeFastSelf(shipBase.Position - shootpos);
                    BulletTarget trg;
                    if (target.target != null)
                    {
                        trg = new BulletTarget(shipBase);
                    }
                    else
                    {
                        trg = new BulletTarget(dir + shootpos);
                    }

                    standartDelegate(trg, origin, weapon, shootpos, bullestartparameters);
                }
            }
        };
    }

    private void CreateBullets(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var shipsInRad = BattleController.Instance.GetAllShipsInRadius(weapon.CurPosition,
            BattleController.OppositeIndex(weapon.TeamIndex), bullestartparameters.radiusShoot);
        if (shipsInRad.Count > 0)
        {
            foreach (var shipBase in shipsInRad)
            {
                var dir = Utils.NormalizeFastSelf(shipBase.Position - shootpos);
                BulletTarget trg;
                if (target.target != null)
                {
                    trg = new BulletTarget(shipBase);
                }
                else
                {
                    trg = new BulletTarget(dir + shootpos);
                }
                
                var b1 = Bullet.Create(origin, weapon, dir, shootpos, trg.target, bullestartparameters);
            }
        }
    }


    // protected void AffectTargetDelegate(ShipParameters paramsTargte, ShipBase ship, Bullet bullet, DamageDoneDelegate doneDelegate, WeaponAffectionAdditionalParams additional)
    // {
    //     var shipsInRad = BattleController.Instance.GetAllShipsInRadius(ship.Position,
    //         BattleController.OppositeIndex(bullet.Weapon.TeamIndex), SEARCH_RAD + Level * SEARCH_PER_LVL);
    //     shipsInRad.Remove(ship);
    //     if (shipsInRad.Count > 0 && shipsInRad != null)
    //     {
    //         var rndSHip = shipsInRad.RandomElement();
    //         if (rndSHip != null)
    //         {
    //             bullet.Weapon.BulletCreateByDir(rndSHip, ship.LookDirection);
    //         }
    //     }
    // }
    public override string DescSupport()
    {
        return String.Format(Namings.Tag("ChainModulDesc"), Utils.FloatToChance(Decrase));
        // return ($"After hit can reflect to another target. Decrease damage by {Utils.FloatToChance(Decrase)}.");
    }

    // protected override WeaponInventoryAffectTarget AffectTarget(WeaponInventoryAffectTarget affections)
    // {
    //     affections.Add(AffectTargetDelegate);
    //     return base.AffectTarget(affections);
    // }
    public override void ChangeParams(IAffectParameters weapon)
    {

        weapon.CurrentDamage.BodyDamage *= Decrase;
        weapon.CurrentDamage.ShieldDamage *= Decrase;

    }
}
