using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InGameMainUI : BaseWindow
{
    public DebugUIControl DebugUiControl;
    public ShipModulsUI ShipModulsUI;
    //    public HoldClearCoinUI HoldClearCoinUI;
    public BattleCoinUI BattleCoinUI;
    private ShipBase _selectedShip;
//    private bool _paused = false;
    private SpellInGame _spellSelected;
    public Transform ShipsInfoContainer;
    public ArrowTarget ArrowTarget;
    public Transform FlyingInfosContainer;
    public CameraController MainCamera { get; set; }
    public Toggle debugToggle;
    public Commander MyCommander { get; private set; }
    private BattleController _battle;
    private int layerMask = 1;
    // public Button FastEndButton;
    public PreFinishWindow PreFinish;
    public TeamInfoContainer GreenTeamInfoContainer;
    public TeamInfoContainer RedTeamInfoContainer;
    private Dictionary<int, ShipUIOnMap> ShipsUIs = new Dictionary<int, ShipUIOnMap>();
    public CreditControllerUI CreditController;
    public SpellModulsContainer SpellModulsContainer;
    public event Action<SpellInGame> OnSelectSpell;
    public RetirreButton RetireButton;
    public ReinforsmentsButton ReinforsmentsButton;
    public CamerasLinkButtons CamerasLinkButtons;
    private bool _isTutor;

    public TimeScaleBattleUI TimeScaleBattle;
    //    public Button DebugKillAllEnemies;
    private FlyingNumbersController FlyingNumbersController = new FlyingNumbersController();
    public SimpleTutorialVideo TutorSimple1;
    public SimpleTutorialVideo TutorSimple2;
    public Button TutorButton;
    public event Action<ShipBase> OnShipSelected;

    public ShipBase SelectedShip
    {
        get { return _selectedShip; }
        private set
        {
#if UNITY_EDITOR
            if (value != null)
            {
                if (value.IsDead)
                {
                    Debug.LogError("Selected dead ship");
                }
            }
#endif
            if (_selectedShip != null)
            {
                _selectedShip.OnDeath -= OnDeadSelected;
                _selectedShip.Select(false);
            }
            _selectedShip = value;
            if (_selectedShip != null)
            {
                ShipModulsUI.gameObject.SetActive(true);
                _selectedShip.OnDeath += OnDeadSelected;
                DebugUiControl.InitShip(_selectedShip);
                ShipModulsUI.Init(value);
                _selectedShip.Select(true);
                ArrowTarget.SetOwner(_selectedShip);
            }
            else
            {
                ShipModulsUI.gameObject.SetActive(false);
            }
            OnShipSelected?.Invoke(_selectedShip);
            CamerasLinkButtons.SelectedShip(_selectedShip);
            //Debug.Log("Ship selected " + _selectedShip.Id);

            //            UpdateModuls();
        }
    }


    // public void CanFastEnd()
    // {
    //     FastEndButton.gameObject.SetActive(true);
    // }

    public void Init(BattleController battle)
    {
        //#if UNITY_EDITOR
        //        DebugKillAllEnemies.gameObject.SetActive(true);
        //#else
        //        DebugKillAllEnemies.gameObject.SetActive(false);
        //#endif
        //        if (WindowKeys != null)
        //        {
        //            WindowKeys = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.WindowKeys);
        //            WindowKeys.transform.SetParent(transform);

        //            WindowKeys.transform.localPosition = Vector3.zero;
        //            WindowKeys.Init();
        //            WindowKeys.transform.SetAsLastSibling();
        //            WindowKeys.gameObject.SetActive(false);
        //        }
        //        CommanderPriority.Init(MyCommander);
        FlyingNumbersController.Init(FlyingInfosContainer);
        PreFinish.Init();
        // FastEndButton.gameObject.SetActive(false);
        ShipModulsUI.gameObject.SetActive(false);
        this._battle = battle;
        UnselectSpell();
        _isTutor = battle.RedCommander.Player is PlayerAITutor;
        TutorSimple1.Init();
        TutorSimple2.Init();

        MyCommander = battle.GreenCommander;
        BattleCoinUI.Init(MyCommander.CoinController);
        battle.OnShipAdd += OnShipAdd;
        var canRetire = battle.CanRetire;
        if (_isTutor)
        {
            canRetire = false;

        }
        RetireButton.Init(this, 15f, canRetire);
        ReinforsmentsButton.Init(this);
        DebugUiControl.Init();
        //        HoldClearCoinUI.Init(HoldComplete);
        CreditController.Init(MyCommander);
        MainCamera = CamerasController.Instance.GameCamera;
        CamerasLinkButtons.Init(MainCamera);
        GreenTeamInfoContainer.Init(this,battle.GreenCommander, ActionShipSelected);
        RedTeamInfoContainer.Init(this, battle.RedCommander, ActionShipSelected);
        int weaponsIndex = 0;
        //        WindowKeys.gameObject.SetActive(false);
        TimeScaleBattle.Init(_battle);
        var mainShip = MyCommander.MainShip;
        if (mainShip != null)
        {
            SpellModulsContainer.Init(this, MyCommander.SpellController, mainShip, OnSpellClicked, MyCommander.CoinController);
        }
        TutorButton.gameObject.SetActive(_isTutor);
        ActivateCurrentTutor();
    }

    public void OnClickOpenLastTutor()
    {
        ActivateCurrentTutor();
    }

    private void ActivateCurrentTutor()
    {

        if (_isTutor)
        {
            BattleController.Instance.PauseData.Pause();
            if (_battle.RedCommander.Ships.Count <= 1)
            {
                TutorSimple1.Open(Unpause);
            }
            else
            {
                TutorSimple2.Open(Unpause);
            }
        }
    }

    private void Unpause()
    {

        BattleController.Instance.PauseData.Unpase(1f);
    }

    private void OnSpellClicked(SpellInGame spellToCast)
    {
        if (spellToCast.CanCast())
        {
            if (_spellSelected != null)
            {
                _spellSelected.EndShowCast();
            }

            _spellSelected = spellToCast;
            if (OnSelectSpell != null)
            {
                OnSelectSpell(_spellSelected);
            }
            _spellSelected.StartShowCast();
            Debug.Log("spell select " + _spellSelected.Name);
        }
        else
        {
            SpellModulsContainer.CastFail();
            Debug.Log("Can't cast spell " + spellToCast.Name + "   " + spellToCast.CostCount);
        }
    }

    public void OnKillOnEnemiesDebugClick()
    {
#if UNITY_EDITOR
        DebugUtils.KillAllEnemies();
#endif
    }
    private void UnselectSpell()
    {
        _spellSelected = null;
        if (OnSelectSpell != null)
        {
            OnSelectSpell(_spellSelected);
        }
    }

    void Update()
    {
        var ray = GetPointByClick(Input.mousePosition);
        if (ray.HasValue)
        {
            if (_spellSelected != null)
            {
                _spellSelected.UpdateShowCast(ray.Value);
            }

        }
    }

    public void OnClickSettings()
    {
        BattleController.Instance.PauseData.Pause();

        void UnPause()
        {
            BattleController.Instance.PauseData.Unpase(1f);
        }
        WindowManager.Instance.OpenSettingsSettings(EWindowSettingsLauch.battle, UnPause);
    }

    public void ActionShipSelected(ShipBase obj)
    {
        if (obj == SelectedShip)
        {
            CenterCam(obj);
        }
        SelectedShip = obj;
    }


    private void DoRunAway(HashSet<ShipBase> shipsToDamage)
    {
        BattleController.Instance.RunAway(shipsToDamage);
    }

    private void CenterCam(ShipBase shipBase)
    {
        CamerasController.Instance.SetCameraTo(shipBase.Position);
    }

    private void OnDeadSelected(ShipBase obj)
    {
        SelectedShip = null;
    }

    private void OnShipAdd(ShipBase ship, bool val)
    {
        //        Debug.Log("OnShipAdd " + ship.Id + "   val:" + val);
        if (val)
        {
            FlyingNumbersController.AddShip(ship);
            ShipUIOnMap info;
            var fullSize = ship.TeamIndex == TeamIndex.green;
            if (fullSize)
            {
                info = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.ShipUIOnMap);
            }
            else
            {
                info = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.ShipUIOnMapMini);
            }
            info.Init(ship, fullSize);
            //            ship.DamageData.OnDamageDone += OnDamageDone;
            //            ship.ShipParameters.OnHealthChanged += OnHealthChanged;
            //            ship.ShipParameters.ShieldParameters.OnShildChanged += OnShildChanged;
            ShipsUIs.Add(ship.Id, info);
            info.transform.SetParent(ShipsInfoContainer);
        }
        else
        {
            FlyingNumbersController.RemoveShip(ship);
            //            ship.DamageData.OnDamageDone -= OnDamageDone;
            //            ship.ShipParameters.OnHealthChanged -= OnHealthChanged;
            //            ship.ShipParameters.ShieldParameters.OnShildChanged -= OnShildChanged;
            var d = ShipsUIs[ship.Id];
            GameObject.Destroy(d.gameObject);
            ShipsUIs.Remove(ship.Id);
        }
    }

    private void OnDamageDone(ShipBase shipOwner, ShipDamageType arg1, bool arg2)
    {
        string info = "";
        switch (arg1)
        {
            case ShipDamageType.engine:
                info = Namings.Tag("EngineDest");
                break;
            //            case ShipDamageType.turnEngine:
            //                info = "Turn engine destroyed";
            //                break;
            // case ShipDamageType.weapon:
            //     info = Namings.WeaponDest; 
            //     break;
            case ShipDamageType.shiled:
                info = Namings.Tag("ShieldDest");
                break;
            //            case ShipDamageType.moduls:
            //                info = "Modul destroyed";
            //                break;
            case ShipDamageType.fire:
                info = Namings.Tag("FireDest");
                break;
        }
        Debug.LogError($"add {arg1}");
        FlyNumberWithDependence.Create(shipOwner.transform, info, Color.red, FlyingInfosContainer, FlyNumerDirection.right);
    }

    private string GetDeltaStr(float delta)
    {
        return ((delta > 0) ? "+" : "") + delta.ToString("0");
    }

    public void Hold(Vector3 pos, bool left, float delta)
    {
        //        Debug.Log("hold...");
        //        var pt = GetShipByPoint(pos);
        //        if (pt != null)
        //        {
        //            HoldClearCoinUI.Hold(pt,left,delta);
        //        }
    }

    public void OnClickReinforsment()
    {
        var player = MainController.Instance.MainPlayer;
        if (player.ReputationData.TryCallReinforsments(out ShipConfig config))
        {
            BattleController.Instance.CallReinforcments(config);
        }
        else
        {
#if UNITY_EDITOR                                                
            BattleController.Instance.CallReinforcments(ShipConfig.droid);
#endif
        }
    }

    public void OnClickFastFinish()
    {
        _battle.FastFinish();
    }

    public void Clicked(Vector3 pos, bool left, float delta)
    {
        var isOverUI = EventSystem.current.IsPointerOverGameObject();
        if (isOverUI)
        {
            return;
        }

        if (_spellSelected != null)
        {
            if (!BattleController.Instance.PauseData.IsPause)
            {
                if (left)
                {
                    var ray = GetPointByClick(pos);
                    //                    Debug.LogError($"Try cast CLICK!   {ray.HasValue}");
                    if (ray.HasValue)
                    {
                        if (MyCommander.SpellController.TryCastspell(_spellSelected, ray.Value))
                        {
                            Debug.Log("spell TryCast " + _spellSelected.Name);
                            EndCastSpell();
                            return;
                        }
                        EndCastSpell();
                    }
                    EndCastSpell();
                }
                else
                {
                    EndCastSpell();
                }
            }
        }
        else
        {
            if (left)
            {
                if (delta < HoldClearCoinUI.BOT_LINE)
                {
                    var pt = GetShipByPoint(pos);
                    if (pt != null)
                    {
                        if (left)
                        {
                            SelectedShip = pt;
                        }
                    }
                }
            }
            else
            {
                var myMainShip = MyCommander.MainShip;
                var ray = GetPointByClick(pos);
                if (ray.HasValue)
                {
                    myMainShip.GoToPointAction(ray.Value, true);
                }
            }
        }
    }

    private void EndCastSpell()
    {
        if (_spellSelected != null)
        {
            _spellSelected.EndShowCast();
        }
        UnselectSpell();
    }

    private Vector3? GetPointByClick(Vector3 pos)
    {
        if (MainCamera == null)
        {
            return null;
        }
        var ray = MainCamera.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 9999999, layerMask))
        {
            return hit.point;
        }
        return null;
    }

    private ShipBase GetShipByPoint(Vector3 pos)
    {
        var ray = GetPointByClick(pos);
        if (ray.HasValue)
        {
            return _battle.Clicked(ray.Value);
        }
        return null;
    }

    public override void Dispose()
    {
        if (_spellSelected != null)
        {
            _spellSelected.EndShowCast();
        }

        OnShipSelected = null;
        MainCamera.ReturnCamera();
        //        WindowKeys.gameObject.SetActive(false);
        FlyingNumbersController.Dispose();
        UnselectSpell();
        CreditController.Dispose();
        foreach (var shipUiOnMap in ShipsUIs)
        {
            Destroy(shipUiOnMap.Value);
        }
        SpellModulsContainer.Dispose();
    }

    public void OnShowNextShip()
    {
        SelectedShip = MyCommander.GetNextShip(SelectedShip);
    }

    public void OnClickDebugShip()
    {
        DebugUiControl.Enable(debugToggle.isOn);
    }

    public void EndBattle(EndBattleType endBattleType)
    {
        GreenTeamInfoContainer.Dispose();
        RedTeamInfoContainer.Dispose();
        BattleCoinUI.Dispose();
        CamerasLinkButtons.Dispose();
        ArrowTarget.Disable();
        PreFinish.Activate(_battle, endBattleType);
    }
}

