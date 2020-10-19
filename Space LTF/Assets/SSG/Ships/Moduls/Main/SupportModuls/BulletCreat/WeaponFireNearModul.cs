
using System;

[Serializable]
public class WeaponFireNearModul : BaseSupportModul
{
    private const float RAD = 3.8f;
    private int SEC = 4;


    public WeaponFireNearModul(int level)
        : base(SimpleModulType.WeaponFireNear, level)
    {
    }
    protected override bool BulletImplement => true;

    private float Sec => SEC + Level * 2;

    protected override CreateBulletDelegate BulletCreate(CreateBulletDelegate standartDelegate)
    {
        // weapon.

        return (target, origin, weapon, pos, parameters) =>
            {
                var ships = BattleController.Instance.GetAllShipsInRadius(pos,
                    BattleController.OppositeIndex(weapon.TeamIndex), RAD);


                foreach (var ship in ships)
                {

                    ship.DamageData.ApplyEffect(ShipDamageType.fire, Sec);

                }
                standartDelegate(target, origin, weapon, pos, parameters);
            };
    }

    public override string DescSupport()
    {
        return Namings.Format(Namings.Tag("FireNearModulDesc"), Sec);
    }


}
