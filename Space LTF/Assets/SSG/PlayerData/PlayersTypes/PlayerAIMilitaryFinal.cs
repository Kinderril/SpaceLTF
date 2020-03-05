using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerAIMilitaryFinal : PlayerAI
{
    public PlayerAIMilitaryFinal(string name, Dictionary<PlayerParameterType, int> startData = null)
        : base(name, startData)
    {

    }

    public override LastReward GetReward(Player winner)
    {
        MainController.Instance.Statistics.AddOpenPoints(5);

        var reward = new LastReward();
        List<ItemType> items = new List<ItemType>() { ItemType.modul, ItemType.spell, ItemType.weapon };
        var rnd = items.RandomElement();
        items.Remove(rnd);
        var power = Army.GetPower();
        // _getRewardsItems = new List<IItemInv>();   
        var countMircochips = power > 25 ? 2 : 1;
        winner.MoneyData.AddMicrochips(countMircochips);
        reward.Microchips = countMircochips;
        int deltaMin, deltaMax;

        foreach (var itemType in items)
        {
            switch (itemType)
            {
                case ItemType.weapon:
                    var weaponLvl = power * 0.1f;
                    deltaMin = Mathf.Clamp((int)(weaponLvl - 1), 2, 5);
                    deltaMax = Mathf.Clamp((int)(weaponLvl + 1), 2, 6);
                    var item = Library.CreateDamageWeapon(MyExtensions.Random(deltaMin, deltaMax));
                    item.CurrentInventory = Inventory;
                    Debug.Log($"exit dungeon army weapon reward weaponLvl:{weaponLvl}  armyPower:{power}");
                    reward.Weapons.Add(item);
                    break;
                case ItemType.modul:
                    var modulLvl = power * 0.06f;
                    deltaMin = Mathf.Clamp((int)(modulLvl - 1), 1, 2);
                    deltaMax = Mathf.Clamp((int)(modulLvl + 1), 1, 3);
                    var ite1m2 = Library.CreatSimpleModul(MyExtensions.Random(deltaMin, deltaMax));
                    ite1m2.CurrentInventory = Inventory;
                    Debug.Log($"exit dungeon army weapon reward modulLvl:{modulLvl}  armyPower:{power}");
                    reward.Moduls.Add(ite1m2);
                    break;
                case ItemType.spell:
                    var spellLvl = power * 0.08f;
                    deltaMin = Mathf.Clamp((int)(spellLvl - 1), 2, 4);
                    deltaMax = Mathf.Clamp((int)(spellLvl + 1), 2, 5);
                    var ite1m = Library.CreateSpell(MyExtensions.Random(deltaMin, deltaMax));
                    ite1m.CurrentInventory = Inventory;
                    Debug.Log($"exit dungeon army weapon reward spell:{spellLvl}  armyPower:{power}");
                    reward.Spells.Add(ite1m);
                    break;
            }
        }
        return reward;
    }
}

