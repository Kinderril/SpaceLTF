using JetBrains.Annotations;
using UnityEngine;


public class ShipPeriodDamage : ShipData
{
    private const float DAMAGE_IGNORE_SHIELD = 2f;
    private bool _using = false;
    private float _endTime;
    private float _nextDamageTime;
    private float _coef = 1f;
    private int _ticksRemain;

    public ShipPeriodDamage([NotNull] ShipBase owner)
        : base(owner)
    {

    }

    public void ManualUpdate()
    {
        if (_using)
        {
            if (_ticksRemain <= 0)
            {
                Stop();
                return;
            }
            if (_nextDamageTime < Time.time)
            {
                Damage();
            }
        }
    }

    private void Damage()
    {
        //        Debug.LogError("Damage period :" + 1f + "/" + 1f + "_nextDamageTime:"+ _nextDamageTime);
        _nextDamageTime = Time.time + 1f;
        _ticksRemain--;
        var damage = DAMAGE_IGNORE_SHIELD * _coef;
        _owner.ShipParameters.DamageIgnoreShield(damage, null);
    }

    public int Start(int ticks, float coef)
    {
        if (coef > _coef)
        {
            _coef = coef;
        }
        //        Debug.LogError("Start period damage period:" + period);
        if (_using)
        {
            var nextTicks = _ticksRemain + ticks / 2;
            _ticksRemain = Mathf.Max(nextTicks, ticks);
        }
        else
        {
            _ticksRemain = ticks;
        }

        if (_owner.PeriodDamageEffect != null)
            _owner.PeriodDamageEffect.Play();
        _using = true;
        _nextDamageTime = 0f;
        return _ticksRemain;
    }

    public void Stop()
    {
        _coef = 1f;
        //        Debug.LogError("Stop period damage");
        if (_owner.PeriodDamageEffect != null)
            _owner.PeriodDamageEffect.Stop();
        _using = false;
    }
}

