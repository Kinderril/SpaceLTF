using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class PilotMoneyData
{
    public int Money { get; private set; }
//    public int CurLevel { get; private set; }

    public PilotMoneyData()
    {
//        CurLevel = 1;
        Money = 0;
    }

    public void AddMoney(int money)
    {
        Money += money;
    }

    public void RemoveMoney(int money)
    {
        Money -= money;
    }

    public bool CanLvlUp(int parameterLvl,int additionalMoney = 0)
    {
        return Money + additionalMoney >= Library.PilotLvlUpCost(parameterLvl);
    }

    public bool PayLvlUp(int parameterLvl)
    {
        if (CanLvlUp(parameterLvl))
        {
            Money = Money - Library.PilotLvlUpCost(parameterLvl);
            return true;
        }
        return false;
    }
    
}

