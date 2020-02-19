[System.Serializable]
public class WeaponSelfDamageModul : BaseSupportModul
{

    private const int Damage = 2;
    private const int _self = 2;
    public WeaponSelfDamageModul(int level)
        : base(SimpleModulType.WeaponSelfDamage, level)
    {
    }

    private int Self => _self + Level * 1;
    private int Dmg => Damage + Level * 2;

    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.CurrentDamage.BodyDamage += Dmg;
        weapon.CurrentDamage.ShieldDamage += Dmg;
    }

    protected override bool BulletImplement => true;

    protected override CreateBulletDelegate BulletCreate(CreateBulletDelegate standartDelegate)
    {
        CreateBulletDelegate delegat1 = (target, origin, weapon, pos, parameters) =>
            {
                weapon.Owner.ShipParameters.Damage(Self, Self, (delta, shieldDelta, attacker) => { }, null);
                standartDelegate(target, origin, weapon, pos, parameters);
            };

        return delegat1;
    }

    public override string DescSupport()
    {
        return Namings.Format(Namings.Tag("WeaponSelfDamage"), Dmg, Self);
    }




}
