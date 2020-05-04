using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MapNavigationList : MonoBehaviour
{
    public Transform Layout;
    public NavigationButton ButtonPrefab;
    public GlobalMapController GlobalMap { get; private set; }
    private bool IsScoutsInited;
    private bool _isHomeInited;
    private bool _isTargetInited;
    private NavigationButton _lastSoutButton;

    public void Init(GlobalMapController globalMap)
    {
        GlobalMap = globalMap;
        //        subInit();
        InitHome();
//        InitTarget();
    }

    private void subInit()
    {
        MainController.Instance.MainPlayer.QuestData.OnElementFound += OnElementFound;
        MainController.Instance.MainPlayer.LastScoutsData.OnLastScouts += OnLastScouts;

        if (!IsScoutsInited)
        {
            IsScoutsInited = true;
            InitButtons();
        }
    }

    private void OnLastScouts()
    {
        _lastSoutButton.gameObject.SetActive(true);
    }

    private void InitScoutsButton()
    {
        _lastSoutButton = InitScout();
        _lastSoutButton.Field.text = Namings.Tag("Scouted");
        _lastSoutButton.gameObject.SetActive(false);

    }

    public void NewGameDrop()
    {
        IsScoutsInited = true;
        Layout.ClearTransform();
        _isHomeInited = false;
        _isTargetInited = false;
    }

    private void InitButtons()
    {
        Layout.ClearTransform();
        InitHome();
//        InitTarget();
        var player = MainController.Instance.MainPlayer;
        var map = player.MapData.GalaxyData;
        var cells = map.AllCells();
        int cnt = 1;
        InitScoutsButton();
        for (int i = 0; i < map.SizeX; i++)
        {
            for (int j = 0; j < map.SizeZ; j++)
            {
                var c = cells[i, j];
                NavigationButton navigButton = null;
                var coreCell = c as CoreGlobalMapCell;
                if (coreCell != null)
                {
                    navigButton = InitCellButton(c);
                    SetTextCoreCell(navigButton, coreCell, cnt);
                    cnt++;
                }
                else
                {
                    var exitCell = c as EndGlobalCell;
                    if (exitCell != null)
                    {
                        navigButton = InitCellButton(exitCell);
                        navigButton.Field.text = Namings.Tag("Gate");
                    }

                }

                if (navigButton != null)
                {
                    navigButton.gameObject.SetActive(c.InfoOpen);
                }
            }
        }
        _lastSoutButton.gameObject.SetActive(player.LastScoutsData.HaveScouted);
    }

    private void SetTextCoreCell(NavigationButton navigButton, CoreGlobalMapCell cell, int c)
    {
        string txt;
        if (cell.Taken)
        {
            txt = Namings.Format(Namings.Tag("Complete"), c);
        }
        else
        {
            txt = Namings.Format(Namings.Tag("Target"), c);

        }
        navigButton.Field.text = txt;
    }

    private void InitHome()
    {
        if (_isHomeInited)
        {
            return;
        }
        var homeBtn = DataBaseController.GetItem(ButtonPrefab);
        homeBtn.gameObject.transform.SetParent(Layout, false);
        homeBtn.Button.onClick.AddListener(() =>
        {
            GlobalMap.SetCameraHome();
        });
        homeBtn.Field.text = Namings.Tag("Home");
        _isHomeInited = true;
    }  
//    private void InitTarget()
//    {
//        if (_isTargetInited)
//        {
//            return;
//        }
//        var trgBtn = DataBaseController.GetItem(ButtonPrefab);
//        trgBtn.gameObject.transform.SetParent(Layout, false);
//        trgBtn.Button.onClick.AddListener(SetCameraToClosestGoal);
//        trgBtn.Field.text = Namings.Tag("Goal");
//        _isTargetInited = true;
//    }

    public void SetCameraToClosestGoal()
    {
        var player = MainController.Instance.MainPlayer;  

        GlobalMapCell targetCEll = null;
        HashSet<GlobalMapCell> cellsToLightUp = new HashSet<GlobalMapCell>();
        Vector3 camPos;
        if (player.QuestData.Completed())
        {

            var endCEll = player.MapData.GalaxyData.GetAllList().FirstOrDefault(x=>x is EndGlobalCell || x is EndTutorGlobalCell);
            if (endCEll != null)
            {
                targetCEll = endCEll;
                cellsToLightUp.Add(endCEll);
            }

            if (targetCEll != null)
            {
                GlobalMap.SetCameraToCellHome(targetCEll);
                GlobalMap.LighterUpCells.LightUpCells(cellsToLightUp);
            }
        }
        else
        {

            var cells = player.MapData.GalaxyData.GetAllList();
            var curPLayerCell = player.MapData.CurrentCell;
            int sumDist = 9999;
            
            foreach (var cell in cells)
            {
                var coreCell = cell as CoreGlobalMapCell;
                if (coreCell != null && !coreCell.Completed)
                {
                    var dist = Mathf.Abs(cell.indX - curPLayerCell.indX) + Mathf.Abs(cell.indZ - curPLayerCell.indZ);
                    if (dist < sumDist)
                    {
                        sumDist = dist;
                        targetCEll = cell;
                    }
                }
            }

            if (targetCEll != null)
            {
                cellsToLightUp.Add(targetCEll);
                foreach (var cell in targetCEll.Sector.ListCells)
                {
                    if (cell != null && cell.Data != null && cell.Data.SectorId == targetCEll.SectorId)
                    {
                        var army = cell.Data as ArmyGlobalMapCell;
                        if (army != null)
                        {
                            cellsToLightUp.Add(army);
                        }
                    }
                }
            }

            GlobalMap.SetCameraToPosition(cellsToLightUp);
            GlobalMap.LighterUpCells.LightUpCells(cellsToLightUp);
            
        }

    }

    private NavigationButton InitCellButton(GlobalMapCell c)
    {
        var navigButton = DataBaseController.GetItem(ButtonPrefab);
        var cell = c;
        navigButton.gameObject.transform.SetParent(Layout, false);
        navigButton.Button.onClick.AddListener(() =>
        {
            GlobalMap.SetCameraToCellHome(cell);
        });
        return navigButton;
    }
    private NavigationButton InitScout()
    {
        var navigButton = DataBaseController.GetItem(ButtonPrefab);
        navigButton.gameObject.transform.SetParent(Layout, false);
        navigButton.Button.onClick.AddListener(() =>
        {
            GlobalMap.SetCameraToCellHome(MainController.Instance.MainPlayer.LastScoutsData.Cell);
        });
        return navigButton;
    }

    private void OnElementFound()
    {
        //InitButtons();
    }

    public void Dispose()
    {
        MainController.Instance.MainPlayer.LastScoutsData.OnLastScouts -= OnLastScouts;
        MainController.Instance.MainPlayer.QuestData.OnElementFound -= OnElementFound;
    }

    public void ClearAll()
    {
        NewGameDrop();
    }
}

