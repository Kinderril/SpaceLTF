using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class SectorCellContainer
{
    public int indX;
    public int indZ;
    public GlobalMapCell Data { get; private set; }
    [field: NonSerialized] public event Action<SectorCellContainer> OnDataChanged;
    public CellArmyContainer CurMovingArmy = new CellArmyContainer();

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
}
