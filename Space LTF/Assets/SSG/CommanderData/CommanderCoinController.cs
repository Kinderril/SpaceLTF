using System;
using UnityEngine;


public class CommanderCoinController
{
    private CommandCoin[] _coins;
    private int availableCount;
    public event Action<CommandCoin, bool> OnCoinChange;
    private ShipBase _controlShip;
    private int _coinsCount;
    private int _levelcharges;
    public float CoefSpeed { get; private set; }

    public void Init(ShipBase controlShip)
    {
        _controlShip = controlShip;
    }
    public CommanderCoinController(int coinsCount, int levelcharges)
    {
        _coinsCount = coinsCount;
        _levelcharges = levelcharges;
        CoefSpeed = 1 - (levelcharges - 1) * Library.CHARGE_SPEED_COEF_PER_LEVEL;
        availableCount = coinsCount;
        _coins = new CommandCoin[coinsCount];
        for (int i = 0; i < coinsCount; i++)
        {
            CommandCoin c1 = new CommandCoin(i + 1, CoefSpeed);
            c1.OnUsed += CoinUsed;
            _coins[i] = (c1);
            c1.EnableRegen(true);
        }

        //        EnableCharge = enableCharge;
        //        DrawRegenEnable();
    }

    private void CoinUsed(CommandCoin arg1, bool val)
    {
        if (val)
        {
            availableCount--;
        }
        else
        {
            availableCount++;
        }
    }

    public CommandCoin[] GetAllCoins()
    {
        return _coins;
    }

    public bool CanUseCoins(int count)
    {
        return availableCount >= count;
    }

    private void UseCoin(CommandCoin c, float delay)
    {
        if (OnCoinChange != null)
        {
            OnCoinChange(c, true);
        }

        delay *= CoefSpeed;
        c.SetUsed(delay);
        //        Debug.Log("UseCoin " + c.Id + " availableCount:" + availableCount);
    }


    public void Dispsoe()
    {
        OnCoinChange = null;
    }

    public void UseCoins(int costCount, int delya)
    {
        //        Debug.LogError("UseCoins costCount:" + costCount + "    _availableCoins.Count:" + availableCount);
        int usedCount = 0;
        if (costCount <= availableCount)
        {
            for (int i = 0; i < _coins.Length; i++)
            {
                var c = _coins[i];
                if (!c.Used)
                {
                    usedCount++;
                    UseCoin(c, delya);
                    if (usedCount == costCount)
                    {
                        return;
                    }
                }
            }
        }
//        Debug.LogError("wrong using coins   costCount:" + costCount + "   availableCount:" + availableCount + "   usedCount:" + usedCount);

    }

    //    public void ChangeRegenEnable()
    //    {
    //        _isRegenEnable = !_isRegenEnable;
    //
    //        EnableRegen(_isRegenEnable);
    //        if (_isRegenEnable)
    //        {
    //            _controlShip.ShipParameters.ShieldParameters.Disable();
    //        }
    //        else
    //        {
    //            _controlShip.ShipParameters.ShieldParameters.Enable();
    //        }
    //        if (OnRegenEnableChange != null)
    //        {
    //            OnRegenEnableChange(_isRegenEnable);
    //        }
    //    }

    //    public bool IsEnable
    //    {
    //        get { return _isRegenEnable; }
    //    }


    //    public void EnableRegen(bool b)
    //    {
    ////        _isRegenEnable = b;
    ////        DrawRegenEnable();
    //        for (int i = 0; i < _coins.Length; i++)
    //        {
    //            var c = _coins[i];
    //            c.EnableRegen(b);
    //        }
    //        if (OnRegenEnable != null)
    //        {
    //            OnRegenEnable(b);
    //        }
    //    }
    // public int NotChargedCoins()
    // {
    //     int c = 0;
    //     foreach (var commandCoin in _coins)
    //     {
    //         if (commandCoin.Used)
    //         {
    //             c++;
    //         }
    //     }
    //
    //     return c;
    // }

    // public void RechargerCoins(int sheildToClear)
    // {
    //     var coinsToRecgarge = _coins.OrderBy(x => x.RemainTime()).ToArray();
    //     var coint = Mathf.Min(sheildToClear, _coins.Length);
    //     for (int i = 0; i < coint; i++)
    //     {
    //         var coin = coinsToRecgarge[i];
    //         coin.Recharge();
    //     }
    // }

    public CommanderCoinController Copy()
    {
        return new CommanderCoinController(_coinsCount, _levelcharges);
    }
}

