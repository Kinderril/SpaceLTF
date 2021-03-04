using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CoinTempController
{
    public const float BATTERY_PERIOD = 0.85f;
    public bool ByOneMode = true;
    public TempCoin[] _fillList;
    private int _indexList = 0;
    public float CoefSpeed = 1f;
    private SpellInGame _usingSpell;
    private int _levelcharges;

    public CoinTempController(int batteryCount,int levelcharges)
    {
        _levelcharges = levelcharges;
        CoefSpeed = 1 - (levelcharges - 1) * Library.CHARGE_SPEED_COEF_PER_LEVEL;

        _fillList = new TempCoin[batteryCount];
        for (int i = 0; i < batteryCount; i++)
        {
            var cc = new TempCoin(i);
            _fillList[i] = cc;
            cc.SetSpeedCoef(CoefSpeed);
        }
    }
    public TempCoin GetUsableCoin(SpellInGame spellIn)
    {
        var blocked = _fillList.FirstOrDefault(x => x.State == CointState.Block
                                                    && x.IsNotMax(spellIn.CostPeriod));
        if (blocked != null)
        {
            return blocked;
        }

        blocked = _fillList.FirstOrDefault(x => x.State == CointState.Ready);
        return blocked;
    }

    public void StartUseSpell(SpellInGame spell)
    {
        _usingSpell = spell;
    }

    public void EndCastSpell()
    {
        bool isOneBlock = false;
        for (int i = 0; i < _fillList.Length; i++)
        {
            var coin = _fillList[i];
            if (coin.State == CointState.Block)
            {
                if (!isOneBlock)
                {
                    isOneBlock = true;
                    coin.SetValue(_usingSpell.CostPeriod);
                }
                coin.SetState(CointState.Recharging);
            }
        }
    }

    public bool TrySpellUsage()
    {
        if (_usingSpell == null)
        {
            Debug.LogError($"Using spell is null");
            return false;
        }
        var period = _usingSpell.CostPeriod * Time.deltaTime;
        var blocked = GetUsableCoin(_usingSpell);
        if (blocked != null)
        {
            blocked.AddVal(period, _usingSpell.CostPeriod);
            return true;
        }

        return false;
    }

    public void Update()
    {
        for (int i = 0; i < _fillList.Length; i++)
        {
            var coin = _fillList[i];
            coin.Update();
        }
    }

    public void Dispsoe()
    {

    }

    public IEnumerable<TempCoin> GetAllCoins()
    {
        return _fillList;
    }

    public CoinTempController Copy()
    {
        return new CoinTempController(_fillList.Length, _levelcharges);
    }

    public bool CanStartCast()
    {
        var readyElement = _fillList.FirstOrDefault(x => x.State == CointState.Ready);
        return readyElement != null;
    }
}
