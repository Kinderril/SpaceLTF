using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum TimeEffectType
{
    disableModul,
//    engineDestoy,
    weaponOff,
}

public abstract class BaseTimeEffect
{
    private TimerManager.ITimer _timer;
    protected ShipBase _shipToApply;

    public static BaseTimeEffect Create(ShipBase ship,TimeEffectType type)
    {
        switch (type)
        {
            case TimeEffectType.disableModul:
                return new DisableModulEffect(ship,10);           
//            case TimeEffectType.engineDestoy:
//                return new StopShipEffect(ship, 5);
            case TimeEffectType.weaponOff:
                return new WeaponOffEffect(ship, 8);
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }

    public BaseTimeEffect(ShipBase shipToApply, float deltaTimeSec)
    {
        _shipToApply = shipToApply;
        _shipToApply.OnDispose += OnDispose;
        _timer = MainController.Instance.TimerManager.MakeTimer(deltaTimeSec);
        _timer.OnTimer += DisApply;
        Apply();
    }

    private void OnDispose(ShipBase obj)
    {
        Dispsoe();
    }

    private void Dispsoe()
    {
        _shipToApply.OnDispose -= OnDispose;
        if (_timer != null && _timer.IsActive)
        {
            _timer.Stop();
        }
    }

    protected abstract void Apply();

    protected virtual void DisApply()
    {
        Dispsoe();
    }
}

