using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class BattleCoinUI :MonoBehaviour
{
    public Image RegenIcon;
    private CommanderCoinController _coinController;
    private List<CoinUI> _coins = new List<CoinUI>();
    public Transform Layout;

    public void Init(CommanderCoinController coinController)
    {
//        RegenIcon.color = Color.blue;
        _coinController = coinController;
//        _coinController.OnRegenEnable += OnRegenEnable;
        var allCoins = _coinController.GetAllCoins();
        foreach (var commandCoin in allCoins)
        {
            CoinUI c = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.CoinPrefab);
            _coins.Add(c);
            c.transform.SetParent(Layout,false);
            c.Init(commandCoin,CoinUISetUsed);
        }

        OnRegenEnable(true);
//        _coinController.OnCoinChange += OnCoinChange;
    }

    private void OnRegenEnable(bool obj)
    {
        RegenIcon.color = obj ? Color.green : Color.red;
    }

    private void CoinUISetUsed(CoinUI obj,bool val)
    {
//        if (val)
//        {
//            obj.transform.SetAsFirstSibling();
//        }
//        else
//        {
//            obj.transform.SetAsLastSibling();
//        }
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

