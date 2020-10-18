using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EShipScoutType
{
    shipType,
    weapons,
    level,
    moduls,
    spells,
}

public class ShipScoutData
{
    public ShipType ShipType;
    public WeaponType WeaponType;
    public int PilotLevel;
    public List<SimpleModulType> Moduls = new List<SimpleModulType>();
    public List<SpellType> Spells = new List<SpellType>();
    //    public WDictionary<EShipScoutType> Chances;
    private Dictionary<EShipScoutType, float> innerData;
    public List<EShipScoutType> InfoTypeGetToShow;


    public ShipScoutData(StartShipPilotData data)
    {
        var dInfo = new Dictionary<EShipScoutType, float>();
        ShipType = data.Ship.ShipType;
        var notNull = data.Ship.WeaponsModuls.GetNonNullActiveSlots().FirstOrDefault();
        if (notNull != null)
        {
            WeaponType = notNull.WeaponType;
            dInfo.Add(EShipScoutType.weapons, 2);
        }

        PilotLevel = data.Pilot.CurLevel;
        foreach (var modul in data.Ship.Moduls.GetNonNullActiveSlots())
        {
            Moduls.Add(modul.Type);
        }
        foreach (var modul in data.Ship.SpellsModuls.GetNonNullActiveSlots())
        {
            if (modul != null)
                Spells.Add(modul.SpellType);
        }

        dInfo.Add(EShipScoutType.shipType, 3);
        dInfo.Add(EShipScoutType.level, 1);
        if (Spells.Count > 0)
            dInfo.Add(EShipScoutType.spells, 2);
        if (Moduls.Count > 0)
            dInfo.Add(EShipScoutType.moduls, 1);
        innerData = dInfo;
    }

    public void ModifByLevel(int level)
    {
        int maxScoutsLevel = MoneyConsts.MAX_PASSIVE_LEVEL;
        int scoutsTypes = 5;
        float percentToShow = (float)level / (float)maxScoutsLevel;
        var itemsToShow = (int)((scoutsTypes - 1) * percentToShow);
        var infosGet = Mathf.Min(itemsToShow, innerData.Count);
        List<EShipScoutType> infoTypeGet = new List<EShipScoutType>();
        Dictionary<EShipScoutType, float> copyInner = CopyData(innerData);
        for (int i = 0; i < infosGet; i++)
        {
            if (copyInner.Count > 0)
            {
                var wD = new WDictionary<EShipScoutType>(copyInner);
                var result = wD.Random();
                copyInner.Remove(result);
                infoTypeGet.Add(result);
            }
        }

        InfoTypeGetToShow = infoTypeGet;
    }

    private Dictionary<EShipScoutType, float> CopyData(Dictionary<EShipScoutType, float> dictionary)
    {
        Dictionary<EShipScoutType, float> d = new Dictionary<EShipScoutType, float>();
        foreach (var f in dictionary)
        {
            d.Add(f.Key, f.Value);
        }
        return d;
    }
}

public class PlayerScoutData
{


    public Player _player;

    public PlayerScoutData(Player player)
    {
        _player = player;
    }

    public List<ShipScoutData> GetShipScouts(int level)
    {
        List<ShipScoutData> ShipScouts = new List<ShipScoutData>();
        foreach (var data in _player.Army.Army)
        {
            var info = new ShipScoutData(data);
            info.ModifByLevel(level);
            ShipScouts.Add(info);
        }

        return ShipScouts;
    }

    public List<string> GetInfo(int level)
    {
        List<string> data = new List<string>();

        var armyCount = _player.Army.Count;
        WeaponType firstWeaponType = WeaponType.laser;
        bool haveBase = _player.Army.Army.Any(startShipPilotData => startShipPilotData.Ship.ShipType == ShipType.Base);
        int weaponsCount = 0;
        int modulsCount = 0;
        HashSet<WeaponType> allTypes = new HashSet<WeaponType>();
        foreach (var startShipPilotData in _player.Army.Army)
        {
            foreach (var weaponsModul in startShipPilotData.Ship.WeaponsModuls.GetNonNullActiveSlots())
            {
                firstWeaponType = weaponsModul.WeaponType;
                allTypes.Add(firstWeaponType);
                weaponsCount++;
            }
            modulsCount = startShipPilotData.Ship.Moduls.GetNonNullActiveSlots().Count;
        }
        string ssWeapons = allTypes.Aggregate("", (current, weaponType) => current + (current.Length > 0 ? "," : "") + weaponType.ToString());

        var a_rnd1 = MyExtensions.Random(armyCount - 2, armyCount);
        var a_rnd2 = MyExtensions.Random(armyCount, armyCount + 2);
        var s1_rndCount = Namings.Format(Namings.Tag("armyCountBetween"), a_rnd1, a_rnd2);
        var s2_armyCount = Namings.Format(Namings.Tag("armyCount"), armyCount);
        var s3_SomeWeapons = Namings.Format( Namings.Tag("armyWeaponsContains"), firstWeaponType.ToString());
        var s4_HaveMother = haveBase ? Namings.Tag("armyHaveMotherShip") :Namings.Tag("armyJustFleet");
        var s5_weapoModulsCount = Namings.Format( Namings.Tag("armyWeaponsCount"), weaponsCount, modulsCount);
        var s6_allweapons = Namings.Format( Namings.Tag("armyUsingWeapons"), ssWeapons);

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
            case 4:
            case 5:
                data.Add(s2_armyCount);
                data.Add(s4_HaveMother);
                data.Add(s5_weapoModulsCount);
                data.Add(s6_allweapons);
                break;
        }

        return data;
    }
}

