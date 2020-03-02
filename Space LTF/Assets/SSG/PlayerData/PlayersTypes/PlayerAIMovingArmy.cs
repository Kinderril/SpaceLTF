using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerAIMovingArmy : PlayerAI
{
    public PlayerAIMovingArmy(string name, Dictionary<PlayerParameterType, int> startData = null)
        : base(name, startData)
    {

    }

    public override LastReward GetReward(Player winner)
    {
        MainController.Instance.Statistics.AddOpenPoints(3);
        var reward = new LastReward();
        var armyPower = Army.GetPower();
        List<ItemType> items = new List<ItemType>() { ItemType.modul, ItemType.spell, ItemType.weapon };
        var rnd = items.RandomElement();
        items.Remove(rnd);
        // var _getRewardsItems = new List<IItemInv>();
        int deltaMin, deltaMax;

        foreach (var itemType in items)
        {
            switch (itemType)
            {
                case ItemType.weapon:
                    var weaponLvl = armyPower * 0.1f;
                    deltaMin = Mathf.Clamp((int)(weaponLvl - 1), 2, 5);
                    deltaMax = Mathf.Clamp((int)(weaponLvl + 1), 2, 6);
                    var item = Library.CreateDamageWeapon(MyExtensions.Random(deltaMin, deltaMax));
                    item.CurrentInventory = Inventory;
                    Debug.Log($"moving army weapon reward weaponLvl:{weaponLvl}  armyPower:{armyPower}");
                    // _getRewardsItems.Add(item);
                    reward.Weapons.Add(item);
                    break;
                case ItemType.modul:
                    var modulLvl = armyPower * 0.06f;
                    deltaMin = Mathf.Clamp((int)(modulLvl - 1), 1, 2);
                    deltaMax = Mathf.Clamp((int)(modulLvl + 1), 1, 3);
                    var ite1m2 = Library.CreatSimpleModul(MyExtensions.Random(deltaMin, deltaMax));
                    ite1m2.CurrentInventory = Inventory;
                    Debug.Log($"moving army weapon reward modulLvl:{modulLvl}  armyPower:{armyPower}");
                    // _getRewardsItems.Add(ite1m2);
                    reward.Moduls.Add(ite1m2);
                    break;
                case ItemType.spell:
                    var spellLvl = armyPower * 0.08f;
                    deltaMin = Mathf.Clamp((int)(spellLvl - 1), 2, 4);
                    deltaMax = Mathf.Clamp((int)(spellLvl + 1), 2, 5);
                    var ite1m = Library.CreateSpell(MyExtensions.Random(deltaMin, deltaMax));
                    ite1m.CurrentInventory = Inventory;
                    Debug.Log($"moving army weapon reward spell:{spellLvl}  armyPower:{armyPower}");
                    // _getRewardsItems.Add(ite1m);
                    reward.Spells.Add(ite1m);
                    break;
            }
        }
        return reward;
    }
}

