using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[System.Serializable]
public class PlayerMoneyData
{
//    private Player _player;
    public int MoneyCount = 10;

    [field: NonSerialized]
    public event Action<int> OnMoneyChange;

    public PlayerMoneyData()
    {
#if UNITY_EDITOR
//        MoneyCount = 1000;
#endif
        //        _player = player;
    }

    public void AddMoney(int moneyToReward)
    {
        MoneyCount += moneyToReward;
        if (OnMoneyChange != null)
        {
            OnMoneyChange(MoneyCount);
        }
    }

    public bool HaveMoney(int costValue)
    {
        return MoneyCount >= costValue;
    }

    public void Dispose()
    {
        OnMoneyChange = null;
    }

    public void RemoveMoney(int costValue)
    {
        MoneyCount -= costValue;
        if (OnMoneyChange != null)
        {
            OnMoneyChange(MoneyCount);
        }
    }

}

