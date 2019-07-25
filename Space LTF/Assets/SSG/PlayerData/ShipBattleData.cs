using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class ShipBattleData
{
    public bool Destroyed;
//    public int DamageDone;
    public float HealthDamage;
    public float ShieldhDamage;
    public float SelfDamage;
    public int Kills { get; private set; }

    [field: NonSerialized]
    public event Action<ShipBattleData> OnStatChanged;

    public ShipBattleData()
    {

    }

    public void AddDamage(float healthdelta, float shielddelta)
    {
        HealthDamage += healthdelta;
        ShieldhDamage += shielddelta;
        if (OnStatChanged != null)
        {
            OnStatChanged(this);
        }
    }

    public void AddKill()
    {
        Kills++;
        if (OnStatChanged != null)
        {
            OnStatChanged(this);
        }
    }

    public float GetTotalExp()
    {
        return 1 + HealthDamage + ShieldhDamage + Kills*5f;
    }

    public void SetDamageCount(float countHealed)
    {
        SelfDamage = countHealed;
    }
}

