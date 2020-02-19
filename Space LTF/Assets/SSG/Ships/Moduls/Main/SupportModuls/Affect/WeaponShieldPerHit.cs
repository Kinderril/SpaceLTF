[System.Serializable]
public class WeaponShieldPerHit : BaseSupportModul
{

    private const float Damage = 0.7f;
    private const int Self = 2;
    private int ShieldToSTeal => Self * Level;
    public WeaponShieldPerHit(int level)
        : base(SimpleModulType.WeaponShieldPerHit, level)
    {
    }


    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.CurrentDamage.BodyDamage -= Damage;
        weapon.CurrentDamage.ShieldDamage -= Damage;
    }

    protected override bool AffectTargetImplement => true;

    protected override WeaponInventoryAffectTarget AffectTarget(WeaponInventoryAffectTarget affections)
    {
        affections.Add(VampireShield);
        return base.AffectTarget(affections);
    }

    private void VampireShield(ShipParameters shipparameters, ShipBase target, Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        bullet.Weapon.Owner.ShipParameters.ShieldParameters.HealShield(ShieldToSTeal);
    }

    public override string DescSupport()
    {
        return Namings.Format(Namings.Tag("WeaponShieldPerHitDesc"), Utils.FloatToChance(Damage),
            ShieldToSTeal);
    }




}
