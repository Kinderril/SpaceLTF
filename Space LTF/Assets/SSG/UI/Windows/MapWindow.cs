using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
//    public TextMeshProUGUI SectorNameField;
    public TextMeshProUGUI MainQuestELelemntField;
    public TextMeshProUGUI ReputationField;

    public ChangingCounter MoneyField;
    public MapNavigationList NavigationList;
    public CellIinfoObjectUI CellIinfoObject;

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
            field.text = Namings.StartInfo;
        }

        PlayerByStepUI.Init(player.ByStepDamage);
        //        player.MapData.OnCellChanged += OnCellChanged;
        //        player.MapData.OnSectorChanged += OnSectorChanged;
        //        _selectedCell = data.GetNextCell(player.MapData.CurrentCell);
        MoneyField.Init(player.MoneyData.MoneyCount);
        player.MoneyData.OnMoneyChange += OnMoneyChange;
        player.MapData.OnCellChanged += OnCellChanged;
        player.OnAddShip += OnAddShip;
        player.ReputationData.OnReputationChange += OnReputationChange;
        CellsOfSector();
        InitMyArmy();
        GlobalMap.SingleInit(player.MapData.GalaxyData,this,MouseNearObject);
        GlobalMap.Open();
        List<GlobalMapCell> connectedCells = player.MapData.ConnectedCellsToCurrent();
        GlobalMap.SingleReset(player.MapData.CurrentCell, connectedCells);
        InventoryUI.Init(player.Inventory,null);
        GlobalMap.UnBlock();
        player.QuestData.OnElementFound += OnElementFound;
//        if (!player.MapData.CurrentCell.LeaveComplete)
//        {
//            if (!player.QuestData.CheckIfOver())
//            {
//                var leavedDialog = player.MapData.CurrentCell.GetLeavedAction();
//                if (leavedDialog != null)
//                {
//                    //                leavedDialog();
//                    StartDialog(leavedDialog, () =>
//                    {
//                        CanvasGroup.interactable = true;
//                        GlobalMap.UnBlock();
//                    });
//
//                }
//            }
//        }
        EnableModif(false);
        EnableArmy(false);
        UpdateMainQuestelements();
        UpdateReputation();
        InitSideShip();
    }
    private void InitSideShip()
    {
        if (!_sideShipsInited)
        {
            _sideShipsInited = true;
            foreach (var pilotData in MainController.Instance.MainPlayer.Army)
            {
                OnAddShip(pilotData, true);
            }
        }
        else
        {
            foreach (var sideShipGlobalMapInfo in _sideInfos)
            {
                sideShipGlobalMapInfo.UpToDate();
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
            var shipToDel = _sideInfos.FirstOrDefault(x => x.Ship == pilotData);
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


    private void OnReputationChange(int obj)
    {
        UpdateReputation();
    }

    public void OnStartInfoClick()
    {
        StartInfo.gameObject.SetActive(false);
    }


    private void OnElementFound()
    {
        UpdateMainQuestelements();
    }

    private void UpdateMainQuestelements()
    {
        var player = MainController.Instance.MainPlayer.QuestData;
        MainQuestELelemntField.text = $"{Namings.MainElements}:{player.mainElementsFound}/{player.MaxMainElements}";
//        NavigationList.UpdateInfo();
    }

    private void OnSomeShipRepaired()
    {
        player.MessagesToConsole.AddMsg("Ships repaired");
    }

    private void UpdateReputation()
    {
        ReputationField.text = String.Format(Namings.Reputation,player.ReputationData.Reputation);
    }


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
        player.MessagesToConsole.AddMsg(String.Format("Add credits {0}", obj));
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
        player.MessagesToConsole.AddMsg("Relocate to:" + cell.Desc());
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
                InitsMainCellDialog(obj);
            }
            else
            {
                GlobalMap.Block();
                string txt;
                if (obj.IsDestroyed)
                {
                    txt = "All destroyed. Nothing to do there.";
                }
                else
                {
                    txt = "Target is too far.";

                }
                WindowManager.Instance.InfoWindow.Init(GlobalMap.UnBlock, txt);
            }
        }
        else
        {
            InitsMainCellDialog(obj);
        }
    }

    private void InitsMainCellDialog(GlobalMapCell obj)
    {
        void ActivateDialog()
        {
            var dialog = obj.GetDialog();
            if (dialog != null)
            {                  
                if (obj.Completed && obj.OneTimeUsed() )
                {
                    WindowManager.Instance.InfoWindow.Init(OnMainDialogEnds, Namings.BeenBefore);
                }
                else
                {
                    StartDialog(dialog, OnMainDialogEnds);
                }
            }
            else
            {
                OnMainDialogEnds();
            }
        }

        if (player.MapData.GoToTarget(obj, GlobalMap, () =>
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
            StartDialog(endCell.GetDialog(), OnMainDialogEnds);
        }
        else
        {
            Debug.LogError("Can't find end game cell");
        }
    }

    private void OnMainDialogEnds()
    {
        player.MapData.CurrentCell.Complete();
        GlobalMap.SingleReset(player.MapData.CurrentCell, player.MapData.ConnectedCellsToCurrent());
        GlobalMap.UnBlock();
        CanvasGroup.interactable = true;
    }

    private void InitMyArmy()
    {
        playerArmyUI = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.PlayerArmyUIPrefab);
        playerArmyUI.Init(player, PlayerContainer,true,new ConnectInventory(player.Inventory));
    }

    public void OnArmyShowClick()
    {
        if (isArmyActive)
        {
            EnableArmy(false);
            GlobalMap.UnBlock();
        }
        else
        {
            EnableModif(false);
            EnableArmy(true);
            GlobalMap.Block();
        }
    }

    public void OnModifShowClick()
    {
        if (modifWindowUI.gameObject.activeSelf)
        {
            EnableModif(false);
            GlobalMap.UnBlock();
        }
        else
        {
            EnableArmy(false);
            EnableModif(true);
            GlobalMap.Block();
        }
    }

    private void EnableArmy(bool val)
    {
        if (val)
        {
            ArmyInfoContainer.transform.position = _stablePos;
        }
        else
        {
            if (!_stablePosCached)
            {
                _stablePosCached = true;
                _stablePos = ArmyInfoContainer.transform.position;
            }
            var v = new Vector3(5000,0,0);
            ArmyInfoContainer.transform.position = v;
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
            if (modifWindowUI.gameObject.activeSelf)
            {
                EnableModif(false);
                return;
            }

            if (isArmyActive)
            {
                EnableArmy(false);
                return;
            }
        }
    }

    private void StartDialog(MessageDialogData dialog,Action callbackOnEnd)
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
        player.OnAddShip -= OnAddShip;
        player.ReputationData.OnReputationChange -= OnReputationChange;
        //        player.MapData.OnSectorChanged -= OnSectorChanged;
        //        player.MapData.OnCellChanged -= OnCellChanged;
        GlobalMap.UnBlock();
        InventoryUI.Dispose();
        DialogWindow.Dispose();
        playerArmyUI.Dispose();
//        Cataclysm.Dispose();
        GameObject.Destroy(playerArmyUI.gameObject);
    }

    public void ClearAll()
    {
        NavigationList.ClearAll();
        LayoutSideShips.ClearTransform();
        _sideInfos.Clear();
        GlobalMap.ClearAll();
        MapConsoleUI.ClearAll();
        Dispose();
    }
}

