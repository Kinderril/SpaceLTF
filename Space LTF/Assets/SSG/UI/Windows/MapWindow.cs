using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class MapWindow : BaseWindow
{
    public Transform PlayerContainer;
    public Transform MapCellsLayout;
    public GameObject ArmyInfoContainer;

    private bool isArmyActive;

    private Vector3 _stablePos;
    private bool _stablePosCached = false;
    //    public GameObject ModifInfoContainer;

    public MapConsoleUI MapConsoleUI;
    public InventoryUI InventoryUI;
    public DialogWindow DialogWindow;

    public TextMeshProUGUI MainQuestELelemntField;
    public TextMeshProUGUI ReputationField;

    public ChangingCounter MoneyField;
    public MapNavigationList NavigationList;
    public CellIinfoObjectUI CellIinfoObject;
    public ReputationMapUI ReputationMapUI;

    private Player player;
    private PlayerArmyUI playerArmyUI;
    public WindowModif modifWindowUI;
    public GlobalMapController GlobalMap;
    //    public CataclysmUI Cataclysm;
    public PlayerByStepUI PlayerByStepUI;
    private GlobalMapCell _lastClosest = null;
    public GameObject StartInfo;
    public Transform LayoutSideShips;
    public SideShipGlobalMapInfo SideShipGlobalMapInfoPrefabs;
    private bool _sideShipsInited = false;
    private List<SideShipGlobalMapInfo> _sideInfos = new List<SideShipGlobalMapInfo>();
    public event Action OnStartInfoClose;
    //    public MapSettingsWindow WindowSettings;
    public QuestOnStartControllerUI QuestsOnStartController;
    public MovingArmyUIController MovingArmyUIController;

    public event Action<bool> OnOpenInventory;

    public override void Init()
    {
        base.Init();

        CellIinfoObject.Disable();
        MapConsoleUI.Appear();
        NavigationList.Init(GlobalMap);
        //        ShipRepairedObject.gameObject.SetActive(false);
        DialogWindow.Dispose();
        GlobalMap.gameObject.SetActive(true);
        //        WindowSettings.gameObject.SetActive(false);
        ReputationMapUI.gameObject.SetActive(false);
        CamerasController.Instance.StartGlobalMap();
        GlobalMap.UnBlock();
        //        ArmyInfoContainer.gameObject.SetActive(false);
        player = MainController.Instance.MainPlayer;
        player.RepairData.OnSomeShipRepaired += OnSomeShipRepaired;
        //        Cataclysm.Init(player.MapData);
        bool showFirstInfo = player.MapData.Step == 0;
        StartInfo.gameObject.SetActive(showFirstInfo);
        if (showFirstInfo)
        {
            var field = StartInfo.GetComponentInChildren<TextMeshProUGUI>();
            field.text = Namings.Tag("StartInfo");
        }
        //        ReputationMapUI.Init();
        PlayerByStepUI.Init(player.ByStepDamage);
        //        player.MapData.OnCellChanged += OnCellChanged;
        //        player.MapData.OnSectorChanged += OnSectorChanged;
        //        _selectedCell = data.GetNextCell(player.MapData.CurrentCell);
        MoneyField.Init(player.MoneyData.MoneyCount);
        player.MoneyData.OnMoneyChange += OnMoneyChange;
        player.MapData.OnCellChanged += OnCellChanged;
        player.Army.OnAddShip += OnAddShip;
        // player.ReputationData.OnReputationNationChange += OnReputationChange;
        CellsOfSector();
        InitMyArmy();
        GlobalMap.SingleInit(player.MapData.GalaxyData, this, MouseNearObject);
        GlobalMap.Open();
        List<GlobalMapCell> connectedCells = player.MapData.ConnectedCellsToCurrent();
        GlobalMap.SingleReset(player.MapData.CurrentCell, connectedCells);
        InventoryUI.Init(player.Inventory, null);
        GlobalMap.UnBlock();
        player.QuestData.OnElementFound += OnElementFound;

        EnableModif(false);
        EnableArmy(false);
        UpdateMainQuestelements();
        // UpdateReputation();
        InitSideShip();
        QuestsOnStartController.Init(player.QuestsOnStartController);
        CheckLeaveDialog();
        MovingArmyUIController.Init(MainController.Instance.MainPlayer.MapData.GalaxyData.GalaxyEnemiesArmyController, GlobalMap);
    }

    private void CheckLeaveDialog()
    {
        var isCurCellFight = player.MapData.CurrentCell is ArmyGlobalMapCell;
        if (isCurCellFight && MainController.Instance.Statistics.LastBattle == EndBattleType.runAway)
        {
            player.MapData.CurrentCell.Uncomplete();
            ReturnToLastCell();
        }
        else if (!player.MapData.CurrentCell.LeavedDialogComplete)
        {
            var leavedDialog = player.MapData.CurrentCell.GetLeavedActionInnerMain();
            if (leavedDialog != null)
            {
                StartDialog(leavedDialog, (val, val2) =>
                {
                    player.MapData.CurrentCell.LeavedDialogComplete = true;
                    CanvasGroup.interactable = true;
                    GlobalMap.UnBlock();
                });

            }
        }
    }

    private void ReturnToLastCell()
    {
        var lastCell = player.MapData.LastCell;
        if (lastCell == null || lastCell == player.MapData.CurrentCell)
        {
            Debug.LogError($"can't return to last cell  lastCell:{lastCell}");
        }

        player.MapData.GoToTarget(lastCell, GlobalMap, (comeToTarget) =>
        {
            GlobalMap.SingleReset(comeToTarget, player.MapData.ConnectedCellsToCurrent());
            GlobalMap.UnBlock();
        });
    }

    private void InitSideShip()
    {
        if (!_sideShipsInited)
        {
            _sideShipsInited = true;
            foreach (var pilotData in MainController.Instance.MainPlayer.Army.Army)
            {
                OnAddShip(pilotData, true);
            }
        }
        else
        {
            var shipsToDel = new List<SideShipGlobalMapInfo>();
            foreach (var sideShipGlobalMapInfo in _sideInfos)
            {
                sideShipGlobalMapInfo.UpToDate();
                if (sideShipGlobalMapInfo.IsDead())
                {
                    shipsToDel.Add(sideShipGlobalMapInfo);
                }
            }

            foreach (var sideShipGlobalMapInfo in shipsToDel)
            {
                OnAddShip(sideShipGlobalMapInfo.pilot, false);
            }
        }
    }

    private void OnAddShip(StartShipPilotData pilotData, bool arg2)
    {
        if (arg2)
        {
            var element = DataBaseController.GetItem(SideShipGlobalMapInfoPrefabs);
            element.gameObject.transform.SetParent(LayoutSideShips, false);
            element.Init(pilotData, this);
            _sideInfos.Add(element);
        }
        else
        {
            var shipToDel = _sideInfos.FirstOrDefault(x => x.pilot == pilotData);
            if (shipToDel != null)
            {
                _sideInfos.Remove(shipToDel);
                GameObject.Destroy(shipToDel.gameObject);
                shipToDel.Dispose();
            }
            else
            {
                Debug.LogError($"OnAddShip. Delete error {pilotData}");
            }

        }
    }

    public void OnClickReputation()
    {
        ReputationMapUI.Init();
    }
    public void OnStartInfoClick()
    {
        StartInfo.gameObject.SetActive(false);
        if (OnStartInfoClose != null)
        {
            OnStartInfoClose();
        }
    }


    private void OnElementFound()
    {
        UpdateMainQuestelements();
    }

    private void UpdateMainQuestelements()
    {
        var player = MainController.Instance.MainPlayer.QuestData;
        MainQuestELelemntField.text = $"{Namings.Tag("MainElements")}:{player.mainElementsFound}/{player.MaxMainElements}";
        //        NavigationList.UpdateInfo();
    }

    private void OnSomeShipRepaired()
    {
        player.MessagesToConsole.AddMsg(Namings.Tag("ShipsRepaired"));
    }

    // private void UpdateReputation()
    // {
    //     ReputationField.text = Namings.TryFormat(Namings.Reputation,player.ReputationData.Reputation);
    // }


    private void MouseNearObject(GlobalMapCellObject obj)
    {
        var haveObj = obj != null;
        if (haveObj)
        {
            if (_lastClosest != obj.Cell || CellIinfoObject.IsDisabled)
            {
                _lastClosest = obj.Cell;
                var isNothing = obj.Cell is GlobalMapNothing;
                if (!isNothing)
                {
                    if (player.MapData.CanGoTo(obj.Cell) || obj.Cell.InfoOpen || obj.Cell.IsScouted)
                    {
                        CellIinfoObject.Init(obj);
                        return;
                    }
                }
                CellIinfoObject.Disable();
            }
        }
        else
        {
            CellIinfoObject.Disable();
        }


    }

    private void OnMoneyChange(int obj)
    {
        MoneyField.Init(player.MoneyData.MoneyCount);
        player.MessagesToConsole.AddMsg(Namings.Format(Namings.Tag("AddCredits"), obj));
    }

    private void CellsOfSector()
    {
        //        UpdateDayField();
        MapCellsLayout.ClearTransform();
    }

    private void OnSectorChanged()
    {
        CellsOfSector();
    }

    private void OnCellChanged(GlobalMapCell cell)
    {
        //        UpdateDayField();
        player.MessagesToConsole.AddMsg(Namings.Format(Namings.Tag("RelocateTo"), cell.Desc()));
        //        GlobalMap.CellChange();
        //        foreach (var mapCellElement in cellsElements)
        //        {
        //            mapCellElement.Refresh(mapCellElement.Id == player.MapData.CurrentCell);
        //        }
    }

    public void OnClickHome()
    {
        GlobalMap.SetCameraHome();
    }

    public void ClickCell(GlobalMapCell obj)
    {
        if (obj != player.MapData.CurrentCell)
        {
            if (player.MapData.CanGoTo(obj))
            {
                TryInitsMainCellDialog(obj);
            }
            else
            {
                GlobalMap.Block();
                string txt;
                txt = obj.IsDestroyed ? "All destroyed. Nothing to do there." : Namings.Tag("targteFar");
                WindowManager.Instance.InfoWindow.Init(GlobalMap.UnBlock, txt);
            }
        }
        else
        {
            TryInitsMainCellDialog(obj);
        }
    }

    private void TryInitsMainCellDialog(GlobalMapCell obj)
    {
        void ActivateDialog()
        {
            var dialog = obj.GetDialogMain(out var activateAnyway);
            
            if (dialog != null)
            {
                if (activateAnyway || !(obj.Completed && obj.OneTimeUsed()))
                {
                    StartDialog(dialog, OnMainDialogEnds);
                }
                else
                {
                    OnMainDialogEnds(true, false);
                }
            }
            else
            {
                OnMainDialogEnds(true, false);
            }
        }

        if (player.MapData.GoToTarget(obj, GlobalMap, (target) =>
        {
            ActivateDialog();
        }))
        {

        }
        else
        {
            ActivateDialog();

        }
    }

    public void DebugActivateEndDialog()
    {
        var endCell =
            MainController.Instance.MainPlayer.MapData.GalaxyData.GetAllList()
                .FirstOrDefault(x => x is EndGlobalCell) as EndGlobalCell;
        if (endCell != null)
        {
            ActivateCellDialog(endCell);
        }
        else
        {
            Debug.LogError("Can't find end game cell");
        }
    }

    public void ActivateCellDialog(GlobalMapCell cell)
    {
        var dialog = cell.GetDialogMain(out var activateAnyway);
        StartDialog(dialog, OnMainDialogEnds);
    }

    private void OnMainDialogEnds(bool shallCompleteCell, bool shallReturnToLastCell)
    {
        if (shallCompleteCell)
        {
            player.MapData.CurrentCell.Complete();
        }
        GlobalMap.SingleReset(player.MapData.CurrentCell, player.MapData.ConnectedCellsToCurrent());
        GlobalMap.UnBlock();
        CanvasGroup.interactable = true;
        if (shallReturnToLastCell)
        {
            ReturnToLastCell();
        }
    }

    private void InitMyArmy()
    {
        playerArmyUI = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.PlayerArmyUIPrefab);
        playerArmyUI.Init(player, PlayerContainer, true, new ConnectInventory(player.Inventory));
    }

    public void OnArmyShowClick()
    {
        if (isArmyActive)
        {
            EnableArmy(false);
        }
        else
        {
            // EnableModif(false);
            EnableArmy(true);
            GlobalMap.Block();
        }
    }

    private void EnableArmy(bool val)
    {
        if (val)
        {
            WindowManager.Instance.UiAudioSource.PlayOneShot(DataBaseController.Instance.AudioDataBase.WindowOpen);
            ArmyInfoContainer.transform.position = _stablePos;
            InventoryUI.RefreshPosition();
        }
        else
        {
            if (!_stablePosCached)
            {
                _stablePosCached = true;
                _stablePos = ArmyInfoContainer.transform.position;
            }
            var v = new Vector3(5000, 0, 0);
            ArmyInfoContainer.transform.position = v;
            GlobalMap.UnBlock();
        }
        if (OnOpenInventory != null)
        {
            OnOpenInventory(val);
        }
        isArmyActive = val;
    }


    private void EnableModif(bool val)
    {
        if (val)
        {
            modifWindowUI.Enable();
        }
        else
        {
            modifWindowUI.Disable();
        }
        modifWindowUI.gameObject.SetActive(val);
    }
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // if (modifWindowUI.gameObject.activeSelf)
            // {
            //     EnableModif(false);
            //     return;
            // }

            if (isArmyActive)
            {
                EnableArmy(false);
                return;
            }
        }
    }
    public void OnClickSettings()
    {
        WindowManager.Instance.OpenSettingsSettings(true);
    }

    private void StartDialog(MessageDialogData dialog, DialogEndsCallback callbackOnEnd)
    {
        DialogWindow.Init(dialog, callbackOnEnd);
        GlobalMap.Block();
        CanvasGroup.interactable = false;
    }

    public override void Dispose()
    {
        PlayerByStepUI.Dispose();
        NavigationList.Dispose();
        _lastClosest = null;
        GlobalMap.Close();
        MapConsoleUI.Close();
        player.QuestData.OnElementFound -= OnElementFound;
        player.RepairData.OnSomeShipRepaired -= OnSomeShipRepaired;
        player.MoneyData.OnMoneyChange -= OnMoneyChange;
        player.MapData.OnCellChanged -= OnCellChanged;
        player.Army.OnAddShip -= OnAddShip;
        // player.ReputationData.OnReputationNationChange -= OnReputationChange;
        //        player.MapData.OnSectorChanged -= OnSectorChanged;
        //        player.MapData.OnCellChanged -= OnCellChanged;
        GlobalMap.UnBlock();
        InventoryUI.Dispose();
        DialogWindow.Dispose();
        playerArmyUI.Dispose();
        ReputationMapUI.Dispose();
        //        Cataclysm.Dispose();
        if (playerArmyUI != null)
            Destroy(playerArmyUI.gameObject);
    }

    public void ClearAll()
    {
        Dispose();
        MovingArmyUIController.ClearAll();
        _sideShipsInited = false;
        //        _stablePosCached = true;
        NavigationList.ClearAll();
        LayoutSideShips.ClearTransform();
        _sideInfos.Clear();
        GlobalMap.ClearAll();
        MapConsoleUI.ClearAll();
        QuestsOnStartController.ClearAll();
    }
}

