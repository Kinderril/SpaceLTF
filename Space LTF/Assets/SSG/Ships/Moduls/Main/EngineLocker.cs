using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public class EngineLocker : TimerModul
{
    private const float Max_Rad = 10f;
    private BaseEffectAbsorber Effect;
    private ShipBase _trg;
    private bool _active;
    private float _endTime;

    public EngineLocker(BaseModulInv baseModulInv) 
        : base(baseModulInv)
    {
        Effect =DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.EngineLockerEffect);
        Effect.Stop();
        Period = 15f;
    }


    protected override float Delay()
    {
        return Period;
    }

    protected override void TimerAction()
    {

    }

    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
        base.Apply(Parameters,owner);
        Effect.transform.SetParent(_owner.transform);
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
                PlayEffect(_owner.Target.ShipLink, 3f);
                Affect(_owner.Target.ShipLink);
            }
        }
    }

    private void PlayEffect(ShipBase trg, float period)
    {
        _trg = trg;
        _endTime = Time.time + period;
        _active = true;
        Effect.Play(_owner.Position, trg.Position);
    }

    public override void UpdateBattle()
    {
        if (!_active)
        {
            return;
        }

        if (_endTime < Time.time)
        {
            Stop();
        }
        else
        {
            if (!_owner.IsDead && !_trg.IsDead)
            {
                Effect.UpdatePositions(_owner.Position, _trg.Position);
            }
            else
            {
                Stop();
            }
        }
    }

    public void Stop()
    {

        _active = false;
        Effect.Stop();
    }

    private void Affect(ShipBase target)
    {
        if (_owner.Enemies[target].Dist < Max_Rad)
        {
            UpdateTime();
            target.DamageData.ApplyEffect(ShipDamageType.engine, 5f + ModulData.Level * 3f);
        }
    }
    public override void Dispose()
    {
        Stop();
        _owner.WeaponsController.OnWeaponShootStart -= OnWeaponShootStart;
        base.Dispose();
    }

    public override void Delete()
    {
        Stop();
        _owner.WeaponsController.OnWeaponShootStart -= OnWeaponShootStart;
        base.Delete();
    }
}

