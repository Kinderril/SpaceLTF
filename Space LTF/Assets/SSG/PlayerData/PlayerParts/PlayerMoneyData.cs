using System;


[System.Serializable]
public class PlayerMoneyData
{
    public int MoneyCount { get; private set; }
    public int MicrochipsCount { get; private set; }

    [field: NonSerialized]
    public event Action<int> OnMoneyChange;
    [field: NonSerialized]
    public event Action<int> OnUpgradeChange;

    public PlayerMoneyData()
    {
#if UNITY_EDITOR
        //        MoneyCount = 1000;
#endif
    }

    public void AddMoney(int moneyToReward)
    {
        MoneyCount += moneyToReward;
        if (OnMoneyChange != null)
        {
            OnMoneyChange(MoneyCount);
        }
    }
    public void AddMicrochips(int val)
    {
        MicrochipsCount += val;
        if (OnUpgradeChange != null)
        {
            OnUpgradeChange(MoneyCount);
        }
    }

    public bool HaveMicrochips(int costValue)
    {
        return MicrochipsCount >= costValue;
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
    public void RemoveMicrochips(int val)
    {
        MicrochipsCount -= val;
        if (OnUpgradeChange != null)
        {
            OnUpgradeChange(MoneyCount);
        }
    }

}

