using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeaponRoundWaveStrike
{
    public const float SHIELD_DAMAGE = 3;
    public const float BODY_DAMAGE = 3;
    private const float Rad = 4f;
    private ShipBase _owner;
    private TeamIndex _indexToSearch;
    private CurWeaponDamage weapon;

    public WeaponRoundWaveStrike(ShipBase owner)
    {
        _owner = owner;
        _indexToSearch = BattleController.OppositeIndex(_owner.TeamIndex);
        weapon = new CurWeaponDamage(SHIELD_DAMAGE, BODY_DAMAGE);
    }
    public void Apply()
    {
        EffectController.Instance.Create(DataBaseController.Instance.DataStructPrefabs.WeaponWaveStrike, _owner.transform, 2f);
        var shipsToDamage = BattleController.Instance.GetAllShipsInRadius(_owner.Position, _indexToSearch, Rad);
        foreach (var shipBase in shipsToDamage)
        {
            shipBase.ShipParameters.Damage(weapon.ShieldDamage, weapon.BodyDamage,damageDoneCallback,_owner);
        }
    }

    private void damageDoneCallback(float healthdelta, float shielddelta, ShipBase attacker)
    {
        _owner.ShipInventory.LastBattleData.AddDamage(healthdelta, shielddelta);
    }
}

