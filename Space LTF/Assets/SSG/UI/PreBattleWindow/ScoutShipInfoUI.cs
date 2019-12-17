using UnityEngine;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class ScoutShipInfoUI : MonoBehaviour
{
    public ImageWithTooltip ShipTypeImage;
    public TextMeshProUGUI LevelField;
    public Transform Layout;

    public void Init(ShipScoutData data)
    {
        ShipTypeImage.gameObject.SetActive(false);
        LevelField.gameObject.SetActive(false);

        foreach (var eShipScoutType in data.InfoTypeGetToShow)
        {
            switch (eShipScoutType)
            {
                case EShipScoutType.shipType:
                    ShipTypeImage.gameObject.SetActive(true);
                    ShipTypeImage.Init(DataBaseController.Instance.DataStructPrefabs.GetShipTypeIcon(data.ShipType), Namings.ShipType(data.ShipType) );
                    break;
                case EShipScoutType.weapons:
                    var weaponImage = DataBaseController.GetItem(ShipTypeImage);
                    weaponImage.transform.SetParent(Layout,false);
                    weaponImage.transform.SetAsLastSibling();
                    weaponImage.Init(DataBaseController.Instance.DataStructPrefabs.GetWeaponIcon(data.WeaponType),Namings.Weapon(data.WeaponType));
                    weaponImage.gameObject.SetActive(true);
                    break;
                case EShipScoutType.level:
                    LevelField.gameObject.SetActive(true);
                    LevelField.text = data.PilotLevel.ToString();
                    break;
                case EShipScoutType.moduls:
                    foreach (var simpleModulType in data.Moduls)
                    {
                        var modul = DataBaseController.GetItem(ShipTypeImage);
                        modul.transform.SetParent(Layout, false);
                        modul.transform.SetAsLastSibling();
                        modul.Init(DataBaseController.Instance.DataStructPrefabs.GetModulIcon(simpleModulType),Namings.SimpleModulName(simpleModulType));
                        modul.gameObject.SetActive(true);
                    }
                    break;
                case EShipScoutType.spells:
                    foreach (var spellType in data.Spells)
                    {
                        var modul = DataBaseController.GetItem(ShipTypeImage);
                        modul.transform.SetParent(Layout, false);
                        modul.transform.SetAsLastSibling();
                        modul.Init(DataBaseController.Instance.DataStructPrefabs.GetSpellIcon(spellType),Namings.SpellName(spellType));
                        modul.gameObject.SetActive(true);
                    }
                    break;
            }
        }
    }
}
