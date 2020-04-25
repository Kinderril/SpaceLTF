using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class SideShipInfo : MonoBehaviour
{
    public const string PREFS_KEY = "SideShipInfo{0}";
    private const float TACTIC_CHANGE_PERIOD = 3f;

    public ShipSlidersInfo ShipSlidersInfo;
    //    public Image ActionIcon;
    public Image ShipTypeIcon;

    public Transform FullInfoContainer;

    public Transform WeaponsLayout;

    public TextMeshProUGUI DamageDoneField;
    public TextMeshProUGUI KillsField;
    public Toggle ToggleOpen;
    public RawImage RawImage;
    public RawImage BackRawImage;

    public Image FireDamage;
    public Image ShiedDamage;
    // public Image WeaponsDamage;
    public Image EngineDamage;

    public Image TacticPriorityIcon;
    public Image TacticSideIcon;
    public Image GlobalTacticsIcon;
    public CanvasGroup TacticCanvas;
//    private bool _isTaticReady;


    public PriorityTooltipInfo PriorityTooltipInfo;
    public SideAttackTooltipInfo SideAttackTooltipInfo;
    public GlobaltacticsTooltipInfo GlobaltacticsTooltipInfo;
    public Slider BoostSlider;
    private float _nextPosibleChangeTactics;
    public Image TacticDelayLoad;
                                        
    private ShipBase _ship;
    private Action<ShipBase> _shipSelectedAction;
    private Action<SideShipInfo> _toggleCallback;
    public Image SelctedObject;

    public bool IsOpen => FullInfoContainer.gameObject.activeInHierarchy;
    public int Id => _ship.Id;
    private InGameMainUI _mainUI;

    public void Init(ShipBase ship, Action<ShipBase> shipSelectedAction,
        Action<SideShipInfo> toggleCallback, bool shallOpen,InGameMainUI mainUI)
    {
        UnSelect();
        _mainUI = mainUI;
        _mainUI.OnShipSelected += OnShipSelected;
//        _isTaticReady = true;
        FireDamage.gameObject.SetActive(false);
        ShiedDamage.gameObject.SetActive(false);
        // WeaponsDamage.gameObject.SetActive(false);
        EngineDamage.gameObject.SetActive(false);
        _toggleCallback = toggleCallback;
        FullInfoContainer.gameObject.SetActive(false);
        _shipSelectedAction = shipSelectedAction;
        _ship = ship;
        _ship.ShipInventory.LastBattleData.OnStatChanged += OnStatChanged;
        _ship.OnShipDesicionChange += OnShipDesicionChange;
        _ship.DamageData.OnDamageDone += OnDamageDone;
        OnShipDesicionChange(ship, ship.DesicionData);
        ShipSlidersInfo.Init(ship);
        ship.DesicionData.OnChagePriority += OnChagePriority;
        //        _pilot.Tactic.OnSideChange += OnTacticSideChange;
        //        TryWaveButton.Init(_ship,5);
        //        TryChargeButton.Init(_ship,5);
        //        TryWeaponsShipButton.Init(_ship);
        //        TryReloadButton.Init(_ship);
        InitWeapons();
        ShipTypeIcon.sprite =
            DataBaseController.Instance.DataStructPrefabs.GetShipTypeIcon(ship.ShipParameters.StartParams.ShipType);
        ToggleOpen.isOn = shallOpen;
        UpdateToggle(shallOpen);
        UpdateTacticField();
        TacticCanvas.interactable = true;

    }

    private void OnShipSelected(ShipBase obj)
    {
        if (obj == null)
        {
            UnSelect();
        }
        else
        {
            if (obj == _ship)
            {
                Select();
            }
            else
            {

                UnSelect();
            }
        }
    }

    private void UnSelect()
    {
        SelctedObject.color = Color.white;
    }

    private void Select()
    {
        SelctedObject.color = Color.green;
    }

    private void OnChagePriority()
    {
        _nextPosibleChangeTactics = Time.time + TACTIC_CHANGE_PERIOD;
        TacticCanvas.interactable = false;
        UpdateTacticField();
    }

    void Update()
    {
        if (_ship.Boost.IsReady)
        {
            if (BoostSlider.value < 1f)
                BoostSlider.value = 1f;
        }
        else
        {
            BoostSlider.value = _ship.Boost.LoadPercent;
        }

        if (!TacticCanvas.interactable)
        {
            if (!TacticDelayLoad.gameObject.activeSelf)
                TacticDelayLoad.gameObject.SetActive(true);

            var pecnet = (_nextPosibleChangeTactics - Time.time) / TACTIC_CHANGE_PERIOD;
            if (pecnet <= 0)
            {
                TacticCanvas.interactable = true;
            }
            TacticDelayLoad.fillAmount = Mathf.Clamp01(pecnet);
        }
        else
        {
            if (TacticDelayLoad.gameObject.activeSelf)
                TacticDelayLoad.gameObject.SetActive(false);
        }
    }
    private void UpdateTacticField()
    {
        TacticPriorityIcon.sprite = DataBaseController.Instance.DataStructPrefabs.GetTacticIcon(_ship.DesicionData.CommanderPriority1);
        TacticSideIcon.sprite = DataBaseController.Instance.DataStructPrefabs.GetTacticIcon(_ship.DesicionData.SideAttack);
        GlobalTacticsIcon.sprite = DataBaseController.Instance.DataStructPrefabs.GetTacticIcon(_ship.DesicionData.GlobalTactics);
        PriorityTooltipInfo.SetData(_ship.DesicionData.CommanderPriority1);
        SideAttackTooltipInfo.SetData(_ship.DesicionData.SideAttack);
        GlobaltacticsTooltipInfo.SetData(_ship.DesicionData.GlobalTactics);
    }
    private void OnDamageDone(ShipBase arg1, ShipDamageType arg2, bool val)
    {
        switch (arg2)
        {
            case ShipDamageType.engine:
                EngineDamage.gameObject.SetActive(val);
                break;
            // case ShipDamageType.weapon:
            //     WeaponsDamage.gameObject.SetActive(val);
            // break;
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
                e.Init(baseModul);
            }
        }
    }

    private void OnStatChanged(ShipBattleData obj)
    {
        DamageDoneField.text = Namings.Format(Namings.Tag("DamageInfoUI"), obj.ShieldhDamage.ToString("0"), obj.HealthDamage.ToString("0"));
        KillsField.text = Namings.Format(Namings.Tag("KillsInfoUI"), obj.Kills.ToString("0"));
    }

    public void OnToggleClick()
    {
        //        if (ToggleOpen.isOn != FullInfoContainer.gameObject.activeSelf)
        //        {
        //            Debug.Log($"OnToggleClick {ToggleOpen.isOn}  {_ship.Id}");
        var showFull = ToggleOpen.isOn;
        UpdateToggle(showFull);
        PlayerPrefs.SetInt(Namings.Format(PREFS_KEY, _ship.Id), (showFull ? 1 : 0));
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
            _ship.SelfCamera.Init(RawImage, _ship);
            //            if (BackRawImage.texture == null)
            //                BackRawImage.texture = DataBaseController.Instance.DataStructPrefabs.BackgroundRenderTexture;
        }
        else
        {
            //FullOpenHolder.gameObject.SetActive(false);
            //ControlBlockHolder.SetParent(MinorOpenHolder, false);
            _ship.SelfCamera.Dispose();
        }
    }            //Base defence
    public void ClickFight()
    {
        _ship.DesicionData.ChangePriority(EGlobalTactics.Fight);
    }
    public void ClickGosafe()
    {
        _ship.DesicionData.ChangePriority(EGlobalTactics.GoSafe);
    }

    //Straight|Flangs
    public void ClickStraight()
    {
        _ship.DesicionData.ChangePriority(ESideAttack.Straight);
    }
    public void ClickBaseDefence()
    {
        _ship.DesicionData.ChangePriority(ESideAttack.BaseDefence);
    }
    public void ClickFlangs()
    {
        _ship.DesicionData.ChangePriority(ESideAttack.Flangs);
    }

    //Base CommanderPriority   
    public void ClickECommanderPriority1Any()
    {
        _ship.DesicionData.ChangePriority(ECommanderPriority1.Any);
    }
    public void ClickECommanderPriority1MinShield()
    {
        _ship.DesicionData.ChangePriority(ECommanderPriority1.MinShield);
    }
    public void ClickECommanderPriority1MinHealth()
    {
        _ship.DesicionData.ChangePriority(ECommanderPriority1.MinHealth);
    }
    public void ClickECommanderPriority1MaxShield()
    {
        _ship.DesicionData.ChangePriority(ECommanderPriority1.MaxShield);
    }
    public void ClickECommanderPriority1MaxHealth()
    {
        _ship.DesicionData.ChangePriority(ECommanderPriority1.MaxHealth);
    }
    public void ClickECommanderPriority1Fast()
    {
        _ship.DesicionData.ChangePriority(ECommanderPriority1.Fast);
    }
    public void ClickECommanderPriority1Slow()
    {
        _ship.DesicionData.ChangePriority(ECommanderPriority1.Slow);
    }
    public void ClickECommanderPriority1Base()
    {
        _ship.DesicionData.ChangePriority(ECommanderPriority1.Base);
    }
    private void OnShipDesicionChange(ShipBase arg1, IShipDesicion arg2)
    {

    }

    public void OnClick()
    {
        _shipSelectedAction(_ship);
    }

    public void Dispose()
    {
        _mainUI.OnShipSelected -= OnShipSelected;
        _ship.DamageData.OnDamageDone -= OnDamageDone;
        _ship.ShipInventory.LastBattleData.OnStatChanged -= OnStatChanged;
        _ship.OnShipDesicionChange -= OnShipDesicionChange;
        _ship.DesicionData.OnChagePriority -= OnChagePriority;
        ShipSlidersInfo.Dispose();
        //        TryChargeButton.Dispose();
        //        TryWeaponsShipButton.Dispose();
    }

    //    void Update()
    //    {
    //        if (Input.GetMouseButtonUp(0))// && !EventSystem.current.IsPointerOverGameObject())
    //        {
    //            TacticIcons.gameObject.SetActive(false);
    //        }
    //    }
}

