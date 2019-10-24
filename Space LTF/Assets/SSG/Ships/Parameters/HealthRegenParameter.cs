using UnityEngine;
using System.Collections;

public class HealthRegenParameter
{

    public float _healPerTick;

    private float _nextHealTime;
    public float _remainToHeal;
    private ShipParameters _ship;
    public HealthRegenParameter(ShipParameters ship)
    {
        _ship = ship;
    }

    public void Start(float toHeal, float perTick)
    {
        _remainToHeal = _remainToHeal + toHeal;
        _healPerTick = perTick;
        _nextHealTime = Time.time;
    }

    public void Update()
    {
        if (_remainToHeal > 0)
        {
            if (_nextHealTime < Time.time)
            {
                _nextHealTime = Time.time + 1f;
                if (_remainToHeal < _healPerTick)
                {
                    _ship.HealHp(_remainToHeal);
                    _remainToHeal = 0;
                }
                else
                {
                    _ship.HealHp(_healPerTick);
                    _remainToHeal = _remainToHeal - _healPerTick;
                }

            }
        }
    }
}
