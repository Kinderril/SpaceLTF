﻿using System.Collections.Generic;
using UnityEngine;
public enum BattleRewardType
{
    money,
    items,
}


public class LastReward
{
    public int Money;
    public List<WeaponInv> Weapons = new List<WeaponInv>();
    public List<BaseModulInv> Moduls = new List<BaseModulInv>();



    public LastReward(Commander enemyCommander, Player winner)
    {
        var power = enemyCommander.StartPower;
        var scoutsLevel = winner.Parameters.Scouts.Level;
        var powerPlayer = winner.Army.GetPower();
        var coef = power * 0.1f;
        var weaponCoef = power * 0.55f;
        WDictionary<int> modulsLevels;
        WDictionary<BattleRewardType> rewardRnd;

        int minWeapons = 1;
        int maxLowItems = MyExtensions.Random(1, 3);
        if (power > 20)
        {
            minWeapons = 0;
            maxLowItems = MyExtensions.Random(1, 2);
        }

        if (power > 26)
        {
            minWeapons = 0;
            maxLowItems = 1;
        }


        var weapDic = new Dictionary<int, float>();
        var modulsDic = new Dictionary<int, float>();
        var itemsCountCoef = (maxLowItems - 1) * 6.5f;


        float l1 = Mathf.Clamp(12 + weaponCoef, 1,999);
        weapDic.Add(1, l1);

        float l2 = 8 + weaponCoef - itemsCountCoef; 
        if (l2 > 0)
            weapDic.Add(2, l2);
        float l3 = 2 + weaponCoef - itemsCountCoef;
        if (l3 > 0)
            weapDic.Add(3, l3);
        float l4 = weaponCoef - 4 - itemsCountCoef;
        if (l4 > 0)
            weapDic.Add(4, l4);
        float l5 = weaponCoef - 8 - itemsCountCoef;
        if (l5 > 0)
            weapDic.Add(5, l5);

        var weaponLevels = new WDictionary<int>(weapDic);

        l1 = Mathf.Clamp(12 + weaponCoef, 1, 999);
        modulsDic.Add(1, l1);
        l2 = 8 + weaponCoef - itemsCountCoef;
        if (l2 > 0)
            modulsDic.Add(2, l2);
        l3 = 2 + weaponCoef - itemsCountCoef;
        if (l3 > 0)
            modulsDic.Add(3, l3);

        modulsLevels = new WDictionary<int>(modulsDic);

        float lowMoneyCoef = 0.7f;
        var weaponsCount = MyExtensions.Random(minWeapons, maxLowItems);
        var modulesCount = maxLowItems - weaponsCount;

        if (powerPlayer < 20)
        {
            lowMoneyCoef = 0.5f;
            rewardRnd =
                new WDictionary<BattleRewardType>(new Dictionary<BattleRewardType, float>()
                {
                    {BattleRewardType.items, 6 + scoutsLevel},
                    {BattleRewardType.money, scoutsLevel},
                });
        }
        else
        {
            rewardRnd =
                new WDictionary<BattleRewardType>(new Dictionary<BattleRewardType, float>()
                {
                    {BattleRewardType.items, scoutsLevel},
                    {BattleRewardType.money, 4 + scoutsLevel/4},
                });
        }

        var reward = rewardRnd.Random();
        int slotIndex;
        float moneyCoef = 1f;
        Debug.Log("Player end battle. Reward setted.  reward:" + reward.ToString());
        switch (reward)
        {
            case BattleRewardType.money:
                moneyCoef = 1;
                break;
            case BattleRewardType.items:
                moneyCoef = lowMoneyCoef;
                for (int i = 0; i < weaponsCount; i++)
                {
                    var w = Library.CreateWeapon(weaponLevels.Random());
                    if (winner.Inventory.GetFreeWeaponSlot(out slotIndex))
                    {
                        winner.Inventory.TryAddWeaponModul(w, slotIndex);
                        Weapons.Add(w);
                    }
                }
                for (int i = 0; i < modulesCount; i++)
                {
                    var m = Library.CreatSimpleModul(modulsLevels.Random());
                    if (winner.Inventory.GetFreeSpellSlot(out slotIndex))
                    {
                        Moduls.Add(m);
                        winner.Inventory.TryAddSimpleModul(m, slotIndex);
                    }
                }
                break;
        }
        int moneyToReward = (int)(moneyCoef * power * Library.BATTLE_REWARD_WIN_MONEY_COEF);
        Money = moneyToReward;
        winner.MoneyData.AddMoney(moneyToReward);
    }
}

