using System;
using UnityEngine;


public class MapNavigationList : MonoBehaviour
{
    public Transform Layout;
    public NavigationButton ButtonPrefab;
    public GlobalMapController GlobalMap;
    private bool IsScoutsInited;
    private bool _isHomeInited;
    private NavigationButton _lastSoutButton;

    public void Init(GlobalMapController globalMap)
    {
        GlobalMap = globalMap;
        //        subInit();
        InitHome();
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
    }

    private void InitButtons()
    {
        Layout.ClearTransform();
        InitHome();
        var player = MainController.Instance.MainPlayer;
        var map = player.MapData.GalaxyData;
        var cells = map.AllCells();
        int cnt = 1;
        InitScoutsButton();
        for (int i = 0; i < map.Size; i++)
        {
            for (int j = 0; j < map.Size; j++)
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
            txt = Namings.TryFormat(Namings.Tag("Complete"), c);
        }
        else
        {
            txt = Namings.TryFormat(Namings.Tag("Target"), c);

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

