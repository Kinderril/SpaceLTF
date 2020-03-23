using System.Collections.Generic;


[System.Serializable]
public class RoundStrikeModul : TimerModul
{
    private bool isCharged;
    private const float CloseRad = 4f;

    public RoundStrikeModul(BaseModulInv b)
        : base(b)
    {
        isCharged = true;
        Period = 40f;
    }

    protected override float Delay()
    {
        return Period;
    }


    protected override void TimerAction()
    {
        List<ShipBase> _shipsToDamage = new List<ShipBase>();
        foreach (var shipPersonalInfo in _owner.Enemies)
        {
            if (shipPersonalInfo.Value.Dist < CloseRad)
            {
                _shipsToDamage.Add(shipPersonalInfo.Key);
            }
        }
        if (_shipsToDamage.Count >= 2)
        {
            EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.RoundStrikeEffect,
                _owner.transform, 3f);
            _owner.ShipParameters.Damage(20, 0, DamageDoneCallback, _owner);
            foreach (var shipBase in _shipsToDamage)
            {
                EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.RoundStrikeEffectShip,
                    _owner.transform, 3f);
                shipBase.ShipParameters.Damage(8 + ModulData.Level * 2, 8 + ModulData.Level * 2, DamageDoneCallback, _owner);
            }
        }
    }

    private void DamageDoneCallback(float healthdelta, float shielddelta, ShipBase damageAppliyer)
    {
        var coef = damageAppliyer != null ? damageAppliyer.ExpCoef : 0f;
        _owner.ShipInventory.LastBattleData.AddDamage(healthdelta, shielddelta, coef);
    }
}

