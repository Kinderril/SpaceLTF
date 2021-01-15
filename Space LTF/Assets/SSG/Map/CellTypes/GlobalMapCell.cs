using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum EBattleType
{
    standart,
    defenceWaves,
    destroyShipPeriod,
    defenceOfShip,
    massiveFight,
    baseDefence,

}

[Serializable]
public abstract class GlobalMapCell
{
    protected SectorData _sector;

    public SectorData Sector => _sector;
    public CellArmyContainer CurMovingArmy
    {
        get
        {
            if (Container == null)
            {
                Debug.LogError($"cell id:{Id} null Container:{indX}  {indZ}");
            }
            return Container.CurMovingArmy;
        }
    }

    protected EBattlefildEventType? _eventType = null;
    public EBattlefildEventType? EventType => _eventType;
    public SectorCellContainer Container { get; private set; }

    public int ConnectedGates = -1;
    public int indX;
    public int indZ;
    public bool InfoOpen;
    public bool IsDestroyed;
    private bool _isHide = false;
    public bool IsHide => _isHide;
    public bool LeavedDialogComplete = false;
    protected string name;

    [System.NonSerialized]
    private Func<MessageDialogData> _leaverDialogData = null;


    protected GlobalMapCell(int id, int iX, int iZ, SectorData sector, ShipConfig config)
    {
        _sector = sector;
        indX = iX;
        ConfigOwner = config;
        indZ = iZ;
        Id = id;
        //        if (true)
        if (sector.AnyEvent || indX > 5)
        {
            if (MyExtensions.IsTrue01(0.3f))
            {
                WDictionary<EBattlefildEventType> chance = new WDictionary<EBattlefildEventType>(
                    new Dictionary<EBattlefildEventType, float>()
                    {
                        {EBattlefildEventType.asteroids, 1f},
                        {EBattlefildEventType.shieldsOff, 1f},     
                        {EBattlefildEventType.IceZone, 1f},
//                        {EBattlefildEventType.BlackHole, 1f},
                        {EBattlefildEventType.fireVortex, 1f},
                        {EBattlefildEventType.Vortex, 1f},
                    });
                _eventType = chance.Random();

            }

        }
#if UNITY_EDITOR
//        _eventType = EBattlefildEventType.fireVortex;
#endif
        //        if (indZ == 12)
        //        {
        //            Debug.LogError("sas");
        //        }
    }

    public ShipConfig ConfigOwner { get; private set; }
    public bool Completed { get; private set; }
    public bool IsScouted { get; private set; }
    public int SectorId => _sector.Id;

    public int Id { get; set; }

    protected int ScoutsLevel => MainController.Instance.MainPlayer.Parameters.Scouts.Level;
    protected int RepairLevel => MainController.Instance.MainPlayer.Parameters.Repair.Level;
    protected int ChargesCountLevel => MainController.Instance.MainPlayer.Parameters.ChargesCount.Level;


    [field: NonSerialized] public event Action<GlobalMapCell> OnDestoyedCell;

    [field: NonSerialized] public event Action<GlobalMapCell> OnScouted;

    [field: NonSerialized] public event Action<GlobalMapCell> OnUnconnect;

    [field: NonSerialized] public event Action<GlobalMapCell, bool> OnComplete;
    [field: NonSerialized] public event Action<GlobalMapCell, bool> OnHide;

    public abstract string Desc();

    public abstract void Take();

    [CanBeNull]
    protected abstract MessageDialogData GetDialog();

    public MessageDialogData GetDialogMain(out bool activateAnyway)
    {
        Debug.Log($"Activate dialog: {this.ToString()}.");
        var dilaog = CurMovingArmy.GetDialog(FightMovingArmy, GetDialog());
        if (dilaog != null)
        {
            activateAnyway = true;
            return dilaog;
        }

        var dialog = GetDialog();
        if (dialog != null)
        {
            activateAnyway = dialog.ActivateAnyWay();
        }
        else
        {
            activateAnyway = false;
        }
        return dialog;
    }


    public void Unhide()
    {
        _isHide = false;
        OnHide?.Invoke(this,_isHide);
    }

    public void Hide()
    {
        _isHide = true;
        OnHide?.Invoke(this, _isHide);
    }

    public virtual bool CanGotFromIt(bool withAction)
    {
        return true;
    }
    private void FightMovingArmy()
    {
        _leaverDialogData = CurMovingArmy.MoverArmyLeaverEnd;
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer, CurMovingArmy.GetArmyToFight(), false, true);
    }



    public virtual void Complete()
    {
        Completed = true;
        Scouted();
        OnComplete?.Invoke(this, true);
    }

    public void Uncomplete()
    {
        Completed = false;
        OnComplete?.Invoke(this, false);
    }

    public void AddWay(GlobalMapCell extraWay)
    {
        Container.AddWay(extraWay);
    }

    public void SetConnectedCell(int coreIndex)
    {
        ConnectedGates = coreIndex;
    }

    public MessageDialogData GetLeavedActionInnerMain()
    {
        if (_leaverDialogData != null)
        {
            var dialog = _leaverDialogData;
            _leaverDialogData = null;
            return dialog();
        }
        return GetLeavedActionInner();
    }

    protected virtual MessageDialogData GetLeavedActionInner()
    {
        return null;
    }

    public virtual void OpenInfo()
    {
        InfoOpen = true;
    }

    public abstract Color Color();

    public abstract bool OneTimeUsed();

    public virtual void ComeTo(GlobalMapCell from)
    {

    }

    public virtual void Scouted()
    {
        IsScouted = true;
        if (!Completed)
            MainController.Instance.MainPlayer.LastScoutsData.SetLastScout(this);
        OnScouted?.Invoke(this);
    }

    public void UnconnectAll()
    {
        ConnectedGates = -1;
        if (OnUnconnect != null) OnUnconnect(this);
    }


    public abstract bool CanCellDestroy();

    public static int AddMoney(int min, int max)
    {
        var money = MyExtensions.Random(min, max);
        MainController.Instance.MainPlayer.MoneyData.AddMoney(money);
        return money;
    }

    public void SetContainer(SectorCellContainer cellContainer)
    {
        Container = cellContainer;
    }

    protected bool SkillWork(int baseVal, int skillVal)
    {
        var wd = new WDictionary<bool>(new Dictionary<bool, float>
        {
            {true, skillVal},
            {false, baseVal}
        });
        return wd.Random();
    }
    protected MessageDialogData TryRequestSmt(int cost, Action doRequest, string mainMsgKey)
    {
        var ans = new List<AnswerDialogData>();
        string masinMsg;

        var player = MainController.Instance.MainPlayer;
        if (player.MoneyData.HaveMoney(cost))
        {
            player.MoneyData.RemoveMoney(cost);
            doRequest();
            masinMsg = Namings.Tag(mainMsgKey);
        }
        else
        {
            masinMsg = Namings.Tag("NotEnoughtMoney");
        }

        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        var mesData = new MessageDialogData(masinMsg, ans);
        return mesData;
    }
//    public void DestroyCell()
//    {
//        _ways.Clear();
//        IsDestroyed = true;
//        if (OnDestoyedCell != null) OnDestoyedCell(this);
//    }

    public void AddWayObject(GlobalMapCellConnector globalMapCellConnector)
    {
    }

    public void AddWays(List<GlobalMapCell> ways)
    {
        Container.AddWays(ways);
    }

    public HashSet<GlobalMapCell> GetCurrentPosibleWays()
    {
        return Container.GetCurrentPosibleWays();
    }

    public HashSet<GlobalMapCell> GetAllPosibleWays()
    {
        return Container.GetAllPosibleWays();
    }

    public void RemoveWayTo(GlobalMapCell rnd)
    {
        Container.RemoveWayTo(rnd);
    }

    public void VisitCell(PlayerMapData mapData, int step)
    {

        if (!CurMovingArmy.HaveArmy() )
        {
            var army = this as ArmyBornCenterGlobalCell;
            if (army != null)
            {
                if (army.FightComplete)
                {
                    Complete();
                }
            }
            else
            {
                Complete();
            }
        }
        if (_sector.ComeToSector())
        {
            mapData.VisitedSectors++;
        }

    }

    public virtual void UpdateStep(int step)
    {

    }

    public virtual void UpdateAdditionalPower(int additiopnalPower)
    {
    }
    public  virtual void LeaveFromCell()
    {

    }

    public override string ToString()
    {
        return $"CEll:{base.ToString()} X:{indX} Z:{indZ}";
    }

    public virtual void UpdateCollectedPower(float powerDelta)
    {
        
    }

    public virtual bool IsPossibleToChange()
    {
        return false;
    }
}