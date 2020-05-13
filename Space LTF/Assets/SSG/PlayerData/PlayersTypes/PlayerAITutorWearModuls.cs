using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class PlayerAITutorWearModuls : PlayerAI
{
    public PlayerAITutorWearModuls(string name, Dictionary<PlayerParameterType, int> startData = null)
        : base(name, startData)
    {

    }

    public override LastReward GetReward(Player winner)
    {
        var reward = new LastReward();
//        reward.Money = 50;

//        var addExp = winner.Army.Army.FirstOrDefault(x => x.Ship.ShipType != ShipType.Base);
//        if (addExp != null)
//        {
//            for (int i = 0; i < 8; i++)
//            {
//                addExp.Pilot.UpgradeLevelByType(LibraryPilotUpgradeType.health, false);
//            }
//        }
//
//        int index;
//        var laser = Library.CreateWeaponByType(WeaponType.laser);
//        if (winner.Inventory.GetFreeSlot(out index, ItemType.weapon))
//        {
//            winner.Inventory.TryAddWeaponModul(laser, index);
//        }
//        reward.Weapons.Add(laser); 
//
//
//        var anti = Library.CreatSimpleModul(SimpleModulType.WeaponShieldIgnore,1);
//        if (winner.Inventory.GetFreeSlot(out index, ItemType.modul))
//        {
//            winner.Inventory.TryAddSimpleModul(anti, index);
//        }
//        reward.Moduls.Add(anti);

        return reward;
    }
}

