using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public class ShieldLocker : ActionModulInGame
{

    public ShieldLocker(BaseModulInv baseModulInv) 
        : base(baseModulInv)
    {
        Period = 15f;
    }
    

    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
        base.Apply(Parameters,owner);
        _owner.WeaponsController.OnWeaponShootStart += OnWeaponShootStart;
    }

    private void OnWeaponShootStart(WeaponInGame obj)
    {
        if (IsReady())
        {
            if (_owner.Target == null)
            {
                Debug.LogError("can't stop zero ship when locking engine");
            }
            else
            {
                UpdateTime();
                var shield = _owner.Target.ShipLink.ShipParameters.ShieldParameters;
                if (shield.ShiledIsActive && shield.CurShiled > 2)
                {
                    _owner.Target.ShipLink.DamageData.ApplyEffect(ShipDamageType.shiled, 2f + ModulData.Level * 3f);
                }
            }
        }
    }

    public override void Dispose()
    {
        _owner.WeaponsController.OnWeaponShootStart -= OnWeaponShootStart;
        base.Dispose();
    }

    public override void Delete()
    {
        _owner.WeaponsController.OnWeaponShootStart -= OnWeaponShootStart;
        base.Delete();
    }
}

