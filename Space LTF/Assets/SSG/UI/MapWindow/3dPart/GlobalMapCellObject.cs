using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class GlobalMapCellObject : MonoBehaviour
{
    public GlobalMapCell Cell;

    public GameObject Unknown;
    public GameObject Fleet;
    public GameObject Shop;
    public GameObject Event;
    public GameObject Repair;
    public GameObject Destroyed;
    public GameObject ExitObject;
    public GameObject StartObject;
    public Renderer ActiveRenderer;

    private GameObject ObjectPainted;


    private bool _isArmy;
    public GameObject IAmHere;
    public Transform Container;
    public GameObject SelectedObj;
//    public GameObject CanBeTargetObject;
    public Color CompleteColor;
    private bool _IamHere;

    public Vector3 ModifiedPosition
    {
        get { return Container.position; }
    }

    void Awake()
    {
        Unknown.gameObject.SetActive(false);
        Fleet.gameObject.SetActive(false);
        Shop.gameObject.SetActive(false);
        Event.gameObject.SetActive(false);
        Repair.gameObject.SetActive(false);
        Destroyed.gameObject.SetActive(false);
        ExitObject.gameObject.SetActive(false);
        StartObject.gameObject.SetActive(false);
    }

    public void Init(GlobalMapCell cell, float cellSize)
    {
//        HistoryInfo.gameObject.SetActive(false);
//        DestroyedContainer.gameObject.SetActive(false);
        Cell = cell;
        Cell.OnDestoyedCell += OnDestoyedCell;
        Cell.OnUnconnect += OnUnconnect;
        Cell.OnScouted += OnScouted;
        Cell.OnComplete += OnComplete;
        InitMainObject();
        var c = cellSize * 0.3f;
        Container.localPosition = new Vector3(MyExtensions.Random(-c,c),0, MyExtensions.Random(-c, c));

//        var ranDElta = MainController.Instance.TimerManager.MakeTimer(MyExtensions.Random(0, 0.3f));
//        ranDElta.OnTimer += () =>
//        {
//            main.gameObject.SetActive(true);
//        };
        if (Cell.IsDestroyed)
        {
            OnDestoyedCell(cell);
        }
        UpdateConnetedGates();
    }

//    void OnEnable()
//    {
//        var timer = MainController.Instance.TimerManager.MakeTimer(MyExtensions.Random(0.1f, 1f));
//        timer.OnTimer += () => { ShipAnimator.enabled = true; };
//    }

    private void OnComplete(GlobalMapCell obj)
    {
        Unknown.gameObject.SetActive(false);
        ObjectPainted.gameObject.SetActive(true);
        if (ActiveRenderer != null)
        {
            SetColor(CompleteColor);
        }
    }

    private void SetColor(Color color)
    {
        var mats = Utils.CopyMaterials(ActiveRenderer, color);
        for (int i = 0; i < mats.Length; i++)
        {
            var mat = mats[i];
            mat.SetColor("_TintColor", CompleteColor);
        }
    }

    private void OnScouted(GlobalMapCell obj)
    {
        Unknown.gameObject.SetActive(false);
        ObjectPainted.gameObject.SetActive(true);
    }

    private void OnUnconnect(GlobalMapCell obj)
    {
        UpdateConnetedGates();
    }

    private void UpdateConnetedGates()
    {
        var shallDraw = Cell.ConnectedGates > 0;
//        CanBeTargetObject.SetActive(shallDraw);
    }

    private void InitMainObject()
    {
        var army = Cell as ArmyGlobalMapCell;
        _isArmy = army != null;
        Unknown.gameObject.SetActive(!_isArmy);
        if (_isArmy)
        {
            Fleet.gameObject.SetActive(true);
            ObjectPainted = Fleet;
            ActiveRenderer = Fleet.GetComponent<Renderer>();
            //            ObjectPainted = Fleet;
        }
        else
        {
            bool checkOnEnd = false;
            if (Cell is ShopGlobalMapCell)
            {
                ObjectPainted = Shop;
            }
            else if (Cell is RepairStationGlobalCell)
            {
                ObjectPainted = Repair;
            }
            else if (Cell is EventGlobalMapCell)
            {
                ObjectPainted = Event;
            }
            else
            {
                checkOnEnd = true;
                ObjectPainted = Unknown;
            }

            ObjectPainted.gameObject.gameObject.SetActive(false);

            if (checkOnEnd)
            {
                if (Cell is EndGlobalCell)
                {
                    Unknown.gameObject.gameObject.SetActive(false);
                    ExitObject.gameObject.gameObject.SetActive(true);
//                ObjectPainted = ExitObject;
                }
                else if (Cell is StartGlobalCell)
                {
                    Unknown.gameObject.gameObject.SetActive(false);
                    StartObject.gameObject.gameObject.SetActive(true);
//                ObjectPainted = StartObject;
                }
            }

            ActiveRenderer = ObjectPainted.GetComponent<Renderer>();
        }
#if UNITY_EDITOR
        if (ObjectPainted == null)
        {
            Debug.LogError("Wrong ObjectPainted " + Cell);
        }
        
#endif
    }

    private void OnDestoyedCell(GlobalMapCell cell)
    {
//        Container.gameObject.SetActive(!Cell.IsDestroyed);
        Destroyed.gameObject.SetActive(Cell.IsDestroyed);
        Unknown.gameObject.SetActive(false);
        ObjectPainted.gameObject.SetActive(false);
//        Destroyed.gameObject.SetActive(true);
//        _mainGameObject.SetColor(Color.black);
//        foreach (var allPartile in allPartiles)
//        {
//            allPartile.gameObject.SetActive(false);
//        }
    }

    public void SetIAmHere(bool iAmHere)
    {
//        if (Cell.InfoOpen)
//        {
//            SetColorToPS(Cell.Color());
//        }
//        else
//        {
//            SetColorToPS(Color.gray);
//        }
        IAmHere.gameObject.SetActive(iAmHere);
        if (iAmHere)
        {
            SelectedObj.gameObject.SetActive(false);
        }
//        CompletedGameObject.gameObject.SetActive(Cell.Completed);
        _IamHere = iAmHere;
    }

    public void Selected()
    {
        if (!_IamHere)
        {
            SelectedObj.gameObject.SetActive(true);
        }
//        foreach (var systemsColor in _systemsColors)
//        {
//            ParticleSystem.MainModule ma = systemsColor.Key.main;
//            ma.startColor = new ParticleSystem.MinMaxGradient(SelectColor);
//        }
    }

//    private void SetColorToPS(Color color)
//    {
//        foreach (var systemsColor in _systemsColors)
//        {
//            ParticleSystem.MainModule ma = systemsColor.Key.main;
//            ma.startColor = new ParticleSystem.MinMaxGradient(color);
//        }
//    }

    public void UnSelected()
    {
        if (!_IamHere)
        {
            SelectedObj.gameObject.SetActive(false);
        }
//        foreach (var systemsColor in _systemsColors)
//        {
//            ParticleSystem.MainModule ma = systemsColor.Key.main;
//            ma.startColor = new ParticleSystem.MinMaxGradient(systemsColor.Value);
//        }
    }

    public void Refresh()
    {
        
    }

    void OnDestroy()
    {
        if (Cell != null)
        {
            Cell.OnDestoyedCell -= OnDestoyedCell;
            Cell.OnUnconnect -= OnUnconnect;
            Cell.OnScouted -= OnScouted;
        }
    }
}

