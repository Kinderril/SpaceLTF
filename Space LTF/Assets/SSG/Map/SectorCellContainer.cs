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
    private HashSet<GlobalMapCell> _ways = new HashSet<GlobalMapCell>();

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
    }


    public bool IsFreeToPopulate()
    {
        return Data == null;
    }

    public void AddWay(GlobalMapCell extraWay)
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

    public void AddWays(List<GlobalMapCell> ways)
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
    public HashSet<GlobalMapCell> GetCurrentPosibleWays()
    {
        var ways = new HashSet<GlobalMapCell>();
        foreach (var globalMapCell in _ways)
        {
            if (!globalMapCell.IsHide)
            {
                ways.Add(globalMapCell);
            }
        }
        return ways;
    }

    public HashSet<GlobalMapCell> GetAllPosibleWays()
    {
        return _ways;
    }

    public void RemoveWayTo(GlobalMapCell rnd)
    {
        _ways.Remove(rnd);
    }
}
