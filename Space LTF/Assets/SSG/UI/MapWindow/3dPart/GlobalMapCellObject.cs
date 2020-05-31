﻿using UnityEngine;


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
    public GameObject AsteroidsEvent;
    public GameObject EMPSurgeEvent;
    public GameObject FireVortexEvent;
    public GameObject VortexEvent;
    public GameObject IceZoneEvent;
    public GameObject BlackHoleEvent;
    public GameObject StartDungeon;
    public GameObject QuestInfo;
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
        AsteroidsEvent.gameObject.SetActive(false);
        EMPSurgeEvent.gameObject.SetActive(false);
        StartDungeon.gameObject.SetActive(false);

        FireVortexEvent.SetActive(false);
        VortexEvent.SetActive(false);
        IceZoneEvent.SetActive(false);
        BlackHoleEvent.SetActive(false);
        QuestInfo.SetActive(false);


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
        var armyCell = Cell as FreeActionGlobalMapCell;
        if (armyCell != null)
        {
            armyCell.OnQuestDialogChanges += OnQuestDialogChanges;
        }
        Container.localPosition = new Vector3(MyExtensions.Random(-c, c), 0, MyExtensions.Random(-c, c));

        //        var ranDElta = MainController.Instance.TimerManager.MakeTimer(MyExtensions.Random(0, 0.3f));
        //        ranDElta.OnTimer += () =>
        //        {
        //            main.gameObject.SetActive(true);
        //        };
        if (Cell.IsDestroyed)
        {
            OnDestoyedCell(cell);
        }

        UpdateQuestInfo();

    }

    private void OnQuestDialogChanges(bool obj)
    {
        UpdateQuestInfo();
    }

    private void UpdateQuestInfo()
    {
        var armyCell = Cell as FreeActionGlobalMapCell;
        if (armyCell != null)
        {
            var haevDialog = armyCell.HaveQuest;
            QuestInfo.SetActive(haevDialog);
        }

    }

    //    void OnEnable()
    //    {
    //        var timer = MainController.Instance.TimerManager.MakeTimer(MyExtensions.Random(0.1f, 1f));
    //        timer.OnTimer += () => { ShipAnimator.enabled = true; };
    //    }

    private void OnComplete(GlobalMapCell obj, bool isComplete)
    {
        Unknown.gameObject.SetActive(false);
        if (ObjectPainted != null)
            ObjectPainted.gameObject.SetActive(true);
        if (ActiveRenderer != null)
        {
            SetColor(CompleteColor);
        }
        TryOpenBattleEvent();
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
        if (ObjectPainted != null)
            ObjectPainted.gameObject.SetActive(true);
        TryOpenBattleEvent();
    }

    public void TryOpenBattleEvent()
    {
        if (MainController.Instance.MainPlayer.Parameters.Scouts.Level >= 3)
        {
            if (Cell.EventType.HasValue)
            {
                switch (Cell.EventType)
                {
                    case EBattlefildEventType.asteroids:
                        AsteroidsEvent.gameObject.SetActive(true);
                        break;
                    case EBattlefildEventType.shieldsOff:
                        EMPSurgeEvent.gameObject.SetActive(true);
                        break;
                    // case EBattlefildEventType.engineOff:
                    //     break;
                    case EBattlefildEventType.fireVortex:
                        FireVortexEvent.gameObject.SetActive(true);
                        break;
                    case EBattlefildEventType.Vortex:
                        VortexEvent.gameObject.SetActive(true);
                        break;
                    case EBattlefildEventType.IceZone:
                        IceZoneEvent.gameObject.SetActive(true);
                        break;
                    case EBattlefildEventType.BlackHole:
                        BlackHoleEvent.gameObject.SetActive(true);
                        break;
                }
            }
        }
    }

    private void OnUnconnect(GlobalMapCell obj)
    {

    }

    private void InitMainObject()
    {
        var army = Cell as ArmyGlobalMapCell;
        _isArmy = army != null;
        Unknown.gameObject.SetActive(!_isArmy);
        if (_isArmy)
        {
            ActiveRenderer = Fleet.GetComponent<Renderer>();
//            if (Cell is CoreGlobalMapCell)
//            {
//                Unknown.gameObject.SetActive(true);
//                ObjectPainted = Unknown;
////                Unknown = StartDungeon;
//            }
//            else
            if (Cell is EndGlobalCell || Cell is EndTutorGlobalCell)
            {
                Unknown.gameObject.gameObject.SetActive(false);
                ExitObject.gameObject.SetActive(true);
                ObjectPainted = ExitObject;
            }
            else  if (Cell is ArmyDungeonEnterGlobalMapCell || Cell is ArmyDungeonExitGlobalMapCell || Cell is ArmyDungeonGlobalMapCell)
            {
                StartDungeon.gameObject.SetActive(true);
                ObjectPainted = StartDungeon;
                StartDungeon = Fleet;
            }
            else
            {
                Fleet.gameObject.SetActive(true);
                ObjectPainted = Fleet;
            }
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
            else if (Cell is EndTutorGlobalCell)
            {
                Unknown.gameObject.gameObject.SetActive(false);
                ExitObject.gameObject.gameObject.SetActive(true);
                ObjectPainted = Unknown;
            }
            else
            {
                checkOnEnd = true;
                ObjectPainted = Unknown;
            }

            ObjectPainted.gameObject.gameObject.SetActive(false);

            if (checkOnEnd)
            {
                if (Cell is StartGlobalCell)
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
        Destroyed.gameObject.SetActive(Cell.IsDestroyed);
        Unknown.gameObject.SetActive(false);
        if (ObjectPainted != null)
            ObjectPainted.gameObject.SetActive(false);
    }

    public void SetIAmHere(bool iAmHere)
    {
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
    }

    public void UnSelected()
    {
        if (!_IamHere)
        {
            SelectedObj.gameObject.SetActive(false);
        }
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

