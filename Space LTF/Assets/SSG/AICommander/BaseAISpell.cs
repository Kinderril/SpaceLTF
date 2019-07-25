﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BaseAISpell
{
    private float _delay = 1f;
    private float _nextCheck = 1f;

    protected BaseAISpell()
    {
        _delay = MyExtensions.Random(1f, 1.2f);
    }

    public virtual void Init()
    {

    }

    public virtual void ManualUpdate()
    {
    }

    public void PeriodlUpdate()
    {
        if (_nextCheck < Time.time)
        {
            _nextCheck = Time.time + _delay;
            PeriodInnerUpdate();
        }
    }

    protected virtual void PeriodInnerUpdate()
    {

    }

    public virtual void Dispose()
    {

    }
}

public abstract class BaseAISpell<T> : BaseAISpell  where T : BaseSpellModulInv 
{
    protected Commander _commander;
    protected T _spell;

    protected BaseAISpell(T spell,Commander commander)
    {
        Debug.Log("AI spell controller init:" + spell.GetType());
        _spell = spell;
        _commander = commander;

    }

    protected bool CanCast()
    {
        return _commander.CoinController.CanUseCoins(_spell.CostCount);
    }

    protected void TryUse(Vector3 v)
    {
        _spell.TryCast(_commander.CoinController, v);
    }

    //    protected override void PeriodInnerUpdate()
    //    {
    //        if (_commander.CoinController.CanUseCoins(_spell.CostCount))
    //        {
    //
    //        }
    //    }

}

