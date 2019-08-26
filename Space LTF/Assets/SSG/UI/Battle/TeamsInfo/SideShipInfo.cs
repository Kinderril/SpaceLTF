using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SideShipInfo : MonoBehaviour
{
    public const string PREFS_KEY = "SideShipInfo{0}";

    public ShipSlidersInfo ShipSlidersInfo;
    public Image ActionIcon;
    public Image ShipTypeIcon;
    public TacticsButton TacticIcons;

    public Transform ControlBlockHolder;


    public Transform FullOpenHolder;
    public Transform MinorOpenHolder;
    public Transform FullInfoContainer;

    public Transform WeaponsLayout;

    public TryChargeButton TryChargeButton;
    public TryBuffShip TryReloadButton;

    public TextMeshProUGUI DamageDoneField;
    public TextMeshProUGUI KillsField;
    public Toggle ToggleOpen;
    public RawImage RawImage;
    public RawImage BackRawImage;

    public Image FireDamage;
    public Image ShiedDamage;
    public Image WeaponsDamage;
    public Image EngineDamage;
    
    private ShipBase _ship;
    private Action<ShipBase> _shipSelectedAction;
    private Action<SideShipInfo> _toggleCallback;

    public bool IsOpen => FullInfoContainer.gameObject.activeInHierarchy;
    public int Id => _ship.Id;

    public void Init(ShipBase ship, Action<ShipBase> shipSelectedAction, Action<SideShipInfo> toggleCallback,bool shallOpen)
    {
        FireDamage.gameObject.SetActive(false);
        ShiedDamage.gameObject.SetActive(false);
        WeaponsDamage.gameObject.SetActive(false);
        EngineDamage.gameObject.SetActive(false);
        _toggleCallback = toggleCallback;
        FullInfoContainer.gameObject.SetActive(false);
        TacticIcons.gameObject.SetActive(false);
        _shipSelectedAction = shipSelectedAction;
        _ship = ship;
        _ship.ShipInventory.LastBattleData.OnStatChanged += OnStatChanged;
        _ship.OnShipDesicionChange += OnShipDesicionChange;
        _ship.DamageData.OnDamageDone += OnDamageDone;
        OnShipDesicionChange(ship, ship.DesicionData);
        ShipSlidersInfo.Init(ship);
        TryChargeButton.Init(_ship);
        TryReloadButton.Init(_ship);
        InitWeapons();
        ShipTypeIcon.sprite =
            DataBaseController.Instance.DataStructPrefabs.GetShipTypeIcon(ship.ShipParameters.StartParams.ShipType);
        ToggleOpen.isOn = shallOpen;
        UpdateToggle(shallOpen);

    }

    private void OnDamageDone(ShipBase arg1, ShipDamageType arg2, bool val)
    {
        switch (arg2)
        {
            case ShipDamageType.engine:
                EngineDamage.gameObject.SetActive(val);
                break;
            case ShipDamageType.weapon:
                WeaponsDamage.gameObject.SetActive(val);
                break;
            case ShipDamageType.shiled:
                ShiedDamage.gameObject.SetActive(val);
                break;
            case ShipDamageType.fire:
                FireDamage.gameObject.SetActive(val);
                break;
        }

    }

    private void InitWeapons()
    {
        var prefab = DataBaseController.Instance.DataStructPrefabs.WeaponModulUI;
        var weapons = _ship.WeaponsController.GelAllWeapons();
        WeaponsLayout.gameObject.SetActive(weapons.Count > 0);
        foreach (var baseModul in weapons)
        {
            if (baseModul != null)
            {
                var e = DataBaseController.GetItem(prefab);
                e.transform.SetParent(WeaponsLayout);
                e.gameObject.SetActive(false);
//                var wModul = _weaponModuls[weaponIndex];
                e.Init(baseModul);
//                weaponIndex++;
            }
        }
    }

    private void OnStatChanged(ShipBattleData obj)
    {
        DamageDoneField.text = String.Format(Namings.DamageInfoUI,obj.ShieldhDamage.ToString("0"),obj.HealthDamage.ToString("0"));
        KillsField.text = String.Format(Namings.KillsInfoUI,obj.Kills.ToString("0"));
    }

    public void OnToggleClick()
    {
//        if (ToggleOpen.isOn != FullInfoContainer.gameObject.activeSelf)
//        {
//            Debug.Log($"OnToggleClick {ToggleOpen.isOn}  {_ship.Id}");
            var showFull = ToggleOpen.isOn;
            UpdateToggle(showFull);
            PlayerPrefs.SetInt(String.Format(PREFS_KEY, _ship.Id), (showFull ? 1 : 0));
            _toggleCallback(this);
//        }

    }

    public void ToggleViaCode()
    {
        ToggleOpen.isOn = !ToggleOpen.isOn;
        OnToggleClick();
    }
    public void OnClickRunAway()
    {
        _ship.RunAwayAction();
    }
    private void UpdateToggle(bool val)
    {
        FullInfoContainer.gameObject.SetActive(val);
        if (val)
        {
            //FullOpenHolder.gameObject.SetActive(true);
            //ControlBlockHolder.SetParent(FullOpenHolder, false);
            _ship.SelfCamera.Init(RawImage,_ship);
//            if (BackRawImage.texture == null)
//                BackRawImage.texture = DataBaseController.Instance.DataStructPrefabs.BackgroundRenderTexture;
        }
        else
        {
            //FullOpenHolder.gameObject.SetActive(false);
            //ControlBlockHolder.SetParent(MinorOpenHolder, false);
            _ship.SelfCamera.Dispose();
        }
    }
    

    private void OnShipDesicionChange(ShipBase arg1, IShipDesicion arg2)
    {
        if (arg2 != null)
        {
            Sprite icon = DataBaseController.Instance.DataStructPrefabs.GetTacticIcon(arg2.GetTacticType());
            ActionIcon.sprite = icon;
//            ActionIcon.gameObject.SetActive(true);
        }
        else
        {
//            ActionIcon.gameObject.SetActive(false);
        }
    }

    public void OnTacticClick()
    {
        TacticIcons.gameObject.SetActive(!TacticIcons.gameObject.activeSelf);
        TacticIcons.Init(_ship.DesicionData.GetTacticType(),OnTatciClick);
    }

    private void OnTatciClick(PilotTcatic obj)
    {
        _ship.DesicionData.TryChangeTactic(obj);
        TacticIcons.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        _shipSelectedAction(_ship);
    }

    public void Dispose()
    {
        _ship.DamageData.OnDamageDone -= OnDamageDone;
        _ship.ShipInventory.LastBattleData.OnStatChanged -= OnStatChanged;
        _ship.OnShipDesicionChange -= OnShipDesicionChange;
        ShipSlidersInfo.Dispose();
        TryChargeButton.Dispose();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))// && !EventSystem.current.IsPointerOverGameObject())
        {
            TacticIcons.gameObject.SetActive(false);
        }
    }
}

