using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PlayerScoutData
{
    public Player _player;

    public PlayerScoutData(Player player)
    {
        _player = player;
    }

    public List<string> GetInfo(int level)
    {
        List<string> data = new List<string>();

        var armyCount = _player.Army.Count;
        WeaponType firstWeaponType = WeaponType.laser;
        bool haveBase = _player.Army.Any(startShipPilotData => startShipPilotData.Ship.ShipType == ShipType.Base);
        int weaponsCount = 0;
        int modulsCount = 0;
        HashSet<WeaponType> allTypes = new HashSet<WeaponType>();
        foreach (var startShipPilotData in _player.Army)
        {
            foreach (var weaponsModul in startShipPilotData.Ship.WeaponsModuls)
            {
                if (weaponsModul != null)
                {
                    firstWeaponType = weaponsModul.WeaponType;
                    allTypes.Add(firstWeaponType);
                    weaponsCount++;
                }
            }
            foreach (var baseModulInv in startShipPilotData.Ship.Moduls.SimpleModuls)
            {
                if (baseModulInv != null)
                {
                    modulsCount++;
                }
            }
        }
        string ssWeapons = allTypes.Aggregate("", (current, weaponType) => current + (current.Length > 0 ? "," : "") + weaponType.ToString());

        var a_rnd1 = MyExtensions.Random(armyCount - 2, armyCount);
        var a_rnd2 = MyExtensions.Random(armyCount, armyCount + 2);
        var s1_rndCount = String.Format("Army count between {0} and {1}", a_rnd1, a_rnd2);
        var s2_armyCount = String.Format("Army count is {0}", armyCount);
        var s3_SomeWeapons = String.Format("Weapons types contains {0}", firstWeaponType.ToString());
        var s4_HaveMother = haveBase ? "Have mother ship" : "Just fleet";
        var s5_weapoModulsCount = String.Format("Weapons count:{0}. Moduls count:{1}", weaponsCount, modulsCount);
        var s6_allweapons = String.Format("Using weapons:{0}", ssWeapons);

        switch (level)
        {
            case 1:
                data.Add(s1_rndCount);
                data.Add(s4_HaveMother);
                break;
            case 2:
                data.Add(s2_armyCount);
                data.Add(s3_SomeWeapons);
                data.Add(s4_HaveMother);
                break;
            case 3:
                data.Add(s2_armyCount);
                data.Add(s4_HaveMother);
                data.Add(s5_weapoModulsCount);
                data.Add(s6_allweapons);
                break;
        }

        return data;
    } 
}

