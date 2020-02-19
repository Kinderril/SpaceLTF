using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public  class ShipAttackersData       :ShipData
{
    private ShipPersonalInfo _lastAttacker = null;

    public ShipPersonalInfo CurAttacker => _lastAttacker;
    public ShipAttackersData(ShipBase owner) 
        : base(owner)
    {

    }

    public void UpdateData()
    {
        if (CurAttacker != null)
        {
            if (CurAttacker.IsInBack() && CurAttacker.Dist < 14)
            {
                if (_owner.Boost.IsReady)
                {
                    _owner.Boost.ActivateBack();
                }
            }
            else  if (CurAttacker.IsInFrontSector() && CurAttacker.Dist < 19)
            {
                if (_owner.Boost.IsReady)
                {
                    if (MyExtensions.IsTrueEqual())
                    {
                        _owner.Boost.EvadeToSide();
                    }
                }
            }
        }
    }

    public void ShipStartsAttack(ShipBase attacker)
    {
        if (_owner.Enemies.TryGetValue(attacker, out var info))
        {
            _lastAttacker = info;
        }
    }   

    public void ShipEndsAttack(ShipBase attacker)
    {
        var info = _owner.Enemies[attacker];
        if (_lastAttacker == info)
        {
            _lastAttacker = null;
        }
    }

    public void Remove(ShipPersonalInfo attacker)
    {
        if (_lastAttacker == attacker)
        {
            _lastAttacker = null;
        }
    }
}

