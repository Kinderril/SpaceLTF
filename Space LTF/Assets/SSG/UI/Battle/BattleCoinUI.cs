using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class BattleCoinUI :MonoBehaviour
{
    private CoinTempController _coinController;
    private List<CoinUI> _coins = new List<CoinUI>();
    public Transform Layout;

    public void Init(CoinTempController coinController)
    {
        _coinController = coinController;
        var allCoins = _coinController.GetAllCoins();
        foreach (var commandCoin in allCoins)
        {
            CoinUI c = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.CoinPrefab);
            _coins.Add(c);
            c.transform.SetParent(Layout,false);
            c.Init(commandCoin);
        }
    }
    
    public void Dispose()
    {
//        _coinController.OnRegenEnable -= OnRegenEnable;
        foreach (var coinUi in _coins)
        {
            coinUi.Dispose();
            GameObject.Destroy(coinUi.gameObject);
        }
        _coins.Clear();
    }
}

