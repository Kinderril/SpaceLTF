using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class GlobalMapCell
{
    protected SectorData _sector;

    private List<GlobalMapCell> _ways = new List<GlobalMapCell>();
    public MovingArmy CurMovingArmy = null;

    public int ConnectedGates = -1;
    public int indX;
    public int indZ;
    public bool InfoOpen;
    public bool IsDestroyed;
    public bool LeavedDialogComplete = false;
    protected string name;

    [System.NonSerialized]
    private MessageDialogData _leaverDialogData = null;


    protected GlobalMapCell(int id, int iX, int iZ, SectorData sector, ShipConfig config)
    {
        _sector = sector;
        indX = iX;
        ConfigOwner = config;
        indZ = iZ;
        Id = id;
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

    public abstract string Desc();

    public abstract void Take();

    [CanBeNull]
    protected abstract MessageDialogData GetDialog();

    public MessageDialogData GetDialogMain()
    {
        if (CurMovingArmy != null)
        {
            return DialogMovingArmy();
        }
        return GetDialog();
    }

    private MessageDialogData DialogMovingArmy()
    {
        var ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.DialogTag("MovingArmyFight"), FightMovingArmy,  null,false,false),
        };
        var mesData = new MessageDialogData(Namings.DialogTag("MovingArmyStart"), ans);
        return mesData;
    }


    private void FightMovingArmy()
    {
        _leaverDialogData = MoverArmyLeaverEnd();
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer, CurMovingArmy._player, false, false);
    }

    private MessageDialogData MoverArmyLeaverEnd()
    {
        var movingArmy = CurMovingArmy;
        var ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.DialogTag("MovingArmyGerReward"), movingArmy.GetRewardsItems,  null,false,false),
        };
        var mesData = new MessageDialogData(Namings.DialogTag("MovingArmyWin"), ans);
        return mesData;

    }

    public void Complete()
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
        _ways.Add(extraWay);
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
            return dialog;
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

    public virtual void ComeTo()
    {
    }

    public virtual void Scouted()
    {
        //        Debug.LogError($"Scouted {Id}");
        //UnconnectAll();
        IsScouted = true;
        if (!Completed) MainController.Instance.MainPlayer.LastScoutsData.SetLastScout(this);

        if (OnScouted != null) OnScouted(this);
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


    protected bool SkillWork(int baseVal, int skillVal)
    {
        var wd = new WDictionary<bool>(new Dictionary<bool, float>
        {
            {true, skillVal},
            {false, baseVal}
        });
        return wd.Random();
    }

    public void DestroyCell()
    {
        _ways.Clear();
        IsDestroyed = true;
        if (OnDestoyedCell != null) OnDestoyedCell(this);
    }

    public void AddWayObject(GlobalMapCellConnector globalMapCellConnector)
    {
    }

    public void AddWays(List<GlobalMapCell> ways)
    {
        foreach (var way in ways) _ways.Add(way);
    }

    public List<GlobalMapCell> GetCurrentPosibleWays()
    {
        return _ways;
    }

    public void RemoveWayTo(GlobalMapCell rnd)
    {
        _ways.Remove(rnd);
    }

    public void VisitCell(PlayerMapData mapData, int step)
    {
        if (_sector.ComeToSector(mapData.VisitedSectors, step)) mapData.VisitedSectors++;
    }

    public virtual void UpdatePowers(int visitedSectors, int startPower, int additionalPower)
    {
    }
}