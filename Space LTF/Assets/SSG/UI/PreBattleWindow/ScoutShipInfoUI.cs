using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoutShipInfoUI : MonoBehaviour
{
    public ImageWithTooltip ShipTypeImage;
    public TextMeshProUGUI LevelField;
    public Transform Layout;
    public Transform Modules;
    public Transform Spells;

    public void Init(ShipScoutData data)
    {
        ShipTypeImage.gameObject.SetActive(false);
        LevelField.gameObject.SetActive(false);
        var allScountTYpes = ((EShipScoutType[])Enum.GetValues(typeof(EShipScoutType))).ToList();
        var copy = allScountTYpes.ToList();
        Dictionary<EShipScoutType, GameObject> allObjecst = new Dictionary<EShipScoutType, GameObject>();

        foreach (var eShipScoutType in data.InfoTypeGetToShow)
        {
            allScountTYpes.Remove(eShipScoutType);
            switch (eShipScoutType)
            {
                case EShipScoutType.shipType:
                    ShipTypeImage.gameObject.SetActive(true);
                    ShipTypeImage.Init(DataBaseController.Instance.DataStructPrefabs.GetShipTypeIcon(data.ShipType), Namings.ShipType(data.ShipType));
                    allObjecst.Add(eShipScoutType, ShipTypeImage.gameObject);
                    break;
                case EShipScoutType.weapons:
                    var weaponImage = DataBaseController.GetItem(ShipTypeImage);
                    weaponImage.transform.SetParent(Layout, false);
                    weaponImage.transform.SetAsLastSibling();
                    weaponImage.Init(DataBaseController.Instance.DataStructPrefabs.GetWeaponIcon(data.WeaponType), Namings.Weapon(data.WeaponType));
                    weaponImage.gameObject.SetActive(true);
                    allObjecst.Add(eShipScoutType, weaponImage.gameObject);
                    break;
                case EShipScoutType.level:
                    LevelField.gameObject.SetActive(true);
                    LevelField.text = data.PilotLevel.ToString();
                    allObjecst.Add(eShipScoutType, LevelField.gameObject);
                    break;
                case EShipScoutType.moduls:
                    foreach (var simpleModulType in data.Moduls)
                    {
                        var modul = DataBaseController.GetItem(ShipTypeImage);
                        modul.transform.SetParent(Modules, false);
                        modul.transform.SetAsLastSibling();
                        modul.Init(DataBaseController.Instance.DataStructPrefabs.GetModulIcon(simpleModulType), Namings.SimpleModulName(simpleModulType));
                        modul.gameObject.SetActive(true);
                    }
                    allObjecst.Add(eShipScoutType, Modules.gameObject);
                    break;
                case EShipScoutType.spells:
                    foreach (var spellType in data.Spells)
                    {
                        var modul = DataBaseController.GetItem(ShipTypeImage);
                        modul.transform.SetParent(Spells, false);
                        modul.transform.SetAsLastSibling();
                        modul.Init(DataBaseController.Instance.DataStructPrefabs.GetSpellIcon(spellType), Namings.SpellName(spellType));
                        modul.gameObject.SetActive(true);
                    }
                    allObjecst.Add(eShipScoutType, Spells.gameObject);
                    break;
            }
        }

        foreach (var eShipScoutType in allScountTYpes)
        {
            switch (data.ShipType)
            {
                case ShipType.Light:
                case ShipType.Middle:
                case ShipType.Turret:
                case ShipType.Heavy:
                    if (eShipScoutType == EShipScoutType.spells)
                    {
                        continue;
                    }
                    break;
                case ShipType.Base:
                    if (eShipScoutType == EShipScoutType.weapons || eShipScoutType == EShipScoutType.moduls)
                    {
                        continue;
                    }
                    break;
            }

            if (eShipScoutType == EShipScoutType.weapons && data.ShipType == ShipType.Base)
            {

            }
            var nodata = DataBaseController.Instance.DataStructPrefabs.NoScoutData;
            ImageWithTooltip element = DataBaseController.GetItem(nodata);
            element.transform.SetParent(Layout);
            allObjecst.Add(eShipScoutType, element.gameObject);
            element.Init(null, Namings.Tag($"scout_{eShipScoutType.ToString()}"));
        }

        foreach (var eShipScoutType in copy)
        {
            if (allObjecst.ContainsKey(eShipScoutType))
            {
                var obj = allObjecst[eShipScoutType];
                obj.transform.SetAsLastSibling();
            }
        }
    }
}
