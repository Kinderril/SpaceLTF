using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class QuestContainerReward 
{

    public WeaponInv WeaponReward { get; private set; }
    public BaseModulInv ModulReward { get; private set; }
    public int MoneyCount { get; private set; }

    public void Init(int targetCounter)
    {
        MoneyCount = targetCounter;

        WDictionary<int> levelsWeapons = new WDictionary<int>(new Dictionary<int, float>()
        {
            {2, 3f},
            {3, 4f},
            {4, 2f},
            {5, 1f},
            {6, 1f},
        });
        WeaponReward = Library.CreateDamageWeapon(levelsWeapons.Random());
        WDictionary<int> levelsModuls = new WDictionary<int>(new Dictionary<int, float>()
        {
            {2, 5f},
            {3, 4f},
            {4, 2f},
            {5, 1f},
        });
        ModulReward = Library.CreatSimpleModul(levelsModuls.Random());



    }

    public void TakeWeapon()
    {
        var inv = MainController.Instance.MainPlayer.Inventory;
        if (inv.GetFreeWeaponSlot(out int slot))
        {
            inv.TryAddWeaponModul(WeaponReward, slot);
        }
    }

    public void TakeModul()
    {
        var inv = MainController.Instance.MainPlayer.Inventory;
        if (inv.GetFreeSimpleSlot(out int slot))
        {
            inv.TryAddSimpleModul(ModulReward, slot);
        }
    }

    public void TakeMoney()
    {
        MainController.Instance.MainPlayer.MoneyData.AddMoney(MoneyCount);
    }
}
