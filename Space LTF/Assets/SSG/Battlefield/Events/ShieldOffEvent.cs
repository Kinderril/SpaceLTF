using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShieldOffEvent : BattleFieldEvent
{
    private TimerManager.ITimer _battleTimer ;
    private const float MIN_OFF_TIME = 3f;
    private const float MAX_OFF_TIME = 7f;
    private float _offTime;
    public override BattlefildEventType Type => BattlefildEventType.shieldsOff;
    

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
//        var center = _battle.CellController.Data.CenterZone;
//        var rad = _battle.CellController.Data.Radius;
//        var xx = MyExtensions.Random(-rad, rad);
//        var zz = MyExtensions.Random(-rad, rad);
//        var pos = center + new Vector3(xx, 0, zz);

//        EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.BattlefieldEMIEffect, pos, 5f);
        CamerasController.Instance.GameCamera.ApplyEMPEffect();

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
}
