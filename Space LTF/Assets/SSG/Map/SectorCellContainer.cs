using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SectorCellContainer
{
    public int indX;
    public int indZ;
    public GlobalMapCell Data { get; private set; }
    [field: NonSerialized] public event Action<SectorCellContainer> OnDataChanged;
    public CellArmyContainer CurMovingArmy = new CellArmyContainer();
    private HashSet<SectorCellContainer> _ways = new HashSet<SectorCellContainer>();

    public SectorCellContainer(int x,int z)
    {
        Data = null;
        indX = x;
        indZ = z;
    }

    public void SetData(GlobalMapCell data)
    {
        Data = data;
        data.SetContainer(this);
        OnDataChanged?.Invoke(this);
        GlobalEventDispatcher.CellDataChanged(this);
    }


    public bool IsFreeToPopulate()
    {
        return Data == null;
    }

    public void AddWay(SectorCellContainer extraWay)
    {

        if (extraWay != null)
        {
            _ways.Add(extraWay);
        }
        else
        {
            Debug.LogError("Try add null way");
        }

    }

    public void AddWays(List<SectorCellContainer> ways)
    {

        foreach (var way in ways)
        {
            if (way != null)
            {
                _ways.Add(way);
            }
            else
            {
                Debug.LogError("Try add null way");
            }
        }
    }
    public HashSet<SectorCellContainer> GetCurrentPosibleWays()
    {
        var ways = new HashSet<SectorCellContainer>();
        foreach (var globalMapCell in _ways)
        {
            if (!globalMapCell.Data.IsHide)
            {
                ways.Add(globalMapCell);
            }
        }
        return ways;
    }

    public HashSet<SectorCellContainer> GetAllPosibleWays()
    {
        return _ways;
    }

    public void RemoveWayTo(SectorCellContainer rnd)
    {
        _ways.Remove(rnd);
    }
}
