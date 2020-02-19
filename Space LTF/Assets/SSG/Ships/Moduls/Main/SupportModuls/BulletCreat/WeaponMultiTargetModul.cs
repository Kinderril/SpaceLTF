[System.Serializable]
public class WeaponMultiTargetModul : BaseSupportModul
{
    private const float _decrase_Base = 0.35f;

    public float Decrase => _decrase_Base + 0.11f * Level;
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

    public override string DescSupport()
    {
        return Namings.Format(Namings.Tag("ChainModulDesc"), Utils.FloatToChance(Decrase));
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
