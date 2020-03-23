using System;
using UnityEngine;

[Serializable]
public class ShipBattleData
{
    public bool Destroyed;
    //    public int DamageDone;
    public float HealthDamage;
    public float ShieldhDamage;
    public float SelfDamage;
    private float _expCollected = 0;
    public int Kills { get; private set; }

    [field: NonSerialized]
    public event Action<ShipBattleData> OnStatChanged;

    public ShipBattleData()
    {

    }

    public void AddDamage(float healthdelta, float shielddelta, float expCoef)
    {
        HealthDamage += healthdelta;
        ShieldhDamage += shielddelta;
        var exp = (Mathf.Abs(healthdelta) + Mathf.Abs(shielddelta)) * expCoef;
        _expCollected += exp;
        if (OnStatChanged != null)
        {
            OnStatChanged(this);
        }
    }

    public void AddKill()
    {
        _expCollected += Library.KILL_EXP;
        Kills++;
        if (OnStatChanged != null)
        {
            OnStatChanged(this);
        }
    }

    public int GetTotalExp()
    {
        return Mathf.Clamp((int)_expCollected, 1, 999999);
    }

    public void SetDamageCount(float countHealed)
    {
        SelfDamage = countHealed;
    }
}

