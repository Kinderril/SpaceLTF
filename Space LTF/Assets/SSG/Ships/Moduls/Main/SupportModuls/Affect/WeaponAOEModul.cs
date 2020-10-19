
using System;

[Serializable]
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
        return Namings.Format(Namings.Tag(Type.ToString()), RAD,
            Utils.FloatToChance(decrase));
    }
    protected void AffectTargetDelegate(ShipParameters paramsTargte, ShipBase target,
        Bullet bullet, DamageDoneDelegate doneDelegate, WeaponAffectionAdditionalParams additional)
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

    protected void subAffectTargetDelegate(ShipParameters paramsTargte, ShipBase target,
        Bullet bullet, DamageDoneDelegate doneDelegate, WeaponAffectionAdditionalParams additional, bool damage)
    {
        var index = damage ? BattleController.OppositeIndex(bullet.Weapon.TeamIndex) : bullet.Weapon.TeamIndex;
        var ships = BattleController.Instance.GetAllShipsInRadius(target.Position, index, RAD);

        var dmg = bullet.Weapon.CurrentDamage;
        foreach (var ship in ships)
        {
            if (ship != target)
            {
                if (damage)
                {
                    ship.ShipParameters.Damage(dmg.ShieldDamage, dmg.BodyDamage, doneDelegate, target);
                }
                else
                {
                    if (MyExtensions.IsTrueEqual())
                    {
                        ship.ShipParameters.HealHp(dmg.BodyDamage);
                    }
                    else
                    {
                        ship.ShipParameters.ShieldParameters.HealShield(dmg.ShieldDamage);
                    }
                }
            }
        }
    }

    public override void ChangeParams(IAffectParameters weapon)
    {

        weapon.CurrentDamage.BodyDamage *= decrase;
        weapon.CurrentDamage.ShieldDamage *= decrase;

    }
}
