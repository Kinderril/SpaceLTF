
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


[System.Serializable]
public abstract class GlobalMapCell
{
    protected string name;
    public bool LeaveComplete = false;
    public bool Completed = false;
    public bool InfoOpen = false;
    public bool IsDestroyed = false;
    public bool IsScouted { get; private set; }
    public int indX;
    public int indZ;
    public int SectorId = -1;
    public int ConnectedGates = -1;
    protected SectorData _sector;
    private List<GlobalMapCell> _ways = new List<GlobalMapCell>();
//    public GlobalMapCell ExtraWay = null;

    [field: NonSerialized]
    public event Action<GlobalMapCell> OnDestoyedCell;
    [field: NonSerialized]
    public event Action<GlobalMapCell> OnScouted;
    [field: NonSerialized]
    public event Action<GlobalMapCell> OnUnconnect;
    [field: NonSerialized]
    public event Action<GlobalMapCell> OnComplete;

    protected GlobalMapCell(int id,int iX,int iZ,SectorData sector)
    {
        _sector = sector;
        this.indX = iX;
        this.indZ = iZ;
        Id = id;
    }

    public int Id { get; set; }

    public abstract string Desc();

    public abstract void Take();

    [CanBeNull]
    public abstract MessageDialogData GetDialog();

    public void Complete()
    {
        Debug.Log("Map cell complete " + Id);
        Completed = true;
        Scouted();
        if (OnComplete!= null)
        {
            OnComplete(this);
        }
    }
    public void AddWay(GlobalMapCell extraWay)
    {
        _ways.Add(extraWay);
    }

    public MessageDialogData GetLeavedAction()
    {
        LeaveComplete = true;
        return GetLeavedActionInner();
    }
    public void SetConnectedCell(int coreIndex)
    {
        ConnectedGates = coreIndex;
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
        //UnconnectAll();
        IsScouted = true;
        if (!Completed)
        {
            MainController.Instance.MainPlayer.LastScoutsData.SetLastScout(this);
        }

        if (OnScouted != null)
        {
            OnScouted(this);
        }
        
    }
    public void UnconnectAll()
    {
        ConnectedGates = -1;
        if (OnUnconnect != null)
        {
            OnUnconnect(this);
        }
    }


    public abstract bool CanCellDestroy();

    public static int AddMoney(int min, int max)
    {
        int money = MyExtensions.Random(min, max);
        MainController.Instance.MainPlayer.MoneyData.AddMoney(money);
        return money;
    }

    protected bool SkillWork(int baseVal, int skillVal)
    {
        WDictionary<bool> wd = new WDictionary<bool>(new Dictionary<bool, float>()
        {
            {true,skillVal },
            {false,baseVal},
        });
        return wd.Random();
    }

    protected int ScoutsLevel { get { return MainController.Instance.MainPlayer.Parameters.Scouts.Level; } }
    protected int DiplomacyLevel { get { return MainController.Instance.MainPlayer.Parameters.Diplomaty.Level; } }
    protected int RepairLevel { get { return MainController.Instance.MainPlayer.Parameters.Repair.Level; } }
    protected int ChargesCountLevel { get { return MainController.Instance.MainPlayer.Parameters.ChargesCount.Level; } }

    public void DestroyCell()
    {
        _ways.Clear();
        IsDestroyed = true;
        if (OnDestoyedCell != null)
        {
            OnDestoyedCell(this);
        }
    }

    public void AddWayObject(GlobalMapCellConnector globalMapCellConnector)
    {
        
    }

    public void AddWays(List<GlobalMapCell> ways)
    {
        foreach (var way in ways)
        {
            _ways.Add(way);
        }
    }

    public List<GlobalMapCell> GetCurrentPosibleWays()
    {
        return _ways;
    }

    public void RemoveWayTo(GlobalMapCell rnd)
    {
        _ways.Remove(rnd);
    }

    public void VisitCell(PlayerMapData mapData)
    {
        if (_sector.ComeToSector(mapData.VisitedSectors))
        {
            mapData.VisitedSectors++;
        }
    }

    public virtual void UpdatePowers(int visitedSectors)
    {
        
    }
}

