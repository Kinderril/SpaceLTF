using System;


[System.Serializable]
public class PlayerMoneyData
{
    public int MoneyCount => _links.Credits;
    public int MicrochipsCount => _links.Microchips;
    private PlayerSafe _links;

//    [field: NonSerialized]
//    public event Action<int> OnMoneyChange;
//    [field: NonSerialized]
//    public event Action<int> OnUpgradeChange;

    public PlayerMoneyData(PlayerSafe SafeLinks )
    {
        _links = SafeLinks;
#if UNITY_EDITOR
        //        MoneyCount = 1000;
#endif
    }

    public void AddMoney(int moneyToReward)
    {
        _links.SetMoney(MoneyCount + moneyToReward);
//        if (OnMoneyChange != null)
//        {
//            OnMoneyChange(MoneyCount);
//        }
    }
    public void AddMicrochips(int val)
    {
        _links.SetMicrochips(MicrochipsCount + val);
//        if (OnUpgradeChange != null)
//        {
//            OnUpgradeChange(MoneyCount);
//        }
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
//        OnMoneyChange = null;
    }

    public void RemoveMoney(int costValue)
    {
        _links.SetMoney(MoneyCount - costValue);
//        if (OnMoneyChange != null)
//        {
//            OnMoneyChange(MoneyCount);
//        }
    }
    public void RemoveMicrochips(int val)
    {
        _links.SetMicrochips(MicrochipsCount - val);
//        if (OnUpgradeChange != null)
//        {
//            OnUpgradeChange(MoneyCount);
//        }
    }

}

