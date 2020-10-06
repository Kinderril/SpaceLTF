using System;
using System.Collections.Generic;

[Serializable]
public class QuestContainerReward 
{

    public WeaponInv WeaponReward { get; private set; }
    public BaseModulInv ModulReward { get; private set; }
    public int MoneyCount { get; private set; }

    public void Init(int targetCounter)
    {
        var player = MainController.Instance.MainPlayer;
        MoneyCount = (int)(targetCounter * player.SafeLinks.CreditsCoef);

        var levelsOfPower = new List<int>();
        var levelsOfPowerModuls = new List<int>();
        if (targetCounter < 20)
        {
            levelsOfPower.Add(1);
            levelsOfPower.Add(2);
            levelsOfPowerModuls.Add(1);
        }
        else if (targetCounter < 25)
        {
            levelsOfPower.Add(1);
            levelsOfPower.Add(2);
            levelsOfPower.Add(3);
            levelsOfPowerModuls.Add(1);
            levelsOfPowerModuls.Add(2);
        }
        else if (targetCounter < 30)
        {
            levelsOfPower.Add(2);
            levelsOfPower.Add(3);
            levelsOfPower.Add(4);
            levelsOfPowerModuls.Add(3);
            levelsOfPowerModuls.Add(2);
        }
        else if (targetCounter < 35)
        {
            levelsOfPower.Add(3);
            levelsOfPower.Add(4);
            levelsOfPower.Add(5);
            levelsOfPowerModuls.Add(3);
        }
        else
        {
            levelsOfPower.Add(4);
            levelsOfPower.Add(5);
            levelsOfPower.Add(6);
            levelsOfPowerModuls.Add(3);
            levelsOfPowerModuls.Add(4);
        }

        WeaponReward = Library.CreateDamageWeapon(levelsOfPower.RandomElement());
        ModulReward = Library.CreatSimpleModul(levelsOfPowerModuls.RandomElement());
    }

    public void TakeRandom()
    {
        List<Action> allAct = new List<Action>()
        {
            TakeModul,
            TakeMoney,
            TakeWeapon,
        };
        var reult = allAct.RandomElement();
        reult();
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
