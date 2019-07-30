using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CommanderCoinController
{
//    private List<CommandCoin> _availableCoins = new List<CommandCoin>();
    private CommandCoin[] _coins ;
    private int availableCount;
    public event Action<CommandCoin, bool> OnCoinChange;
    public event Action< bool> OnRegenEnable;
    private ShipBase _controlShip;
    private bool _isRegenEnable = true;
    public event Action<bool> OnRegenEnableChange;

    public bool EnableCharge { get; private set; }

    public float CoefSpeed  { get; private set; }
//    private bool _isRegenEnable;

public CommanderCoinController(int coinsCount,bool enableCharge,int levelcharges)
    {
        CoefSpeed = 1 - (levelcharges - 1)* Library.CHARGE_SPEED_COEF_PER_LEVEL;
        availableCount = coinsCount;
        _coins = new CommandCoin[coinsCount];
        for (int i = 0; i < coinsCount; i++)
        {
            CommandCoin c1 = new CommandCoin(i+1,CoefSpeed);
            c1.OnUsed += CoinUsed;
            _coins[i] = (c1);
            c1.EnableRegen(enableCharge);
        }

        EnableCharge = enableCharge;
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
    
    private void UseCoin(CommandCoin c,float delay)
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
        Debug.LogError("wrong using coins   costCount:" + costCount 
            + "   availableCount:" + availableCount + "   usedCount:" + usedCount);

    }

    public void ChangeRegenEnable()
    {
        _isRegenEnable = !_isRegenEnable;

        EnableRegen(_isRegenEnable);
        if (_isRegenEnable)
        {
            _controlShip.ShipParameters.ShieldParameters.Disable();
        }
        else
        {
            _controlShip.ShipParameters.ShieldParameters.Enable();
        }
        if (OnRegenEnableChange != null)
        {
            OnRegenEnableChange(_isRegenEnable);
        }
    }

    public bool IsEnable
    {
        get { return _isRegenEnable; }
    }

    public void Init(ShipBase controlShip)
    {
        _controlShip = controlShip;
    }

    public void EnableRegen(bool b)
    {
//        _isRegenEnable = b;
//        DrawRegenEnable();
        for (int i = 0; i < _coins.Length; i++)
        {
            var c = _coins[i];
            c.EnableRegen(b);
        }
        if (OnRegenEnable != null)
        {
            OnRegenEnable(b);
        }
    }
}

