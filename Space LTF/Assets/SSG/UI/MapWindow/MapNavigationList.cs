using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class MapNavigationList : MonoBehaviour
{
    public Transform Layout;
    public NavigationButton ButtonPrefab;
    public GlobalMapController GlobalMap;
    private bool IsScoutsInited;
    private NavigationButton _lastSoutButton;

    public void Init(GlobalMapController globalMap)
    {
        GlobalMap = globalMap;
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
        var player = MainController.Instance.MainPlayer;
        _lastSoutButton = InitScout();
        _lastSoutButton.Field.text = "Scouted";
        _lastSoutButton.gameObject.SetActive(false);
     
    }

    public void NewGameDrop()
    {
        IsScoutsInited = true;
        Layout.ClearTransform();
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
                        navigButton.Field.text = "Gate";
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

    private void SetTextCoreCell(NavigationButton navigButton, CoreGlobalMapCell cell,int c)
    {
        string txt;
        if (cell.Taken)
        {
            txt = String.Format("Complete {0}", c);
        }
        else
        {
            txt = String.Format("Target {0}", c);

        }
        navigButton.Field.text = txt;
    }

    private void InitHome()
    {
        var homeBtn = DataBaseController.GetItem(ButtonPrefab);
        homeBtn.gameObject.transform.SetParent(Layout, false);
        homeBtn.Button.onClick.AddListener(() =>
        {
            GlobalMap.SetCameraHome();
        });
        homeBtn.Field.text = "Home";
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

}

