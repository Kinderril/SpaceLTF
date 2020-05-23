using JetBrains.Annotations;
using UnityEngine;


public class ShipPeriodDamage : ShipData
{
    private const float DAMAGE_IGNORE_SHIELD = 2f;
    private const float CONSTANT_FIRE_PERIOD = 0.5f;
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
        UpdateConstantFire();
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

    private float _nextConstantFire;

    private void UpdateConstantFire()
    {
        if (_constantFire)
        {
            if (_nextConstantFire < Time.time)
            {
                _nextConstantFire = Time.time + CONSTANT_FIRE_PERIOD;
                var damage = DAMAGE_IGNORE_SHIELD * _constantPowerCoef;
                _owner.ShipParameters.DamageIgnoreShield(damage, null);

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
        if (!_constantFire)
        {
            if (_owner.PeriodDamageEffect != null)
                _owner.PeriodDamageEffect.Stop();
        }
        _using = false;
    }

    private bool _constantFire;
    private float _constantPowerCoef;

    public void StartConstantFire(float f)
    {
        Debug.LogError($"StartConstantFire");
        if (_owner.PeriodDamageEffect != null)
            _owner.PeriodDamageEffect.Play();
        _constantFire = true;
        _constantPowerCoef = f;
    }

    public void StopConstantFire(float f)
    {
        Debug.LogError($"StopConstantFire");
        if (!_using)
        {
            if (_owner.PeriodDamageEffect != null)
                _owner.PeriodDamageEffect.Stop();
        }
        _constantFire = false;
    }
}

