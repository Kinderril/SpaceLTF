using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShieldOffEvent : BattleFieldEvent
{
    private TimerManager.ITimer _battleTimer ;
    private TimerManager.ITimer _affectTimer ;
    private const float MIN_OFF_TIME = 3f;
    private const float MAX_OFF_TIME = 7f;
    private const float VisualEffectPeriod = 1.5f;
    private float _offTime;
    public override EBattlefildEventType Type => EBattlefildEventType.shieldsOff;
    

    public ShieldOffEvent(BattleController battle) : base(battle)
    {

    }

    public override void Init()
    {
        _offTime = MyExtensions.Random(MIN_OFF_TIME, MAX_OFF_TIME);
        _battleTimer = MainController.Instance.BattleTimerManager.MakeTimer(MyExtensions.Random(10, 12), true);
        _battleTimer.OnTimer += OnTimer;
    }

    private void OnTimer()
    {
        CamerasController.Instance.GameCamera.ApplyEMPEffect(VisualEffectPeriod);
        if (_affectTimer != null && _affectTimer.IsActive)
        {
            _affectTimer.Stop();
        }
        _affectTimer = MainController.Instance.BattleTimerManager.MakeTimer(VisualEffectPeriod / 2f);
        _affectTimer.OnTimer += OnTimerAffect;
    }

    private void OnTimerAffect()
    {
        _offTime = MyExtensions.Random(MIN_OFF_TIME, MAX_OFF_TIME);
        DeactivateAllByCommander(_battle.GreenCommander);
        DeactivateAllByCommander(_battle.RedCommander);
    }

    private void DeactivateAllByCommander(Commander commander)
    {
        foreach (var shipsValue in commander.Ships.Values)
        {
            shipsValue.DamageData.ApplyEffect(ShipDamageType.shiled, _offTime);
        }
    }

    public override void Dispose()
    {
        if (_battleTimer != null && _battleTimer.IsActive)
        {
            _battleTimer.Stop();
        }
    }

    public override List<Vector3> GetBlockedPosition()
    {
        return null;
    }
}
